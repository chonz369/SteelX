using NetworkCompression;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Profiling;

public interface INetworkClientCallbacks : INetworkCallbacks
{
    void OnMapUpdate(ref NetworkReader reader);
}

public interface ISnapshotConsumer
{
    void ProcessEntityDespawns(int serverTime, List<int> despawns);
    void ProcessEntitySpawn(int serverTime, int id, ushort typeId);
    void ProcessEntityUpdate(int serverTime, int id, ref NetworkReader reader);
}

public class NetworkClient
{
    [ConfigVar(Name = "client.debug", DefaultValue = "0", Description = "Enable debug printing of client handshake etc.", Flags = ConfigVar.Flags.None)]
    public static ConfigVar clientDebug;

    public delegate void DataGenerator(ref NetworkWriter data);

    public enum ConnectionState
    {
        Disconnected,
        Connecting,
        Connected,//client is connected if received client info
    }

    protected ConnectionState connectionState { get {
        return clientConnection == null ? ConnectionState.Disconnected : clientConnection.connectionState;
    }}

    // Sent from client to server when changed
    public class ClientConfig
    {
        public int serverUpdateRate;            // max bytes/sec
        public int serverUpdateInterval;        // requested tick / update
    }

    public class ClientPackageInfo : PackageInfo
    {
        public int commandTime;
        public int commandSequence;

        public override void Reset() {
            base.Reset();
            commandTime = 0;
            commandSequence = 0;
        }
    }

    public class Counters : NetworkConnectionCounters
    {
        public int snapshotsIn;             // Total number of snapshots received
        public int fullSnapshotsIn;         // Number of snapshots without a baseline
        public int commandsOut;             // Number of command messages sent
    }

    class EntityTypeInfo
    {
        public ushort typeId;
        public NetworkSchema schema;
        public uint[] baseline;
    }

    class EntityInfo
    {
        //public int id;
        public EntityTypeInfo type;
        public int lastUpdateSequence;
        public uint[] lastUpdate = new uint[NetworkConfig.maxEntitySnapshotDataSize];
        public int despawnSequence;
        public byte fieldMask;
        public uint[] prediction = new uint[NetworkConfig.maxEntitySnapshotDataSize];
        public byte[] fieldsChangedPrediction = new byte[(NetworkConfig.maxFieldsPerSchema + 7) / 8];

        public void Reset() {
            type = null;
            lastUpdateSequence = 0;
            despawnSequence = 0;
            fieldMask = 0;
            // TODO needed?
            for (var i = 0; i < lastUpdate.Length; i++)
                lastUpdate[i] = 0;

            baselines.Clear();
        }

        public SparseSequenceBuffer baselines = new SparseSequenceBuffer(
            NetworkConfig.snapshotDeltaCacheSize, NetworkConfig.maxEntitySnapshotDataSize);
    }

    class CommandInfo
    {
        public int time = 0;
        public uint[] data = new uint[512];
    }

    private INetworkTransport _transport;
    private ClientConfig _clientConfig;
    public int clientId { get { return clientConnection != null ? clientConnection.ConnectionId : -1; } }
    public int serverTime { get { return clientConnection != null ? clientConnection.serverTime : -1; } }
    public int serverTickRate { get { return clientConnection != null ? clientConnection.serverTickRate : 60; } }
    public int rtt { get { return clientConnection != null ? clientConnection.rtt : 0; } }
    public float timeSinceSnapshot { get { return clientConnection != null ? NetworkUtils.stopwatch.ElapsedMilliseconds - clientConnection.snapshotReceivedTime : -1; } }
    public int lastAcknowlegdedCommandTime { get { return clientConnection != null ? clientConnection.lastAcknowlegdedCommandTime : -1; } }

    public ClientConnection clientConnection;
    Dictionary<ushort, NetworkEventType> m_EventTypesOut = new Dictionary<ushort, NetworkEventType>();

    public bool IsConnected { get { return connectionState == ConnectionState.Connected; } }

