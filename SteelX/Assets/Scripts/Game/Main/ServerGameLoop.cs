using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Profiling;

public class ServerGameWorld : ISnapshotGenerator, IClientCommandProcessor
{
    public int WorldTick { get { return _gameWorld.WorldTime.tick; } }
    public int TickRate {
        get {
            return _gameWorld.WorldTime.tickRate;
        }
        set {
            _gameWorld.WorldTime.tickRate = value;
        }
    }
    public float TickInterval { get { return _gameWorld.WorldTime.tickInterval; } }

    public ServerGameWorld(GameWorld world, BundledResourceManager resourceSystem, Dictionary<int, ServerGameLoop.ClientInfo> clients, NetworkServer networkServer) {
        _gameWorld = world;
        _networkServer = networkServer;
        m_Clients = clients;

        m_PlayerModule = new PlayerModuleServer(_gameWorld, resourceSystem);
        m_CharacterModule = new CharacterModuleServer(_gameWorld, resourceSystem);
        m_ReplicatedEntityModule = new ReplicatedEntityModuleServer(_gameWorld, resourceSystem, networkServer);
        m_ReplicatedEntityModule.ReserveSceneEntities(networkServer);

        movableSystemServer = new MovableSystemServer(_gameWorld, resourceSystem);

        m_GameModeSystem = _gameWorld.GetECSWorld().CreateSystem<GameModeSystemServer>(_gameWorld);
    }

    public void Update() {

    }

    public void LateUpdate() {
        m_CharacterModule.AttachmentUpdate();
    }

    public void ProcessCommand(int connectionId, int tick, ref NetworkReader data) {
        ServerGameLoop.ClientInfo client;
        if (!m_Clients.TryGetValue(connectionId, out client))
            return;

        if (client.player) {
            var serializeContext = new SerializeContext {
                entityManager = _gameWorld.GetEntityManager(),
                entity = Entity.Null,
                refSerializer = null,
                tick = tick
            };

            if (tick == _gameWorld.WorldTime.tick)
                client.latestCommand.Deserialize(ref serializeContext, ref data);

            // Pass on command to controlled entity
            if (client.player.controlledEntity != Entity.Null) {
                var userCommand = _gameWorld.GetEntityManager().GetComponentData<UserCommandComponentData>(
                    client.player.controlledEntity);

                userCommand.command = client.latestCommand;
                
                _gameWorld.GetEntityManager().SetComponentData(client.player.controlledEntity, userCommand);
            }
        }
    }

    public void ServerTickUpdate() {
        _gameWorld.WorldTime.tick++;
        _gameWorld.WorldTime.tickDuration = _gameWorld.WorldTime.tickInterval;
        _gameWorld.frameDuration = _gameWorld.WorldTime.tickInterval;

        // This call backs into ProcessCommand
        _networkServer.HandleClientCommands(_gameWorld.WorldTime.tick, this);

        // Handle spawn requests. All creation of game entities should happen in this phase        
        m_CharacterModule.HandleSpawnRequests();

        // Handle newly spawned entities          
        m_CharacterModule.HandleSpawns();
        m_ReplicatedEntityModule.HandleSpawning();

        // Start movement of scene objects. Scene objects that player movement
        // depends on should finish movement in this phase
        movableSystemServer.Update();

        // Update movement of player controlled units 
        m_CharacterModule.AbilityRequestUpdate();
        m_CharacterModule.MovementStart();
        m_CharacterModule.MovementResolve();
        m_CharacterModule.AbilityStart();
        m_CharacterModule.AbilityResolve();

        // Finalize movement of modules that only depend on data from previous frames
        // We want to wait as long as possible so queries potentially can be handled in jobs  

        // Handle damage

        m_CharacterModule.PresentationUpdate();

        m_GameModeSystem.Update();

        // Handle despawns
        m_CharacterModule.HandleDespawns();
        m_ReplicatedEntityModule.HandleDespawning();
        _gameWorld.ProcessDespawns();
    }

    public void HandleClientConnect(ServerGameLoop.ClientInfo client) {
        client.player = m_PlayerModule.CreatePlayer(_gameWorld, client.id, "", client.isReady);
    }

    public void HandleClientDisconnect(ServerGameLoop.ClientInfo client) {
        m_PlayerModule.CleanupPlayer(client.player);
        m_CharacterModule.CleanupPlayer(client.player);
    }

