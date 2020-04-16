using NetworkCompression;
using Photon.Pun;
using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Profiling;

public interface ISnapshotGenerator
{
    int WorldTick { get; }
    void GenerateEntitySnapshot(int entityId, ref NetworkWriter writer);
    string GenerateEntityName(int entityId);
}

public interface IClientCommandProcessor
{
    void ProcessCommand(int connectionId, int tick, ref NetworkReader data);
}

unsafe public class NetworkServer
{
    [ConfigVar(Name = "server.debug", DefaultValue = "0", Description = "Enable debug printing of server handshake etc.", Flags = ConfigVar.Flags.None)]
    public static ConfigVar serverDebug;

    [ConfigVar(Name = "server.debugentityids", DefaultValue = "1", Description = "Enable debug printing entity id recycling.", Flags = ConfigVar.Flags.None)]
    public static ConfigVar serverDebugEntityIds;

    public delegate void DataGenerator(ref NetworkWriter writer);

    [ConfigVar(Name = "server.network_prediction", DefaultValue = "0", Description = "Predict snapshots data to improve compression and minimize bandwidth")]
    public static ConfigVar network_prediction;

    // Each client needs to receive this on connect and when any of the values changes
    public class ServerInfo
    {
        public int ServerTickRate;
    }

    private enum ConnectionState
    {
        Disconnected,
        Connecting,
        Connected,
    }

    public class ServerPackageInfo : PackageInfo
    {
        public int serverSequence;
        public int serverTime;
    }

    public class Counters : NetworkConnectionCounters
    {
        public int snapshotsOut;
        public int commandsIn;
    }

    private unsafe class MapInfo
    {
        public int serverInitSequence;                  // The server frame the map was initialized
        public ushort mapId;                            // Unique sequence number for the map (to deal with redudant mapinfo messages)
        public NetworkSchema schema;                    // Schema for the map info
        public uint* data = (uint*)UnsafeUtility.Malloc(1024, UnsafeUtility.AlignOf<uint>(), Unity.Collections.Allocator.Persistent);            // Game specific payload
    }

    unsafe public class EntityTypeInfo
    {
        public string name;
        public ushort typeId;
        public NetworkSchema schema;
        public int createdSequence;
        public uint* baseline;
        //public int stats_count;
        //public int stats_bits;
    }

    unsafe class EntitySnapshotInfo
    {
        public uint* start;     // pointer into WorldSnapshot.data block (see below)
        public int length;      // length of data in words
    }

    // Each tick a WorldSnapshot is generated. The data buffer contains serialized data
    // from all serializable entitites
    unsafe class WorldSnapshot
    {
        public int serverTime;  // server tick for this snapshot
        public int length;      // length of data in data field
        public uint* data;
    }

    unsafe class EntityInfo
    {
        public EntityInfo() {
            snapshots = new SequenceBuffer<EntitySnapshotInfo>(NetworkConfig.snapshotDeltaCacheSize, () => new EntitySnapshotInfo());
        }

        public void Reset() {
            typeId = 0;
            spawnSequence = 0;
            despawnSequence = 0;
            updateSequence = 0;
            snapshots.Clear();
            for (var i = 0; i < fieldsChangedPrediction.Length; i++)
                fieldsChangedPrediction[i] = 0;
            predictingClientId = -1;
        }

        public ushort typeId;
        public int predictingClientId = -1;

        public int spawnSequence;
        public int despawnSequence;
        public int updateSequence;

        public SequenceBuffer<EntitySnapshotInfo> snapshots;
        public uint* prediction;                                // NOTE: used in WriteSnapshot but invalid outside that function
        public byte[] fieldsChangedPrediction = new byte[(NetworkConfig.maxFieldsPerSchema + 7) / 8];

        // On server the fieldmask of an entity is different depending on what client we are sending to
        // Flags:
        //    1 : receiving client is predicting
        //    2 : receiving client is not predicting
        public byte GetFieldMask(int connectionId) {
            byte mask = 0;
            if (predictingClientId == -1)
                return 0;
            if (predictingClientId == connectionId)
                mask |= 0x1;
            else
                mask |= 0x2;
            return mask;
        }
    }

    private ConnectionState _connectionState = ConnectionState.Disconnected;

    private INetworkTransport _transport;
    private ServerConnection _serverConnection;

    //the game time on the server
    public int serverTime { get; private set; }
    public ServerInfo serverInfo;

    public bool IsConnected { get { return _connectionState == ConnectionState.Connected; } }

    unsafe public NetworkServer(INetworkTransport transport) {
        _transport = transport;

        serverInfo = new ServerInfo();
        // Allocate array to hold world snapshots
        m_Snapshots = new WorldSnapshot[NetworkConfig.snapshotDeltaCacheSize];
        for (int i = 0; i < m_Snapshots.Length; ++i) {
            m_Snapshots[i] = new WorldSnapshot();
            m_Snapshots[i].data = (uint*)UnsafeUtility.Malloc(NetworkConfig.maxWorldSnapshotDataSize, UnsafeUtility.AlignOf<UInt32>(), Unity.Collections.Allocator.Persistent);
        }

        // Allocate scratch buffer to hold predictions. This is overwritten every time
        // a snapshot is being written to a specific client
        m_Prediction = (uint*)UnsafeUtility.Malloc(NetworkConfig.maxWorldSnapshotDataSize, UnsafeUtility.AlignOf<UInt32>(), Unity.Collections.Allocator.Persistent);
    }