    public NetworkClient(INetworkTransport transport) {
        _transport = transport;
        _clientConfig = new ClientConfig();
    }

    internal void UpdateClientConfig() {
        Profiler.BeginSample("NetworkClient.UpdateClientConfig");

        _clientConfig.serverUpdateRate = ClientGameLoop.clientUpdateRate.IntValue;
        _clientConfig.serverUpdateInterval = ClientGameLoop.clientUpdateInterval.IntValue;
        if (clientConnection != null)
            clientConnection.ClientConfigChanged();

        Profiler.EndSample();
    }

    public void Disconnect() {
        _transport.Disconnect();
    }

    public void Connect() {
        GameDebug.Assert(connectionState == ConnectionState.Disconnected);
        GameDebug.Assert(clientConnection == null);

        _transport.Connect();

        clientConnection = new ClientConnection(0, _transport, _clientConfig);//0 is temp. , wait server send our id
    }

    public void QueueCommand(int time, DataGenerator generator) {
        if (clientConnection == null)
            return;

        clientConnection.QueueCommand(time, generator);
    }

    public void Update(INetworkClientCallbacks clientNetworkConsumer, ISnapshotConsumer snapshotConsumer) {
        _transport.Update();

        TransportEvent e = new TransportEvent();
        while(_transport.NextEvent(ref e)) {
            switch (e.type) {
                case TransportEvent.Type.Connect:
                OnConnect(e.ConnectionId);
                break;
                case TransportEvent.Type.Disconnect:
                OnDisconnect(e.ConnectionId);
                break;
                case TransportEvent.Type.Data:
                OnData(e.Data, clientNetworkConsumer, snapshotConsumer);
                break;
            }
        }

        if (clientConnection != null)
            clientConnection.ProcessMapUpdate(clientNetworkConsumer);
    }

    public void SendData() {
        if (clientConnection == null)
            return;

        clientConnection.SendPackage();
    }

    public void QueueEvent(ushort typeId, bool reliable, NetworkEventGenerator generator) {
        if (clientConnection == null)
            return;

        var e = NetworkEvent.Serialize(typeId, reliable, m_EventTypesOut, generator);
        clientConnection.QueueEvent(e);
        e.Release();
    }

    public void OnData(byte[] data, INetworkClientCallbacks clientNetworkConsumer, ISnapshotConsumer snapshotConsumer) {
        if (clientConnection == null)
            return;

        clientConnection.ReadPackage(data, snapshotConsumer, clientNetworkConsumer);
    }

    public void OnConnect(int connectionId) {//connect to photon
        GameDebug.Assert(connectionState == ConnectionState.Connecting);
    }

    public void OnDisconnect(int connectionId) {
        if (clientConnection == null) return;
        
        GameDebug.Log("Disconnected");
        clientConnection = null;
    }

    public class ClientConnection : NetworkConnection<ClientPackageInfo, NetworkClient.Counters>
    {
        public ConnectionState connectionState = ConnectionState.Connecting;
        private ClientConfig _clientConfig;

        public long snapshotReceivedTime;               // Time we received the last snapshot
        public float serverSimTime;                     // Server simulation time (actualy time spent doing simulation regardless of tickrate)

        public ClientConnection(int connectionId, INetworkTransport transport, ClientConfig clientConfig) :  base(connectionId, transport){
            _clientConfig = clientConfig;
        }

        public void Reset() {
            serverTime = 0;
            entities.Clear();
            spawns.Clear();
            despawns.Clear();
            updates.Clear();
        }

        public void ClientConfigChanged() {
            sendClientConfig = true;
        }

        unsafe public void ProcessMapUpdate(INetworkClientCallbacks loop) {
            if (mapInfo.mapSequence > 0 && !mapInfo.processed) {
                fixed (uint* data = mapInfo.data) {
                    var reader = new NetworkReader(data, mapInfo.schema);
                    loop.OnMapUpdate(ref reader);
                    mapInfo.processed = true;
                }
            }
        }