    public void Shutdown() {
        m_CharacterModule.Shutdown();
        m_PlayerModule.Shutdown();

        m_ReplicatedEntityModule.Shutdown();
        movableSystemServer.Shutdown();
    }

    public void GenerateEntitySnapshot(int entityId, ref NetworkWriter writer) {
        Profiler.BeginSample("ServerGameLoop.GenerateEntitySnapshot()");

        m_ReplicatedEntityModule.GenerateEntitySnapshot(entityId, ref writer);

        Profiler.EndSample();
    }

    public string GenerateEntityName(int entityId) {
        return m_ReplicatedEntityModule.GenerateName(entityId);
    }

    private GameWorld _gameWorld;
    private NetworkServer _networkServer;
    Dictionary<int, ServerGameLoop.ClientInfo> m_Clients;

    readonly GameModeSystemServer m_GameModeSystem;
    readonly PlayerModuleServer m_PlayerModule;
    readonly CharacterModuleServer m_CharacterModule;
    readonly ReplicatedEntityModuleServer m_ReplicatedEntityModule;
    readonly MovableSystemServer movableSystemServer;
}

public class ServerGameLoop : Game.IGameLoop, INetworkCallbacks
{
    private enum ServerState
    {
        Connecting,
        Loading,
        Active,
    }

    private NetworkServer _networkServer;
    private ServerGameWorld _serverGameWorld;
    private GameWorld _gameWorld;
    private StateMachine<ServerState> _stateMachine;

    public bool Init(string[] args) {
        _gameWorld = new GameWorld("ServerWorld");
        _networkServer = new NetworkServer(new ServerPhotonNetworkTransport());
        _networkStatistics = new NetworkStatisticsServer(_networkServer);

        _stateMachine = new StateMachine<ServerState>();
        _stateMachine.Add(ServerState.Connecting, EnterConnectingState, UpdateConnectingState, null);
        _stateMachine.Add(ServerState.Loading, EnterLoadingState, UpdateLoadingState, null);
        _stateMachine.Add(ServerState.Active, EnterActiveState, UpdateActiveState, LeaveActiveState);

        _networkServer.UpdateClientInfo();

        _stateMachine.SwitchTo(ServerState.Connecting);

        return true;
    }

    private void EnterConnectingState() {
        _networkServer.Connect();
    }

    private void UpdateConnectingState() {
        if (_networkServer.IsConnected) {
            _stateMachine.SwitchTo(ServerState.Loading);
        }
    }

    private void EnterLoadingState() {
        Game.Instance.levelManager.LoadLevel("testscene");
    }

    private void UpdateLoadingState() {
        if (Game.Instance.levelManager.IsCurrentLevelLoaded())
            _stateMachine.SwitchTo(ServerState.Active);
    }

    private void EnterActiveState() {
        GameDebug.Assert(_serverGameWorld == null);

        _gameWorld.RegisterSceneEntities();

        m_resourceSystem = new BundledResourceManager(_gameWorld, "BundledResources/Server");

        _serverGameWorld = new ServerGameWorld(_gameWorld, m_resourceSystem, m_Clients, _networkServer);

        _networkServer.InitializeMap((ref NetworkWriter data) => {
            data.WriteString("name", "testscene");
        });

        m_nextTickTime = Game.frameTime;

        foreach (var pair in m_Clients) {
            _serverGameWorld.HandleClientConnect(pair.Value);
        }
    }

    Dictionary<int, int> m_TickStats = new Dictionary<int, int>();
    private void UpdateActiveState() {
        int tickCount = 0;
        while (Game.frameTime > m_nextTickTime) {
            tickCount++;
            _serverGameWorld.ServerTickUpdate();

            Profiler.BeginSample("GenerateSnapshots");
            _networkServer.GenerateSnapshot(_serverGameWorld, m_LastSimTime);
            Profiler.EndSample();

            m_nextTickTime += _serverGameWorld.TickInterval;
            m_performLateUpdate = true;
        }

        //
        // If running as headless we nudge the Application.targetFramerate back and forth
        // around the actual framerate -- always trying to have a remaining time of half a frame
        // The goal is to have the while loop above tick exactly 1 time
        //
        // The reason for using targetFramerate is to allow Unity to sleep between frames
        // reducing cpu usage on server.
        //
        if (Game.IsHeadless) {
            float remainTime = (float)(m_nextTickTime - Game.frameTime);

            int rate = _serverGameWorld.TickRate;
            if (remainTime > 0.75f * _serverGameWorld.TickInterval)
                rate -= 2;
            else if (remainTime < 0.25f * _serverGameWorld.TickInterval)
                rate += 2;

            Application.targetFrameRate = rate;

            //
            // Show some stats about how many world ticks per unity update we have been running
            //
            if (debugServerTickStats.IntValue > 0) {
                if (Time.frameCount % 10 == 0)
                    GameDebug.Log(remainTime + ":" + rate);

                if (!m_TickStats.ContainsKey(tickCount))
                    m_TickStats[tickCount] = 0;
                m_TickStats[tickCount] = m_TickStats[tickCount] + 1;
                if (Time.frameCount % 100 == 0) {
                    foreach (var p in m_TickStats) {
                        GameDebug.Log(p.Key + ":" + p.Value);
                    }
                }
            }
        }
    }