    public void Disconnect() {
        _transport.Disconnect();
    }

    public void Connect() {
        GameDebug.Assert(_connectionState == ConnectionState.Disconnected);
        GameDebug.Assert(_serverConnection == null);

        _transport.Connect();

        _connectionState = ConnectionState.Connecting;
    }

    unsafe public void InitializeMap(DataGenerator generator) {
        // Generate schema the first time we set map info
        bool generateSchema = false;
        if (m_MapInfo.schema == null) {
            m_MapInfo.schema = new NetworkSchema(NetworkConfig.mapSchemaId);
            generateSchema = true;
        }

        // Update map info
        var writer = new NetworkWriter(m_MapInfo.data, 1024, m_MapInfo.schema, generateSchema);
        generator(ref writer);
        writer.Flush();

        m_MapInfo.serverInitSequence = m_ServerSequence;
        ++m_MapInfo.mapId;

        // Reset map and connection state
        serverTime = 0;
        //m_Entities.Clear();
        //m_FreeEntities.Clear();
        foreach (var pair in _serverConnections)
            pair.Value.Reset();
    }

    public void MapReady(int clientId) {
        GameDebug.Log("Client " + clientId + " is ready");
        if (!_serverConnections.ContainsKey(clientId)) {
            GameDebug.Log("Got MapReady from unknown client?");
            return;
        }
        _serverConnections[clientId].mapReady = true;
    }

    // Reserve scene entities with sequential id's starting from 0
    public void ReserveSceneEntities(int count) {
        GameDebug.Assert(m_Entities.Count == 0, "ReserveSceneEntities: Only allowed before other entities have been registrered");
        for (var i = 0; i < count; i++) {
            m_Entities.Add(new EntityInfo());
        }
    }

    // Currently predictingClient can only be set on an entity at time of creation
    // in the future it should be something you can change if you for example enter/leave
    // a vehicle. There are subtle but tricky replication issues when predicting 'ownership' changes, though...
    public int RegisterEntity(int id, ushort typeId, int predictingClientId) {
        Profiler.BeginSample("NetworkServer.RegisterEntity()");
        EntityInfo entityInfo;
        int freeCount = m_FreeEntities.Count;

        if (id >= 0) {
            GameDebug.Assert(m_Entities[id].spawnSequence == 0, "RegisterEntity: Trying to reuse an id that is used by a scene entity");
            entityInfo = m_Entities[id];
        } else if (freeCount > 0) {
            id = m_FreeEntities[freeCount - 1];
            m_FreeEntities.RemoveAt(freeCount - 1);
            entityInfo = m_Entities[id];
            entityInfo.Reset();
        } else {
            entityInfo = new EntityInfo();
            m_Entities.Add(entityInfo);
            id = m_Entities.Count - 1;
        }

        entityInfo.typeId = typeId;
        entityInfo.predictingClientId = predictingClientId;
        entityInfo.spawnSequence = m_ServerSequence + 1; // NOTE : Associate the spawn with the next snapshot

        if (serverDebugEntityIds.IntValue > 1)
            GameDebug.Log("Registred entity id: " + id);

        Profiler.EndSample();
        return id;
    }

    public void UnregisterEntity(int id) {
        Profiler.BeginSample("NetworkServer.UnregisterEntity()");
        m_Entities[id].despawnSequence = m_ServerSequence + 1;
        Profiler.EndSample();
    }

    public void HandleClientCommands(int tick, IClientCommandProcessor processor) {
        foreach (var c in _serverConnections)
            c.Value.ProcessCommands(tick, processor);
    }