        byte[] packageBuffer = new byte[NetworkConfig.MaxPackageSize];
        public void ReadPackage(byte[] packageData, ISnapshotConsumer snapshotConsumer, INetworkCallbacks networkClientConsumer) {
            counters.bytesIn += packageData.Length;

            NetworkUtils.MemCopy(packageData, 0, packageBuffer, 0, packageData.Length);

            NetworkMessage content;
            int headerSize;
            var packageSequence = ProcessPackageHeader(packageBuffer, out content, out headerSize);
            // The package was dropped (duplicate or too old) or if it was a fragment not yet assembled, bail out here
            if (packageSequence == 0) {
                return;
            }

            var input = new RawInputStream();
            input.Initialize(packageBuffer, headerSize);

            if ((content & NetworkMessage.ClientInfo) != 0)
                ReadClientInfo(ref input);

            if ((content & NetworkMessage.MapInfo) != 0)
                ReadMapInfo(ref input);

            if ((content & NetworkMessage.Snapshot) != 0) {
                ReadSnapshot(packageSequence, ref input, snapshotConsumer);

                // Make sure the callback actually picked up the snapshot data. It is important that
                // every snapshot gets processed by the game so that the spawns, despawns and updates lists
                // does not end up containing stuff from different snapshots
                //GameDebug.Assert(spawns.Count == 0 && despawns.Count == 0 && updates.Count == 0, "Game did not consume snapshots");
            }

            if ((content & NetworkMessage.Events) != 0)
                ReadEvents(ref input, networkClientConsumer);
        }

        public void ReadClientInfo(ref RawInputStream input) {
            var newClientId = (int)input.ReadRawBits(8);
            serverTickRate = (int)input.ReadRawBits(8);

            if (connectionState == ConnectionState.Connected) return;

            ConnectionId = newClientId;
            GameDebug.Log("Client received client info");

            connectionState = ConnectionState.Connected;
        }

        void ReadMapInfo(ref RawInputStream input){
            //input.SetStatsType(NetworkCompressionReader.Type.MapInfo);
            var mapSequence = (ushort)input.ReadRawBits(16);
            var schemaIncluded = input.ReadRawBits(1) != 0;
            if (schemaIncluded) {
                mapInfo.schema = NetworkSchema.ReadSchema(ref input);   // might override previous definition
            }

            if (mapSequence > mapInfo.mapSequence) {
                mapInfo.mapSequence = mapSequence;
                mapInfo.ackSequence = inSequence;
                mapInfo.processed = false;
                NetworkSchema.CopyFieldsToBuffer(mapInfo.schema, ref input, mapInfo.data);
            } else
                NetworkSchema.SkipFields(mapInfo.schema, ref input);
        }