    private void LeaveActiveState() {
        m_resourceSystem.Shutdown();
    }

    public void Update() {
        m_SimStartTime = Game.Instance.Clock.ElapsedTicks;
        m_SimStartTimeTick = _serverGameWorld != null ? _serverGameWorld.WorldTick : 0;

        UpdateNetwork();

        _stateMachine.Update();

        _networkServer.Update(this);
        _networkServer.SendData();
        _networkStatistics.Update();
    }

    private void UpdateNetwork() {
        // If serverTickrate was changed, update both game world and info
        if ((ConfigVar.DirtyFlags & ConfigVar.Flags.ServerInfo) == ConfigVar.Flags.ServerInfo) {
            _networkServer.UpdateClientInfo();
            ConfigVar.DirtyFlags &= ~ConfigVar.Flags.ServerInfo;
        }

        if (_serverGameWorld != null && _serverGameWorld.TickRate != Game.serverTickRate.IntValue)
            _serverGameWorld.TickRate = Game.serverTickRate.IntValue;
    }

    public void FixedUpdate() {
    }

    public void LateUpdate() {
        if (_serverGameWorld != null && m_SimStartTimeTick != _serverGameWorld.WorldTick) {
            // Only update sim time if we actually simulatated
            // TODO : remove this when targetFrameRate works the way we want it.
            m_LastSimTime = Game.Instance.Clock.GetTicksDeltaAsMilliseconds(m_SimStartTime);
        }

        if (m_performLateUpdate) {
            _serverGameWorld.LateUpdate();
            m_performLateUpdate = false;
        }
    }

    public void Shutdown() {
        _networkServer.Shutdown();
        _serverGameWorld.Shutdown();
        Game.Instance.levelManager.UnloadLevel();
        _gameWorld.Shutdown();
        _gameWorld = null;
    }

    public void OnConnect(int clientId) {
        var client = new ClientInfo();
        client.id = clientId;
        m_Clients.Add(clientId, client);

        if (_serverGameWorld != null)
            _serverGameWorld.HandleClientConnect(client);
    }

    public void OnDisconnect(int clientId) {
        ClientInfo client;

        if (m_Clients.TryGetValue(clientId, out client)) {
            if (_serverGameWorld != null)
                _serverGameWorld.HandleClientDisconnect(client);

            m_Clients.Remove(clientId);
        }
    }

    unsafe public void OnEvent(int clientId, NetworkEvent info) {
        var client = m_Clients[clientId];
        var type = info.type.typeId;

        fixed (uint* data = info.data) {
            var reader = new NetworkReader(data, info.type.schema);

            switch ((GameNetworkEvents.EventType)type) {
                case GameNetworkEvents.EventType.PlayerReady:
                _networkServer.MapReady(clientId); // TODO hacky
                client.isReady = true;
                break;
            }
        }
    }

    public class ClientInfo
    {
        public int id;
        public PlayerSettings playerSettings = new PlayerSettings();
        public bool isReady;
        public PlayerState player;
        public UserCommand latestCommand = UserCommand.defaultCommand;
    }

    BundledResourceManager m_resourceSystem;
    private NetworkStatisticsServer _networkStatistics;
    Dictionary<int, ClientInfo> m_Clients = new Dictionary<int, ClientInfo>();

    public double m_nextTickTime = 0;
    
    long m_SimStartTime;
    int m_SimStartTimeTick;
    private bool m_performLateUpdate;
    private float m_LastSimTime;

    [ConfigVar(Name = "debug.servertickstats", DefaultValue = "0", Description = "Show stats about how many ticks we run per Unity update (headless only)")]
    static ConfigVar debugServerTickStats;
}