    unsafe public void GenerateSnapshot(ISnapshotGenerator snapshotGenerator, float simTime) {
        var time = snapshotGenerator.WorldTick;
        GameDebug.Assert(time > serverTime);      // Time should always flow forward
        GameDebug.Assert(m_MapInfo.mapId > 0);    // Initialize map before generating snapshot

        ++m_ServerSequence;

        // We currently keep entities around until every client has ack'ed the snapshot with the despawn
        // Then we delete them from our list and recycle the id
        // TODO: we do not need this anymore?

        // Find oldest (smallest seq no) acked snapshot.
        var minClientAck = int.MaxValue;
        foreach (var pair in _serverConnections) {
            var c = pair.Value;
            // If a client is so far behind that we have to send non-baseline updates to it
            // there is no reason to keep despawned entities around for this clients sake
            if (m_ServerSequence - c.maxSnapshotAck >= NetworkConfig.snapshotDeltaCacheSize - 2) // -2 because we want 3 baselines!
                continue;
            var acked = c.maxSnapshotAck;
            if (acked < minClientAck)
                minClientAck = acked;
        }

        // Recycle despawned entities that have been acked by all
        for (int i = 0; i < m_Entities.Count; i++) {
            var e = m_Entities[i];
            if (e.despawnSequence > 0 && e.despawnSequence < minClientAck) {
                //if (serverDebugEntityIds.IntValue > 1)
                //    GameDebug.Log("Recycling entity id: " + i + " because despawned in " + e.despawnSequence + " and minAck is now " + minClientAck);
                e.Reset();
                m_FreeEntities.Add(i);
            }
        }

        serverTime = time;
        m_ServerSimTime = simTime;

        m_LastEntityCount = 0;

        // Grab world snapshot from circular buffer
        var worldsnapshot = m_Snapshots[m_ServerSequence % m_Snapshots.Length];
        worldsnapshot.serverTime = time;
        worldsnapshot.length = 0;

        // Run through all the registered network entities and serialize the snapshot
        for (var id = 0; id < m_Entities.Count; id++) {
            var entity = m_Entities[id];

            // Skip freed
            if (entity.spawnSequence == 0)
                continue;

            // Skip entities that are depawned
            if (entity.despawnSequence > 0)
                continue;
            
            // If we are here and are despawned, we must be a despawn/spawn in same frame situation
            GameDebug.Assert(entity.despawnSequence == 0 || entity.despawnSequence == entity.spawnSequence, "Snapshotting entity that was deleted in the past?");
            GameDebug.Assert(entity.despawnSequence == 0 || entity.despawnSequence == m_ServerSequence, "WUT");

            // For now we generate the entity type info the first time we generate a snapshot
            // for the particular entity as a more lightweight approach rather than introducing
            // a full schema system where the game code must generate and register the type
            EntityTypeInfo typeInfo;
            bool generateSchema = false;
            if (!m_EntityTypes.TryGetValue(entity.typeId, out typeInfo)) {
                typeInfo = new EntityTypeInfo() { name = snapshotGenerator.GenerateEntityName(id), typeId = entity.typeId, createdSequence = m_ServerSequence, schema = new NetworkSchema(entity.typeId + NetworkConfig.firstEntitySchemaId) };
                m_EntityTypes.Add(entity.typeId, typeInfo);
                generateSchema = true;
            }

            // Generate entity snapshot
            var snapshotInfo = entity.snapshots.Acquire(m_ServerSequence);
            snapshotInfo.start = worldsnapshot.data + worldsnapshot.length;

            var writer = new NetworkWriter(snapshotInfo.start, NetworkConfig.maxWorldSnapshotDataSize / 4 - worldsnapshot.length, typeInfo.schema, generateSchema);
            snapshotGenerator.GenerateEntitySnapshot(id, ref writer);
            writer.Flush();
            snapshotInfo.length = writer.GetLength();

            worldsnapshot.length += snapshotInfo.length;

            if (entity.despawnSequence == 0) {
                m_LastEntityCount++;
            }

            GameDebug.Assert(snapshotInfo.length > 0, "Tried to generate a entity snapshot but no data was delivered by generator?");

            if (generateSchema) {
                GameDebug.Assert(typeInfo.baseline == null, "Generating schema twice?");
                // First time a type/schema is encountered, we clone the serialized data and
                // use it as the type-baseline
                typeInfo.baseline = (uint*)UnsafeUtility.Malloc(snapshotInfo.length * 4, UnsafeUtility.AlignOf<UInt32>(), Unity.Collections.Allocator.Persistent);// new uint[snapshot.length];// (uint[])snapshot.data.Clone();
                for (int i = 0; i < snapshotInfo.length; i++)
                    typeInfo.baseline[i] = *(snapshotInfo.start + i);
            }

            // Check if it is different from the previous generated snapshot
            var dirty = !entity.snapshots.Exists(m_ServerSequence - 1);
            if (!dirty) {
                var previousSnapshot = entity.snapshots[m_ServerSequence - 1];
                if (previousSnapshot.length != snapshotInfo.length || // TODO how could length differ???
                    UnsafeUtility.MemCmp(previousSnapshot.start, snapshotInfo.start, snapshotInfo.length) != 0) {
                    dirty = true;
                }
            }

            if (dirty)
                entity.updateSequence = m_ServerSequence;

            //statsGeneratedEntitySnapshots++;
            //statsSnapshotData += snapshotInfo.length;
        }
        //statsGeneratedSnapshotSize += worldsnapshot.length * 4;
    }

    public void Update(INetworkCallbacks loop) {
        _transport.Update();

        TransportEvent e = new TransportEvent();
        while (_transport.NextEvent(ref e)) {
            switch (e.type) {
                case TransportEvent.Type.Connect:
                OnConnect(e.ConnectionId, loop);
                break;
                case TransportEvent.Type.Disconnect:
                OnDisconnect(e.ConnectionId, loop);
                break;
                case TransportEvent.Type.Data:
                OnData(e.ConnectionId, e.Data, loop);
                break;
            }
        }
    }

    public void UpdateClientInfo() {
        serverInfo.ServerTickRate = Game.serverTickRate.IntValue;

        foreach (var pair in _serverConnections)
            pair.Value.clientInfoAcked = false;
    }

    public void SendData() {
        foreach(var connection in _serverConnections.Values) {
            connection.SendPackage();
        }
    }

    public void OnData(int connectionId, byte[] data, INetworkCallbacks loop) {
        if (!_serverConnections.ContainsKey(connectionId))
            return;

        _serverConnections[connectionId].ReadPackage(data, loop);
    }

    public void OnConnect(int connectionId, INetworkCallbacks loop) {
        if(connectionId == PhotonNetwork.LocalPlayer.ActorNumber) {
            _connectionState = ConnectionState.Connected;
            return;
        }
        GameDebug.Log($"Player {connectionId} is connected.");

        if (!_serverConnections.ContainsKey(connectionId)) {
            _serverConnections.Add(connectionId, new ServerConnection(this, connectionId, _transport));
        }

        loop.OnConnect(connectionId);
    }

    public void OnDisconnect(int connectionId, INetworkCallbacks loop) {
        if (connectionId == PhotonNetwork.LocalPlayer.ActorNumber) {
            _connectionState = ConnectionState.Disconnected;
            return;
        }

        GameDebug.Log($"Player {connectionId} is disconnected");
        loop.OnDisconnect(connectionId);

        if (_serverConnections.ContainsKey(connectionId))
            _serverConnections.Remove(connectionId);
    }