        uint[] tempSnapshotBuffer = new uint[NetworkConfig.maxEntitySnapshotDataSize];
        unsafe void ReadSnapshot(int sequence, ref RawInputStream input, ISnapshotConsumer consumer) {
            var haveBaseline = input.ReadRawBits(1) == 1;
            var baseSequence = (int)input.ReadPackedIntDelta(sequence - 1, NetworkConfig.baseSequenceContext);

            bool enableNetworkPrediction = input.ReadRawBits(1) != 0;

            int baseSequence1 = 0;
            int baseSequence2 = 0;
            if (enableNetworkPrediction) {
                baseSequence1 = (int)input.ReadPackedIntDelta(baseSequence - 1, NetworkConfig.baseSequence1Context);
                baseSequence2 = (int)input.ReadPackedIntDelta(baseSequence1 - 1, NetworkConfig.baseSequence2Context);
            }

            var snapshotInfo = snapshots.Acquire(sequence);
            snapshotInfo.serverTime = (int)input.ReadPackedIntDelta(haveBaseline ? snapshots[baseSequence].serverTime : 0, NetworkConfig.serverTimeContext);
            //GameDebug.Log("baseSequence : " + baseSequence + "server time:" + (haveBaseline ? snapshots[baseSequence].serverTime.ToString() : ""));

            var temp = (int)input.ReadRawBits(8);
            serverSimTime = temp * 0.1f;

            // Only update time if received in-order.. 
            // TODO consider dropping out of order snapshots
            // TODO detecting out-of-order on pack sequences
            if (snapshotInfo.serverTime > serverTime) {
                serverTime = snapshotInfo.serverTime;
                snapshotReceivedTime = NetworkUtils.stopwatch.ElapsedMilliseconds;
            } else {
                GameDebug.Log(string.Format("NetworkClient. Dropping out of order snaphot. Server time:{0} snapshot time:{1}", serverTime, snapshotInfo.serverTime));
            }

            // Read schemas
            var schemaCount = input.ReadPackedUInt(NetworkConfig.schemaCountContext);
            for (int schemaIndex = 0; schemaIndex < schemaCount; ++schemaIndex) {
                var typeId = (ushort)input.ReadPackedUInt(NetworkConfig.schemaTypeIdContext);

                var entityType = new EntityTypeInfo() { typeId = typeId };
                entityType.schema = NetworkSchema.ReadSchema(ref input);
                //counters.AddSectionStats("snapShotSchemas", input.GetBitPosition2(), new Color(0.0f, (schemaIndex & 1) == 1 ? 0.5f : 1.0f, 1.0f));
                entityType.baseline = new uint[NetworkConfig.maxEntitySnapshotDataSize];
                NetworkSchema.CopyFieldsToBuffer(entityType.schema, ref input, entityType.baseline);

                if (!entityTypes.ContainsKey(typeId))
                    entityTypes.Add(typeId, entityType);

                //counters.AddSectionStats("snapShotSchemas", input.GetBitPosition2(), new Color(1.0f, (schemaIndex & 1) == 1 ? 0.5f : 1.0f, 1.0f));
            }

            // Remove any despawning entities that belong to older base sequences
            for (int i = 0; i < entities.Count; i++) {
                var e = entities[i];
                if (e.type == null)
                    continue;
                if (e.despawnSequence > 0 && e.despawnSequence <= baseSequence)
                    e.Reset();
            }

            // Read new spawns
            m_TempSpawnList.Clear();
            var previousId = 1;
            var spawnCount = input.ReadPackedUInt(NetworkConfig.spawnCountContext);
            for (var spawnIndex = 0; spawnIndex < spawnCount; ++spawnIndex) {
                var id = (int)input.ReadPackedIntDelta(previousId, NetworkConfig.idContext);
                previousId = id;

                // Register the entity
                var typeId = (ushort)input.ReadPackedUInt(NetworkConfig.spawnTypeIdContext);    //TODO: use another encoding
                GameDebug.Assert(entityTypes.ContainsKey(typeId), "Spawn request with unknown type id {0}", typeId);

                byte fieldMask = (byte)input.ReadRawBits(8);

                // TODO need an max entity id for safety
                while (id >= entities.Count)
                    entities.Add(new EntityInfo());

                // Incoming spawn of different type than what we have for this id, so immediately nuke
                // the one we have to make room for the incoming
                if (entities[id].type != null && entities[id].type.typeId != typeId) {
                    // This should only ever happen in case of no baseline as normally the server will
                    // not reuse an id before all clients have acknowledged its despawn.
                    GameDebug.Assert(haveBaseline == false, "Spawning entity but we already have with different type?");
                    GameDebug.Log("REPLACING old entity: " + id + " because snapshot gave us new type for this id");
                    despawns.Add(id);
                    entities[id].Reset();
                }

                // We can receive spawn information in several snapshots before our ack
                // has reached the server. Only pass on spawn to game layer once
                if (entities[id].type == null) {
                    var e = entities[id];
                    e.type = entityTypes[typeId];
                    e.fieldMask = fieldMask;
                    spawns.Add(id);
                }

                m_TempSpawnList.Add(id);
            }

            //counters.AddSectionStats("snapShotSpawns", input.GetBitPosition2(), new Color(0, 0.58f, 0));

            // Read despawns
            var despawnCount = input.ReadPackedUInt(NetworkConfig.despawnCountContext);

            // If we have no baseline, we need to clear all entities that are not being spawned
            if (!haveBaseline) {
                GameDebug.Assert(despawnCount == 0, "There should not be any despawns in a non-baseline snapshot");
                for (int i = 0, c = entities.Count; i < c; ++i) {
                    var e = entities[i];
                    if (e.type == null)
                        continue;
                    if (m_TempSpawnList.Contains(i))
                        continue;
                    GameDebug.Log("NO BL SO PRUNING Stale entity: " + i);
                    despawns.Add(i);
                    e.Reset();
                }
            }

            for (var despawnIndex = 0; despawnIndex < despawnCount; ++despawnIndex) {
                var id = (int)input.ReadPackedIntDelta(previousId, NetworkConfig.idContext);
                previousId = id;

                // we may see despawns many times, only handle if we still have the entity
                GameDebug.Assert(id < entities.Count, "Getting despawn for id {0} but we only know about entities up to {1}", id, entities.Count);
                if (entities[id].type == null)
                    continue;

                var entity = entities[id];

                // Already in the process of being despawned. This happens with same-snapshot spawn/despawn cases
                if (entity.despawnSequence > 0)
                    continue;

                // If we are spawning and despawning in same snapshot, delay actual deletion of
                // entity as we need it around to be able to read the update part of the snapshot
                if (m_TempSpawnList.Contains(id))
                    entity.despawnSequence = sequence; // keep until baseSequence >= despawnSequence
                else
                    entity.Reset(); // otherwise remove right away; no further updates coming, not even in this snap

                // Add to despawns list so we can request despawn from game later
                GameDebug.Assert(!despawns.Contains(id), "Double despawn in same snaphot? {0}", id);
                despawns.Add(id);
            }

            // Predict all active entities
            for (var id = 0; id < entities.Count; id++) {
                var info = entities[id];
                if (info.type == null)
                    continue;

                // NOTE : As long as the server haven't gotten the spawn acked, it will keep sending
                // delta relative to 0, so we need to check if the entity was in the spawn list to determine
                // if the delta is relative to the last update or not

                int baseline0Time = 0;

                uint[] baseline0 = info.type.baseline;
                GameDebug.Assert(baseline0 != null, "Unable to find schema baseline for type {0}", info.type.typeId);

                if (haveBaseline && !m_TempSpawnList.Contains(id)) {
                    baseline0 = info.baselines.FindMax(baseSequence);
                    GameDebug.Assert(baseline0 != null, "Unable to find baseline for seq {0} for id {1}", baseSequence, id);
                    baseline0Time = snapshots[baseSequence].serverTime;
                }

                if (enableNetworkPrediction) {
                    uint num_baselines = 1; // 1 because either we have schema baseline or we have a real baseline
                    int baseline1Time = 0;
                    int baseline2Time = 0;

                    uint[] baseline1 = null;
                    uint[] baseline2 = null;
                    if (baseSequence1 != baseSequence) {
                        baseline1 = info.baselines.FindMax(baseSequence1);
                        if (baseline1 != null) {
                            num_baselines = 2;
                            baseline1Time = snapshots[baseSequence1].serverTime;
                        }
                        if (baseSequence2 != baseSequence1) {
                            baseline2 = info.baselines.FindMax(baseSequence2);
                            if (baseline2 != null) {
                                num_baselines = 3;
                                baseline2Time = snapshots[baseSequence2].serverTime;
                            }
                        }
                    }

                    // TODO are these clears needed?
                    for (int i = 0, c = info.fieldsChangedPrediction.Length; i < c; ++i)
                        info.fieldsChangedPrediction[i] = 0;
                    for (int i = 0; i < NetworkConfig.maxEntitySnapshotDataSize; i++)
                        info.prediction[i] = 0;

                    fixed (uint* prediction = info.prediction, baseline0p = baseline0, baseline1p = baseline1, baseline2p = baseline2) {
                        NetworkPrediction.PredictSnapshot(prediction, info.fieldsChangedPrediction, info.type.schema, num_baselines, (uint)baseline0Time, baseline0p, (uint)baseline1Time, baseline1p, (uint)baseline2Time, baseline2p, (uint)snapshotInfo.serverTime, info.fieldMask);
                    }
                } else {
                    var f = info.fieldsChangedPrediction;
                    for (var i = 0; i < f.Length; ++i)
                        f[i] = 0;
                    for (int i = 0, c = info.type.schema.GetByteSize() / 4; i < c; ++i)
                        info.prediction[i] = baseline0[i];
                }
            }

            // Read updates
            var updateCount = input.ReadPackedUInt(NetworkConfig.updateCountContext);
            for (var updateIndex = 0; updateIndex < updateCount; ++updateIndex) {
                var id = (int)input.ReadPackedIntDelta(previousId, NetworkConfig.idContext);
                previousId = id;

                var info = entities[id];

                uint hash = 0;
                // Copy prediction to temp buffer as we now overwrite info.prediction with fully unpacked
                // state by applying incoming delta to prediction.
                for (int i = 0, c = info.type.schema.GetByteSize() / 4; i < c; ++i)
                    tempSnapshotBuffer[i] = info.prediction[i];

                DeltaReader.Read(ref input, info.type.schema, info.prediction, tempSnapshotBuffer, info.fieldsChangedPrediction, info.fieldMask, ref hash);
            }

            //if (enableNetworkPrediction)
            //    counters.AddSectionStats("snapShotUpdatesPredict", input.GetBitPosition2(), haveBaseline ? new Color(0.09f, 0.38f, 0.93f) : Color.cyan);
            //else
            //    counters.AddSectionStats("snapShotUpdatesNoPredict", input.GetBitPosition2(), haveBaseline ? new Color(0.09f, 0.38f, 0.93f) : Color.cyan);

            uint snapshotHash = 0; // sum of hash for all (updated or not) entity snapshots
            uint numEnts = 0;

            for (int id = 0; id < entities.Count; id++) {
                var info = entities[id];
                if (info.type == null)
                    continue;

                // Skip despawned that have not also been spawned in this snapshot
                if (info.despawnSequence > 0 && !spawns.Contains(id))
                    continue;

                // If just spawned or if new snapshot is different from the last we deserialized,
                // we need to deserialize. Otherwise just ignore; no reason to deserialize the same
                // values again
                int schemaSize = info.type.schema.GetByteSize();
                if (info.baselines.GetSize() == 0 || NetworkUtils.MemCmp(info.prediction, 0, info.lastUpdate, 0, schemaSize) != 0) {
                    var data = info.baselines.Insert(sequence);
                    for (int i = 0; i < schemaSize / 4; ++i)
                        data[i] = info.prediction[i];
                    if (sequence > info.lastUpdateSequence) {
                        if (!updates.Contains(id))
                            updates.Add(id);

                        for (int i = 0; i < schemaSize / 4; ++i)
                            info.lastUpdate[i] = info.prediction[i];
                        info.lastUpdateSequence = sequence;
                    }
                }

                //if (enableHashing && info.despawnSequence == 0) {
                //    snapshotHash += NetworkUtils.SimpleHash(info.prediction, schemaSize);
                //    numEnts++;
                //}
            }


            if (clientDebug.IntValue > 1) {
                if (clientDebug.IntValue > 2 || spawnCount > 0 || despawnCount > 0 || schemaCount > 0 || !haveBaseline) {
                    string entityIds = "";
                    for (var i = 0; i < entities.Count; i++) {
                        var e = entities[i];
                        entityIds += e.type == null ? ",-" : (e.despawnSequence > 0 ? "," + i + "(" + e.despawnSequence + ")" : "," + i);
                    }
                    string despawnIds = string.Join(",", despawns);
                    string spawnIds = string.Join(",", m_TempSpawnList);
                    string updateIds = string.Join(",", updates);

                    if (enableNetworkPrediction)
                        GameDebug.Log(("SEQ:" + snapshotInfo.serverTime + ":" + sequence) + (haveBaseline ? "Snap [BL]" : "Snap [  ]") + "  " + baseSequence + " - " + baseSequence1 + " - " + baseSequence2 + ". Sche: " + schemaCount + " Spwns: " + spawnCount + "(" + spawnIds + ") Desp: " + despawnCount + "(" + despawnIds + ") Upd: " + updateCount + "(" + updateIds + ")  Ents:" + entities.Count + " EntityIds:" + entityIds);
                    else
                        GameDebug.Log(("SEQ:" + snapshotInfo.serverTime + ":" + sequence) + (haveBaseline ? "Snap [BL]" : "Snap [  ]") + "  " + baseSequence + ". Sche: " + schemaCount + " Spwns: " + spawnCount + "(" + spawnIds + ") Desp: " + despawnCount + "(" + despawnIds + ") Upd: " + updateCount + "(" + updateIds + ")  Ents:" + entities.Count + " EntityIds:" + entityIds);
                }
            }

            //if (enableHashing) {
            //    uint numEntsCheck = input.ReadRawBits(32);
            //    if (numEntsCheck != numEnts) {
            //        GameDebug.Log("SYNC PROBLEM: server num ents: " + numEntsCheck + " us:" + numEnts);
            //        GameDebug.Assert(false);
            //    }
            //}

            //counters.AddSectionStats("snapShotChecksum", input.GetBitPosition2(), new Color(0.2f, 0.2f, 0.2f));

            // Snapshot reading done. Now pass on resulting pawns/despawns to the snapshotconsumer
            Profiler.BeginSample("ProcessSnapshot");

            consumer.ProcessEntityDespawns(serverTime, despawns);
            despawns.Clear();

            foreach (var id in spawns) {
                GameDebug.Assert(entities[id].type != null, "Processing spawn of id {0} but type is null", id);
                consumer.ProcessEntitySpawn(serverTime, id, entities[id].type.typeId);
            }

            spawns.Clear();

            foreach (var id in updates) {
                var info = entities[id];
                GameDebug.Assert(info.type != null, "Processing update of id {0} but type is null", id);
                fixed (uint* data = info.lastUpdate) {
                    var reader = new NetworkReader(data, info.type.schema);
                    consumer.ProcessEntityUpdate(serverTime, id, ref reader);
                }
            }

            updates.Clear();
            Profiler.EndSample();
        }

        public void SendPackage() {
            if (connectionState != ConnectionState.Connected)//do not send anything until we receive client info
                return;

            var rawOutputStream = new BitOutputStream(m_PackageBuffer);

            // todo : only if there is anything to send

            ClientPackageInfo info;
            BeginSendPackage(ref rawOutputStream, out info);

            int endOfHeaderPos = rawOutputStream.Align();
            var output = default(RawOutputStream);
            output.Initialize(m_PackageBuffer, endOfHeaderPos);

            if (sendClientConfig)
                WriteClientConfig(ref output);

            if (commandSequence > 0) {
                lastSentCommandSeq = commandSequence;
                WriteCommands(info, ref output);
            }

            WriteEvents(info, ref output);
            int compressedSize = output.Flush();
            rawOutputStream.SkipBytes(compressedSize);

            CompleteSendPackage(info, ref rawOutputStream);
        }

        unsafe public void QueueCommand(int time, DataGenerator generator) {
            var generateSchema = (commandSchema == null);
            if (generateSchema)
                commandSchema = new NetworkSchema(NetworkConfig.networkClientQueueCommandSchemaId);

            var info = commandsOut.Acquire(++commandSequence);

            info.time = time;
            fixed (uint* buf = info.data) {
                var writer = new NetworkWriter(buf, info.data.Length, commandSchema, generateSchema);
                generator(ref writer);
                writer.Flush();
            }
        }