    public void Shutdown() {
        _transport.Shutdown();

        Disconnect();
    }

    public class ServerConnection : NetworkConnection<ServerPackageInfo, NetworkServer.Counters>
    {
        public void SetSnapshotInterval(int _snapshotInterval) {
            if (_snapshotInterval < 1)
                _snapshotInterval = 1;
            snapshotInterval = _snapshotInterval;
        }

        public ServerConnection(NetworkServer server, int connectionId, INetworkTransport transport) : base(connectionId, transport) {
            _server = server;
            snapshotInterval = 1;
            maxBPS = 0;
            nextOutPackageTime = 0;
        }

        unsafe public void ProcessCommands(int maxTime, IClientCommandProcessor processor) {
            // Check for time jumps backward in the command stream and reset the queue in case
            // we find one. (This will happen if the client determines that it has gotten too
            // far ahead and recalculate the client time.)
            
            // TODO : We should be able to do this in a smarter way
            for (var sequence = commandSequenceProcessed + 1; sequence <= commandSequenceIn; ++sequence) {
                CommandInfo previous;
                CommandInfo current;

                commandsIn.TryGetValue(sequence, out current);
                commandsIn.TryGetValue(sequence - 1, out previous);

                if (current != null && previous != null && current.time <= previous.time)
                    commandSequenceProcessed = sequence - 1;
            }

            for (var sequence = commandSequenceProcessed + 1; sequence <= commandSequenceIn; ++sequence) {
                CommandInfo info;
                if (commandsIn.TryGetValue(sequence, out info)) {
                    if (info.time <= maxTime) {
                        fixed (uint* data = info.data) {
                            var reader = new NetworkReader(data, commandSchema);
                            processor.ProcessCommand(ConnectionId, info.time, ref reader);
                        }
                        commandSequenceProcessed = sequence;
                    } else
                        return;
                }
            }
        }

        byte[] packageBuffer = new byte[NetworkConfig.MaxPackageSize];
        public void ReadPackage(byte[] packageData, INetworkCallbacks loop) {
            counters.bytesIn += packageData.Length;

            NetworkUtils.MemCopy(packageData, 0, packageBuffer, 0, packageData.Length);

            NetworkMessage content;

            int headerSize;
            var packageSequence = ProcessPackageHeader(packageBuffer, out content, out headerSize);

            var input = new RawInputStream(packageBuffer, headerSize);

            if ((content & NetworkMessage.ClientConfig) != 0)
                ReadClientConfig(ref input);

            if ((content & NetworkMessage.Commands) != 0)
                ReadCommands(ref input);

            if ((content & NetworkMessage.Events) != 0)
                ReadEvents(ref input, loop);
        }

        void ReadClientConfig(ref RawInputStream input){
            maxBPS = (int)input.ReadRawBits(32);
            var snapshotInterval = (int)input.ReadRawBits(16);
            SetSnapshotInterval(snapshotInterval);

            if (serverDebug.IntValue > 0) {
                GameDebug.Log(string.Format("ReadClientConfig: updateRate: {0}  snapshotRate: {1}", maxBPS, snapshotInterval));
            }
        }

        void ReadCommands(ref RawInputStream input){
            counters.commandsIn++;
            var schema = input.ReadRawBits(1) != 0;
            if (schema) {
                commandSchema = NetworkSchema.ReadSchema(ref input);    // might be overridden
            }

            // NETTODO Reconstruct the wide sequence
            // NETTODO Rename to commandMessageSequence?
            var sequence = Sequence.FromUInt16((ushort)input.ReadRawBits(16), commandSequenceIn);
            if (sequence > commandSequenceIn)
                commandSequenceIn = sequence;
            
            CommandInfo previous = defaultCommandInfo;
            while (input.ReadRawBits(1) != 0) {
                var command = commandsIn.Acquire(sequence);
                command.time = (int)input.ReadPackedIntDelta(previous.time, NetworkConfig.commandTimeContext);

                uint hash = 0;
                DeltaReader.Read(ref input, commandSchema, command.data, previous.data, zeroFieldsChanged, 0, ref hash);

                previous = command;
                --sequence;
            }
        }
        byte[] zeroFieldsChanged = new byte[(NetworkConfig.maxFieldsPerSchema + 7) / 8];