        void WriteClientConfig(ref RawOutputStream output) {
            AddMessageContentFlag(NetworkMessage.ClientConfig);

            output.WriteRawBits((uint)_clientConfig.serverUpdateRate, 32);
            output.WriteRawBits((uint)_clientConfig.serverUpdateInterval, 16);
            sendClientConfig = false;

            if (clientDebug.IntValue > 0) {
                GameDebug.Log(string.Format("WriteClientConfig: serverUpdateRate {0}    serverUpdateInterval {1}", _clientConfig.serverUpdateRate, _clientConfig.serverUpdateInterval));
            }
        }

        unsafe void WriteCommands(ClientPackageInfo packageInfo, ref RawOutputStream output){
            AddMessageContentFlag(NetworkMessage.Commands);
            counters.commandsOut++;

            var includeSchema = commandSequenceAck == 0;
            output.WriteRawBits(includeSchema ? 1U : 0, 1);
            if (includeSchema)
                NetworkSchema.WriteSchema(commandSchema, ref output);

            var sequence = commandSequence;
            output.WriteRawBits(Sequence.ToUInt16(commandSequence), 16);

            packageInfo.commandSequence = commandSequence;
            packageInfo.commandTime = commandsOut[commandSequence].time;

            CommandInfo previous = defaultCommandInfo;
            CommandInfo command;
            while (commandsOut.TryGetValue(sequence, out command)) {
                // 1 bit to tell there is a command 
                output.WriteRawBits(1, 1);
                output.WritePackedIntDelta(command.time, previous.time, NetworkConfig.commandTimeContext);
                uint hash = 0;
                fixed (uint* data = command.data, baseline = previous.data) {
                    DeltaWriter.Write(ref output, commandSchema, data, baseline, zeroFieldsChanged, 0, ref hash);
                }

                previous = command;
                --sequence;
            }
            output.WriteRawBits(0, 1);
        }
        byte[] zeroFieldsChanged = new byte[(NetworkConfig.maxFieldsPerSchema + 7) / 8];