        public void SendPackage() {
            var rawOutputStream = new BitOutputStream(m_PackageBuffer);

            // Distribute clients evenly according to their with snapshotInterval > 1
            // TODO: This kind of assumes same update interval by all ....
            if ((_server.m_ServerSequence + ConnectionId) % snapshotInterval != 0)
                return;

            // Respect max bps rate cap
            if (Game.frameTime < nextOutPackageTime)
                return;

            ServerPackageInfo packageInfo;
            BeginSendPackage(ref rawOutputStream, out packageInfo);

            int endOfHeaderPos = rawOutputStream.Align();
            var output = new RawOutputStream();// new TOutputStream();  Due to bug new generates garbage here
            output.Initialize(m_PackageBuffer, endOfHeaderPos);

            packageInfo.serverSequence = _server.m_ServerSequence;
            packageInfo.serverTime = _server.serverTime;             // Server time (could be ticks or could be ms)

            // The ifs below are in essence the 'connection handshake' logic.
            if (!clientInfoAcked) {
                // Keep sending client info until it is acked
                WriteClientInfo(ref output);
            }else if (!mapAcked) {
                if (_server.m_MapInfo.serverInitSequence > 0) {
                    // Keep sending map info until it is acked
                    WriteMapInfo(ref output);
                }
            } else {
                // Send snapshot, buf only
                //   if client has declared itself ready
                //   if we have not already sent for this tick (because we need to be able to map a snapshot 
                //     sequence to a package sequence we cannot send the same snapshot multiple times).
                if (mapReady && _server.m_ServerSequence > snapshotServerLastWritten) {
                    WriteSnapshot(ref output);
                }
            }

            int compressedSize = output.Flush();
            rawOutputStream.SkipBytes(compressedSize);

            var messageSize = CompleteSendPackage(packageInfo, ref rawOutputStream);

            // Decide when next package can go out
            if (maxBPS > 0) {
                double timeLimitBPS = messageSize / maxBPS;
                if (timeLimitBPS > (float)snapshotInterval / Game.serverTickRate.FloatValue) {
                    GameDebug.Log("SERVER: Choked by BPS sending " + messageSize);
                    nextOutPackageTime = Game.frameTime + timeLimitBPS;
                }
            }

            CompleteSendPackage(packageInfo, ref rawOutputStream);
        }

        public void WriteClientInfo(ref RawOutputStream output) {
            AddMessageContentFlag(NetworkMessage.ClientInfo);

            output.WriteRawBits((uint)ConnectionId, 8);
            output.WriteRawBits((uint)_server.serverInfo.ServerTickRate, 8);
        }

        unsafe private void WriteMapInfo(ref RawOutputStream output){
            AddMessageContentFlag(NetworkMessage.MapInfo);

            output.WriteRawBits(_server.m_MapInfo.mapId, 16);

            // Write schema if client haven't acked it
            output.WriteRawBits(mapSchemaAcked ? 0 : 1U, 1);
            if (!mapSchemaAcked)
                NetworkSchema.WriteSchema(_server.m_MapInfo.schema, ref output);

            // Write map data
            NetworkSchema.CopyFieldsFromBuffer(_server.m_MapInfo.schema, _server.m_MapInfo.data, ref output);
        }

        unsafe void WriteSnapshot(ref RawOutputStream output){
            Profiler.BeginSample("NetworkServer.WriteSnapshot()");
            AddMessageContentFlag(NetworkMessage.Snapshot);

            bool enableNetworkPrediction = network_prediction.IntValue != 0;
            //bool enableHashing = debug_hashing.IntValue != 0;

            // Check if the baseline from the client is too old. We keep N number of snapshots on the server 
            // so if the client baseline is older than that we cannot generate the snapshot. Furthermore, we require
            // the client to keep the last N updates for any entity, so even though the client might have much older
            // baselines for some entities we cannot guarantee it. 
            // TODO : Can we make this simpler?
            var haveBaseline = maxSnapshotAck != 0;
            if (_server.m_ServerSequence - maxSnapshotAck >= NetworkConfig.snapshotDeltaCacheSize - 2) // -2 because we want 3 baselines!
            {
                if (serverDebug.IntValue > 0)
                    GameDebug.Log("ServerSequence ahead of latest ack'ed snapshot by more than cache size. " + (haveBaseline ? "nobaseline" : "baseline"));
                haveBaseline = false;
            }
            var baseline = haveBaseline ? maxSnapshotAck : 0;

            int snapshot0Baseline = baseline;
            int snapshot1Baseline = baseline;
            int snapshot2Baseline = baseline;
            int snapshot0BaselineClient = snapshotPackageBaseline;
            int snapshot1BaselineClient = snapshotPackageBaseline;
            int snapshot2BaselineClient = snapshotPackageBaseline;
            if (enableNetworkPrediction && haveBaseline) {
                var end = snapshotPackageBaseline - NetworkConfig.clientAckCacheSize;
                end = end < 0 ? 0 : end;
                var a = snapshotPackageBaseline - 1;
                while (a > end) {
                    if (snapshotAcks[a % NetworkConfig.clientAckCacheSize]) {
                        var base1 = snapshotSeqs[a % NetworkConfig.clientAckCacheSize];
                        if (_server.m_ServerSequence - base1 < NetworkConfig.snapshotDeltaCacheSize - 2) {
                            snapshot1Baseline = base1;
                            snapshot1BaselineClient = a;
                            snapshot2Baseline = snapshotSeqs[a % NetworkConfig.clientAckCacheSize];
                            snapshot2BaselineClient = a;
                        }
                        break;
                    }
                    a--;
                }
                a--;
                while (a > end) {
                    if (snapshotAcks[a % NetworkConfig.clientAckCacheSize]) {
                        var base2 = snapshotSeqs[a % NetworkConfig.clientAckCacheSize];
                        if (_server.m_ServerSequence - base2 < NetworkConfig.snapshotDeltaCacheSize - 2) {
                            snapshot2Baseline = base2;
                            snapshot2BaselineClient = a;
                        }
                        break;
                    }
                    a--;
                }
            }
            output.WriteRawBits(haveBaseline ? 1u : 0, 1);
            output.WritePackedIntDelta(snapshot0BaselineClient, outSequence - 1, NetworkConfig.baseSequenceContext);
            output.WriteRawBits(enableNetworkPrediction ? 1u : 0u, 1);
            //output.WriteRawBits(enableHashing ? 1u : 0u, 1);
            if (enableNetworkPrediction) {
                output.WritePackedIntDelta(haveBaseline ? snapshot1BaselineClient : 0, snapshot0BaselineClient - 1, NetworkConfig.baseSequence1Context);
                output.WritePackedIntDelta(haveBaseline ? snapshot2BaselineClient : 0, snapshot1BaselineClient - 1, NetworkConfig.baseSequence2Context);
            }

            // NETTODO: For us serverTime == tick but network layer only cares about a growing int
            output.WritePackedIntDelta(_server.serverTime, haveBaseline ? maxSnapshotTime : 0, NetworkConfig.serverTimeContext);
            // NETTODO: a more generic way to send stats
            var temp = _server.m_ServerSimTime * 10;
            output.WriteRawBits((byte)temp, 8);

            _server.m_TempTypeList.Clear();
            _server.m_TempSpawnList.Clear();
            _server.m_TempDespawnList.Clear();
            _server.m_TempUpdateList.Clear();

            _server.m_PredictionIndex = 0;
            for (int id = 0, c = _server.m_Entities.Count; id < c; id++) {
                var entity = _server.m_Entities[id];

                // Skip freed
                if (entity.spawnSequence == 0)
                    continue;

                bool spawnedSinceBaseline = (entity.spawnSequence > baseline);
                bool despawned = (entity.despawnSequence > 0);

                // Note to future self: This is a bit tricky... We consider lifetimes of entities
                // re the baseline (last ack'ed, so in the past) and the snapshot we are building (now)
                // There are 6 cases (S == spawn, D = despawn):
                //
                //  --------------------------------- time ----------------------------------->
                //
                //                   BASELINE          SNAPSHOT
                //                      |                 |
                //                      v                 v
                //  1.    S-------D                                                  IGNORE
                //  2.    S------------------D                                       SEND DESPAWN
                //  3.    S-------------------------------------D                    SEND UPDATE
                //  4.                        S-----D                                IGNORE
                //  5.                        S-----------------D                    SEND SPAWN + UPDATE
                //  6.                                         S----------D          INVALID (FUTURE)
                //

                if (despawned && entity.despawnSequence <= baseline)
                    continue;                               // case 1: ignore

                if (despawned && !spawnedSinceBaseline) {
                    _server.m_TempDespawnList.Add(id);       // case 2: despawn
                    continue;
                }

                if (spawnedSinceBaseline && despawned)
                    continue;                               // case 4: ignore

                if (spawnedSinceBaseline)
                    _server.m_TempSpawnList.Add(id);         // case 5: send spawn + update

                // case 5. and 3. fall through to here and gets updated

                // Send data from latest tick
                var tickToSend = _server.m_ServerSequence;
                // If despawned, however, we have stopped generating updates so pick latest valid
                if (despawned)
                    tickToSend = Mathf.Max(entity.updateSequence, entity.despawnSequence - 1);

                {
                    var entityType = _server.m_EntityTypes[entity.typeId];

                    var snapshot = entity.snapshots[tickToSend];

                    // NOTE : As long as the server haven't gotten the spawn acked, it will keep sending
                    // delta relative to 0 as we cannot know if we have a valid baseline on the client or not

                    uint num_baselines = 1; // if there is no normal baseline, we use schema baseline so there is always one
                    uint* baseline0 = entityType.baseline;
                    int time0 = maxSnapshotTime;

                    if (haveBaseline && entity.spawnSequence <= maxSnapshotAck) {
                        baseline0 = entity.snapshots[snapshot0Baseline].start;
                    }

                    if (enableNetworkPrediction) {
                        uint* baseline1 = entityType.baseline;
                        uint* baseline2 = entityType.baseline;
                        int time1 = maxSnapshotTime;
                        int time2 = maxSnapshotTime;

                        if (haveBaseline && entity.spawnSequence <= maxSnapshotAck) {
                            GameDebug.Assert(_server.m_Snapshots[snapshot0Baseline % _server.m_Snapshots.Length].serverTime == maxSnapshotTime, "serverTime == maxSnapshotTime");
                            GameDebug.Assert(entity.snapshots.Exists(snapshot0Baseline), "Exists(snapshot0Baseline)");

                            // Newly spawned entities might not have earlier baselines initially
                            if (snapshot1Baseline != snapshot0Baseline && entity.snapshots.Exists(snapshot1Baseline)) {
                                num_baselines = 2;
                                baseline1 = entity.snapshots[snapshot1Baseline].start;
                                time1 = _server.m_Snapshots[snapshot1Baseline % _server.m_Snapshots.Length].serverTime;

                                if (snapshot2Baseline != snapshot1Baseline && entity.snapshots.Exists(snapshot2Baseline)) {
                                    num_baselines = 3;
                                    baseline2 = entity.snapshots[snapshot2Baseline].start;
                                    //time2 = entity.snapshots[snapshot2Baseline].serverTime;
                                    time2 = _server.m_Snapshots[snapshot2Baseline % _server.m_Snapshots.Length].serverTime;
                                }
                            }
                        }

                        entity.prediction = _server.m_Prediction + _server.m_PredictionIndex;
                        NetworkPrediction.PredictSnapshot(entity.prediction, entity.fieldsChangedPrediction, entityType.schema, num_baselines, (uint)time0, baseline0, (uint)time1, baseline1, (uint)time2, baseline2, (uint)_server.serverTime, entity.GetFieldMask(ConnectionId));
                        _server.m_PredictionIndex += entityType.schema.GetByteSize() / 4;
                        //_server.statsProcessedOutgoing += entityType.schema.GetByteSize();

                        if (UnsafeUtility.MemCmp(entity.prediction, snapshot.start, entityType.schema.GetByteSize()) != 0) {
                            _server.m_TempUpdateList.Add(id);
                        }

                        if (serverDebug.IntValue > 2) {
                            GameDebug.Log((haveBaseline ? "Upd [BL]" : "Upd [  ]") +
                                "num_baselines: " + num_baselines + " serverSequence: " + tickToSend + " " +
                                snapshot0Baseline + "(" + snapshot0BaselineClient + "," + time0 + ") - " +
                                snapshot1Baseline + "(" + snapshot1BaselineClient + "," + time1 + ") - " +
                                snapshot2Baseline + "(" + snapshot2BaselineClient + "," + time2 + "). Sche: " +
                                _server.m_TempTypeList.Count + " Spwns: " + _server.m_TempSpawnList.Count + " Desp: " + _server.m_TempDespawnList.Count + " Upd: " + _server.m_TempUpdateList.Count);
                        }
                    } else {
                        var prediction = baseline0;

                        var fcp = entity.fieldsChangedPrediction;
                        for (int i = 0, l = fcp.Length; i < l; ++i)
                            fcp[i] = 0;

                        if (UnsafeUtility.MemCmp(prediction, snapshot.start, entityType.schema.GetByteSize()) != 0) {
                            _server.m_TempUpdateList.Add(id);
                        }

                        if (serverDebug.IntValue > 2) {
                            GameDebug.Log((haveBaseline ? "Upd [BL]" : "Upd [  ]") + snapshot0Baseline + "(" + snapshot0BaselineClient + "," + time0 + "). Sche: " + _server.m_TempTypeList.Count + " Spwns: " + _server.m_TempSpawnList.Count + " Desp: " + _server.m_TempDespawnList.Count + " Upd: " + _server.m_TempUpdateList.Count);
                        }
                    }
                }
            }

            if (serverDebug.IntValue > 1 && (_server.m_TempSpawnList.Count > 0 || _server.m_TempDespawnList.Count > 0)) {
                GameDebug.Log(ConnectionId + ": spwns: " + string.Join(",", _server.m_TempSpawnList) + "    despwans: " + string.Join(",", _server.m_TempDespawnList));
            }

            foreach (var pair in _server.m_EntityTypes) {
                if (pair.Value.createdSequence > maxSnapshotAck)
                    _server.m_TempTypeList.Add(pair.Value);
            }

            output.WritePackedUInt((uint)_server.m_TempTypeList.Count, NetworkConfig.schemaCountContext);
            foreach (var typeInfo in _server.m_TempTypeList) {
                output.WritePackedUInt(typeInfo.typeId, NetworkConfig.schemaTypeIdContext);
                NetworkSchema.WriteSchema(typeInfo.schema, ref output);

                GameDebug.Assert(typeInfo.baseline != null);
                NetworkSchema.CopyFieldsFromBuffer(typeInfo.schema, typeInfo.baseline, ref output);
            }

            int previousId = 1;
            output.WritePackedUInt((uint)_server.m_TempSpawnList.Count, NetworkConfig.spawnCountContext);
            foreach (var id in _server.m_TempSpawnList) {
                output.WritePackedIntDelta(id, previousId, NetworkConfig.idContext);
                previousId = id;

                var entity = _server.m_Entities[id];

                output.WritePackedUInt((uint)entity.typeId, NetworkConfig.spawnTypeIdContext);
                output.WriteRawBits(entity.GetFieldMask(ConnectionId), 8);
            }

            output.WritePackedUInt((uint)_server.m_TempDespawnList.Count, NetworkConfig.despawnCountContext);
            foreach (var id in _server.m_TempDespawnList) {
                output.WritePackedIntDelta(id, previousId, NetworkConfig.idContext);
                previousId = id;
            }

            int numUpdates = _server.m_TempUpdateList.Count;
            output.WritePackedUInt((uint)numUpdates, NetworkConfig.updateCountContext);

            foreach (var id in _server.m_TempUpdateList) {
                var entity = _server.m_Entities[id];
                var entityType = _server.m_EntityTypes[entity.typeId];

                uint* prediction = null;
                if (enableNetworkPrediction) {
                    prediction = entity.prediction;
                } else {
                    prediction = entityType.baseline;
                    if (haveBaseline && entity.spawnSequence <= maxSnapshotAck) {
                        prediction = entity.snapshots[snapshot0Baseline].start;
                    }
                }

                output.WritePackedIntDelta(id, previousId, NetworkConfig.idContext);
                previousId = id;

                // TODO It is a mess that we have to repeat the logic about tickToSend from above here
                int tickToSend = _server.m_ServerSequence;
                if (entity.despawnSequence > 0)
                    tickToSend = Mathf.Max(entity.despawnSequence - 1, entity.updateSequence);

                GameDebug.Assert(_server.m_ServerSequence - tickToSend < NetworkConfig.snapshotDeltaCacheSize);

                if (!entity.snapshots.Exists(tickToSend)) {
                    GameDebug.Log("maxSnapAck: " + maxSnapshotAck);
                    GameDebug.Log("lastWritten: " + snapshotServerLastWritten);
                    GameDebug.Log("spawn: " + entity.spawnSequence);
                    GameDebug.Log("despawn: " + entity.despawnSequence);
                    GameDebug.Log("update: " + entity.updateSequence);
                    GameDebug.Log("tick: " + _server.m_ServerSequence);
                    GameDebug.Log("id: " + id);
                    GameDebug.Log("snapshots: " + entity.snapshots.ToString());
                    GameDebug.Log("WOULD HAVE crashed looking for " + tickToSend + " changing to " + (entity.despawnSequence - 1));
                    tickToSend = entity.despawnSequence - 1;
                    GameDebug.Assert(false, "Unable to find " + tickToSend + " in snapshots. Would update have worked?");
                }
                var snapshotInfo = entity.snapshots[tickToSend];

                // NOTE : As long as the server haven't gotten the spawn acked, it will keep sending
                // delta relative to 0 as we cannot know if we have a valid baseline on the client or not
                uint entity_hash = 0;
                DeltaWriter.Write(ref output, entityType.schema, snapshotInfo.start, prediction, entity.fieldsChangedPrediction, entity.GetFieldMask(ConnectionId), ref entity_hash);
            }

            if (!haveBaseline && serverDebug.IntValue > 0) {
                Debug.Log("Sending no-baseline snapshot. C: " + ConnectionId + " Seq: " + outSequence + " Max: " + maxSnapshotAck + "  Total entities sent: " + _server.m_TempUpdateList.Count + " Type breakdown:");
                //foreach (var c in _server.m_EntityTypes) {
                //    Debug.Log(c.Value.name + " " + c.Key + " #" + (c.Value.stats_count) + " " + (c.Value.stats_bits / 8) + " bytes");
                //}
            }

            snapshotSeqs[outSequence % NetworkConfig.clientAckCacheSize] = _server.m_ServerSequence;
            snapshotServerLastWritten = _server.m_ServerSequence;

            Profiler.EndSample();
        }