        protected override void NotifyDelivered(int sequence, ClientPackageInfo info, bool madeIt) {
            base.NotifyDelivered(sequence, info, madeIt);
            if (madeIt) {
                if (info.commandSequence > commandSequenceAck) {
                    commandSequenceAck = info.commandSequence;
                    lastAcknowlegdedCommandTime = info.commandTime;
                }
            } else {
                //Resend user config if the package was lost
                if ((info.Content & NetworkMessage.ClientConfig) != 0)
                    sendClientConfig = true;
            }
        }

        class MapInfo
        {
            public bool processed;                  // Map reset was processed by game
            public ushort mapSequence;              // map identifier to discard duplicate messages
            public int ackSequence;                 // package sequence the map was acked in (discard packages before this)
            public NetworkSchema schema;            // Schema for the map info
            public uint[] data = new uint[256];     // Game specific map info payload
        }

        class SnapshotInfo
        {
            public int serverTime;
        }

        bool sendClientConfig = true;

        SequenceBuffer<SnapshotInfo> snapshots = new SequenceBuffer<SnapshotInfo>(NetworkConfig.snapshotDeltaCacheSize, () => new SnapshotInfo());
        Dictionary<ushort, EntityTypeInfo> entityTypes = new Dictionary<ushort, EntityTypeInfo>();
        List<EntityInfo> entities = new List<EntityInfo>();

        List<int> despawns = new List<int>();
        List<int> spawns = new List<int>();
        List<int> updates = new List<int>();

        List<int> m_TempSpawnList = new List<int>();

        public int lastAcknowlegdedCommandTime;
        int commandSequence;
        int lastSentCommandSeq = 0;
        int commandSequenceAck;
        NetworkSchema commandSchema;
        SequenceBuffer<CommandInfo> commandsOut = new SequenceBuffer<CommandInfo>(3, () => new CommandInfo());
        CommandInfo defaultCommandInfo = new CommandInfo();

        public int serverTickRate;
        public int serverTime;
        private MapInfo mapInfo = new MapInfo();
    }
}