        protected override void NotifyDelivered(int sequence, ServerPackageInfo info, bool madeIt) {
            base.NotifyDelivered(sequence, info, madeIt);

            if (madeIt) {
                if ((info.Content & NetworkMessage.ClientInfo) != 0) {
                    clientInfoAcked = true;
                }

                // Check if the client received the map info
                if ((info.Content & NetworkMessage.MapInfo) != 0 && info.serverSequence >= _server.m_MapInfo.serverInitSequence) {
                    mapAcked = true;
                    mapSchemaAcked = true;
                }

                // Update the snapshot baseline if the client received the snapshot
                if (mapAcked && (info.Content & NetworkMessage.Snapshot) != 0) {
                    snapshotPackageBaseline = sequence;

                    GameDebug.Assert(snapshotSeqs[sequence % NetworkConfig.clientAckCacheSize] > 0, "Got ack for package we did not expect?");
                    snapshotAcks[sequence % NetworkConfig.clientAckCacheSize] = true;

                    // Keep track of newest ack'ed snapshot
                    if (info.serverSequence > maxSnapshotAck) {
                        if (maxSnapshotAck == 0 && serverDebug.IntValue > 0)
                            GameDebug.Log("SERVER: first max ack for " + info.serverSequence);
                        maxSnapshotAck = info.serverSequence;
                        maxSnapshotTime = info.serverTime;
                    }
                }
            }
        }

        public void Reset() {
            mapAcked = false;
            mapReady = false;
            maxSnapshotAck = 0;
            maxSnapshotTime = 0;
        }

        // flags for ack of individual snapshots indexed by client sequence
        bool[] snapshotAcks = new bool[NetworkConfig.clientAckCacheSize];
        // corresponding server baseline no for each client seq
        int[] snapshotSeqs = new int[NetworkConfig.clientAckCacheSize];
        public int maxSnapshotAck;
        int maxSnapshotTime;
        private int snapshotPackageBaseline;
        private int snapshotServerLastWritten;
        int snapshotInterval;
        double nextOutPackageTime;
        int maxBPS;

        CommandInfo defaultCommandInfo = new CommandInfo();
        int commandSequenceIn;
        int commandSequenceProcessed;
        NetworkSchema commandSchema;
        SequenceBuffer<CommandInfo> commandsIn = new SequenceBuffer<CommandInfo>(NetworkConfig.commandServerQueueSize, () => new CommandInfo());

        public bool clientInfoAcked;
        public bool mapReady;

        private bool mapAcked, mapSchemaAcked;
        private NetworkServer _server;

        class CommandInfo
        {
            public int time = 0;
            public uint[] data = new uint[NetworkConfig.maxCommandDataSize];
        }
    }

    public Dictionary<int, ServerConnection> GetConnections() {
        return _serverConnections;
    }

    WorldSnapshot[] m_Snapshots;
    uint* m_Prediction;
    int m_PredictionIndex;

    int m_ServerSequence = 1;

    // Entity count of entire snapshot
    uint m_LastEntityCount;

    float m_ServerSimTime;
    MapInfo m_MapInfo = new MapInfo();
    private Dictionary<int, ServerConnection> _serverConnections = new Dictionary<int, ServerConnection>();


    Dictionary<ushort, EntityTypeInfo> m_EntityTypes = new Dictionary<ushort, EntityTypeInfo>();
    List<EntityInfo> m_Entities = new List<EntityInfo>();
    List<int> m_FreeEntities = new List<int>();

    List<EntityTypeInfo> m_TempTypeList = new List<EntityTypeInfo>();
    List<int> m_TempSpawnList = new List<int>();
    List<int> m_TempDespawnList = new List<int>();
    List<int> m_TempUpdateList = new List<int>();
}

