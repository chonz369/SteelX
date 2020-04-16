using System.Collections.Generic;
using UnityEngine;

public class ClientGameWorld{
    
    public GameTime PredictedTime
    {
        get { return m_PredictedTime; }
    }

    public GameTime RenderTime
    {
        get { return m_RenderTime; }
    }

    public ClientGameWorld(GameWorld world, NetworkClient networkClient, NetworkStatisticsClient _networkStatistics, BundledResourceManager resourceSystem) {
        _gameWorld = world;
        _networkClient = networkClient;
        this._networkStatistics = _networkStatistics;

        m_CharacterModule = new CharacterModuleClient(_gameWorld, resourceSystem);
        m_PlayerModule = new PlayerModuleClient(_gameWorld);
        m_ReplicatedEntityModule = new ReplicatedEntityModuleClient(_gameWorld, resourceSystem);
    }

    // This is called at the actual client frame rate, so may be faster or slower than tickrate.
    public void Update(float frameDuration) {
        // Advances time and accumulate input into the UserCommand being generated
        HandleTime(frameDuration);
        _gameWorld.WorldTime = m_RenderTime;
        _gameWorld.frameDuration = frameDuration;
        _gameWorld.lastServerTick = _networkClient.serverTime;

        m_PlayerModule.ResolveReferenceFromLocalPlayerToPlayer();
        m_PlayerModule.HandleCommandReset();
        m_ReplicatedEntityModule.UpdateControlledEntityFlags();

        // Handle spawn requests

        // Handle spawning  
        m_CharacterModule.HandleSpawns();
        m_PlayerModule.HandleSpawn();

        // Update movement of scene objects. Projectiles and grenades can also start update as they use collision data from last frame

        m_ReplicatedEntityModule.Interpolate(m_RenderTime);

        // Handle controlled entity changed
        m_PlayerModule.HandleControlledEntityChanged();
        //m_CharacterModule.HandleControlledEntityChanged();

        // Prediction
        _gameWorld.WorldTime = m_PredictedTime;

        if (IsPredictionAllowed()) {
            // ROLLBACK. All predicted entities (with the ServerEntity component) are rolled back to last server state 
            _gameWorld.WorldTime.SetTime(_networkClient.serverTime, m_PredictedTime.tickInterval);
            PredictionRollback();


            // PREDICT PREVIOUS TICKS. Replay every tick *after* the last tick we have from server up to the last stored command we have
            for (var tick = _networkClient.serverTime + 1; tick < m_PredictedTime.tick; tick++) {
                _gameWorld.WorldTime.SetTime(tick, m_PredictedTime.tickInterval);
                m_PlayerModule.RetrieveCommand(_gameWorld.WorldTime.tick);
                PredictionUpdate();
#if UNITY_EDITOR                 
                // We only want to store "full" tick to we use m_PredictedTime.tick-1 (as current can be fraction of tick)
                m_ReplicatedEntityModule.StorePredictedState(tick, m_PredictedTime.tick - 1);
#endif                
            }

            // PREDICT CURRENT TICK. Update current tick using duration of current tick
            _gameWorld.WorldTime = m_PredictedTime;
            m_PlayerModule.RetrieveCommand(_gameWorld.WorldTime.tick);
            // Dont update systems with close to zero time. 
            if (_gameWorld.WorldTime.tickDuration > 0.008f) {
                PredictionUpdate();
            }
            //#if UNITY_EDITOR                 
            //            m_ReplicatedEntityModule.StorePredictedState(m_PredictedTime.tick, m_PredictedTime.tick);
            //#endif                
        }

        //update presentation
        _gameWorld.WorldTime = m_PredictedTime;
        m_CharacterModule.UpdatePresentation();

        _gameWorld.WorldTime = m_RenderTime;

        //Handle despawns
        m_CharacterModule.HandleDespawns();
        _gameWorld.ProcessDespawns();
    }

    public void LateUpdate(float delta) {
        m_CharacterModule.CameraUpdate();
        m_PlayerModule.CameraUpdate();

        m_CharacterModule.LateUpdate();
    }

    private void HandleTime(float frameDuration) {
        // Update tick rate (this will only change runtime in test scenarios)
        // TODO consider use ConfigVars with Server flag for this
        if (_networkClient.serverTickRate != m_PredictedTime.tickRate) {
            m_PredictedTime.tickRate = _networkClient.serverTickRate;
            m_RenderTime.tickRate = _networkClient.serverTickRate;
        }

        // Sample input into current command
        //  The time passed in here is used to calculate the amount of rotation from stick position
        //  The command stores final view direction
        bool userInputEnabled = Game.GetMousePointerLock();

        m_PlayerModule.SampleInput(userInputEnabled, Time.deltaTime, m_RenderTime.tick);

        int prevTick = m_PredictedTime.tick;

        // Increment time
        var deltaPredictedTime = frameDuration * frameTimeScale;
        m_PredictedTime.AddDuration(deltaPredictedTime);

        // Adjust time to be synchronized with server
        int preferredBufferedCommandCount = 2;
        int preferredTick = _networkClient.serverTime + (int)(((_networkClient.timeSinceSnapshot + _networkStatistics.rtt.average) / 1000.0f) * _gameWorld.WorldTime.tickRate) + preferredBufferedCommandCount;
        
        bool resetTime = false;
        if (!resetTime && m_PredictedTime.tick < preferredTick - 3) {
            GameDebug.Log(string.Format("Client hard catchup ... "));
            resetTime = true;
        }

        if (!resetTime && m_PredictedTime.tick > preferredTick + 6) {
            GameDebug.Log(string.Format("Client hard slowdown ... "));
            resetTime = true;
        }
        frameTimeScale = 1.0f;

        if (resetTime) {
            GameDebug.Log(string.Format("CATCHUP ({0} -> {1})", m_PredictedTime.tick, preferredTick));

            _networkStatistics.notifyHardCatchup = true;
            m_PredictedTime.tick = preferredTick;
            m_PredictedTime.SetTime(preferredTick, 0);

        } else {
            int bufferedCommands = _networkClient.lastAcknowlegdedCommandTime - _networkClient.serverTime;
            if (bufferedCommands < preferredBufferedCommandCount)
                frameTimeScale = 1.01f;

            if (bufferedCommands > preferredBufferedCommandCount)
                frameTimeScale = 0.99f;
        }

        // Increment interpolation time
        m_RenderTime.AddDuration(frameDuration * frameTimeScale);

        // Force interp time to not exeede server time
        if (m_RenderTime.tick >= _networkClient.serverTime) {
            m_RenderTime.SetTime(_networkClient.serverTime, 0);
        }

        // hard catchup
        if (m_RenderTime.tick < _networkClient.serverTime - 10) {
            m_RenderTime.SetTime(_networkClient.serverTime - 8, 0);
        }

        // Throttle up to catch up
        if (m_RenderTime.tick < _networkClient.serverTime - 1) {
            m_RenderTime.AddDuration(frameDuration * 0.01f);
        }

        // If predicted time has entered a new tick the stored commands should be sent to server 
        if (m_PredictedTime.tick > prevTick) {
            var oldestCommandToSend = Mathf.Max(prevTick, m_PredictedTime.tick - NetworkConfig.commandClientBufferSize);
            for (int tick = oldestCommandToSend; tick < m_PredictedTime.tick; tick++) {
                m_PlayerModule.StoreCommand(tick);
                m_PlayerModule.SendCommand(tick);
            }

            m_PlayerModule.ResetInput(userInputEnabled);
            m_PlayerModule.StoreCommand(m_PredictedTime.tick);
        }

        // Store command
        m_PlayerModule.StoreCommand(m_PredictedTime.tick);
    }

    public LocalPlayer RegisterLocalPlayer(int playerId) {
        m_ReplicatedEntityModule.SetLocalPlayerId(playerId);
        m_localPlayer = m_PlayerModule.RegisterLocalPlayer(playerId, _networkClient);
        return m_localPlayer;
    }

    bool IsPredictionAllowed() {
        if (!m_PlayerModule.PlayerStateReady) {
            GameDebug.Log("No predict! No player state.");
            return false;
        }

        if (!m_PlayerModule.IsControllingEntity) {
            GameDebug.Log("No predict! No controlled entity.");
            return false;
        }

        if (m_PredictedTime.tick <= _networkClient.serverTime) {
            GameDebug.Log("No predict! Predict time not ahead of server tick! " + GetFramePredictInfo());
            return false;
        }

        if (!m_PlayerModule.HasCommands(_networkClient.serverTime + 1, m_PredictedTime.tick)) {
            GameDebug.Log("No predict! No commands available. " + GetFramePredictInfo());
            return false;
        }

        return true;
    }

    string GetFramePredictInfo() {
        int firstCommandTick;
        int lastCommandTick;
        m_PlayerModule.GetBufferedCommandsTick(out firstCommandTick, out lastCommandTick);

        return string.Format("Last server:{0} predicted:{1} buffer:{2}->{3} time since snap:{4}  rtt avr:{5}",
            _networkClient.serverTime, m_PredictedTime.tick,
            firstCommandTick, lastCommandTick,
            _networkClient.timeSinceSnapshot, _networkStatistics.rtt.average);
    }

    public void Shutdown() {
        m_CharacterModule.Shutdown();
        m_ReplicatedEntityModule.Shutdown();
        m_PlayerModule.Shutdown();
    }

    public ISnapshotConsumer GetSnapshotConsumer() {
        return m_ReplicatedEntityModule;
    }

    void PredictionRollback() {
        m_ReplicatedEntityModule.Rollback();
    }

    void PredictionUpdate() {
        //m_SpectatorCamModule.Update();

        m_CharacterModule.AbilityRequestUpdate();

        //m_CharacterModule.MovementStart();
        //m_CharacterModule.MovementResolve();

        m_CharacterModule.AbilityStart();
        m_CharacterModule.AbilityResolve();
    }


    public float frameTimeScale = 1.0f;
    private GameTime m_RenderTime = new GameTime(60);
    private GameTime m_PredictedTime = new GameTime(60);

    private GameWorld _gameWorld;
    private NetworkClient _networkClient;
    private NetworkStatisticsClient _networkStatistics;

    LocalPlayer m_localPlayer;
    readonly ReplicatedEntityModuleClient m_ReplicatedEntityModule;
    readonly PlayerModuleClient m_PlayerModule;
    readonly CharacterModuleClient m_CharacterModule;
}

public class ClientGameLoop : Game.IGameLoop, INetworkClientCallbacks
{
    private enum ClientState
    {
        Connecting,
        Loading,
        Playing,
        Leaving,
    }
    // Client vars
    [ConfigVar(Name = "client.updaterate", DefaultValue = "30000", Description = "Max bytes/sec client wants to receive", Flags = ConfigVar.Flags.ClientInfo)]
    public static ConfigVar clientUpdateRate;
    [ConfigVar(Name = "client.updateinterval", DefaultValue = "3", Description = "Snapshot sendrate requested by client", Flags = ConfigVar.Flags.ClientInfo)]
    public static ConfigVar clientUpdateInterval;

    private GameWorld _gameWorld;
    private NetworkClient _networkClient;
    private NetworkStatisticsClient _networkStatisticsClient;

    private StateMachine<ClientState> _stateMachine;
    private ClientGameWorld _clientGameWorld;

    public bool Init(string[] args) {
        _stateMachine = new StateMachine<ClientState>();
        _stateMachine.Add(ClientState.Connecting, EnterConnectingState, UpdateConnectingState, null);
        _stateMachine.Add(ClientState.Loading, EnterLoadingState, UpdateLoadingState, null);
        _stateMachine.Add(ClientState.Playing, EnterPlayingState, UpdatePlayingState, null);
        _stateMachine.Add(ClientState.Leaving, EnterLeavingState, UpdateLeavingState, null);

        _networkClient = new NetworkClient(new ClientPhotonNetworkTransport());
        _networkStatisticsClient = new NetworkStatisticsClient(_networkClient);

        _networkClient.UpdateClientConfig();

        _stateMachine.SwitchTo(ClientState.Connecting);

        return true;
    }

    private double timeout;

    private void EnterConnectingState() {
        timeout = Game.frameTime;
        _networkClient.Connect();
    }
    
    private void UpdateConnectingState() {
        if (_networkClient.IsConnected) {
            //wait map update
            timeout = Game.frameTime;
        } else if(Game.frameTime - timeout > 10) {//Todo : fix this constant
            GameDebug.Log("Client timeout. Leaving.");
            _stateMachine.SwitchTo(ClientState.Leaving);
        }        
    }

    private void EnterLoadingState() {
        Console.SetOpen(false);
    }

    private void UpdateLoadingState() {
        // Wait until we got level info
        if (m_LevelName == null)
            return;

        // Load if we are not already loading
        var level = Game.Instance.levelManager.currentLevel;
        if (level == null || level.name != m_LevelName) {
            if (!Game.Instance.levelManager.LoadLevel(m_LevelName)) {
                _networkClient.Disconnect();
                return;
            }
            level = Game.Instance.levelManager.currentLevel;
        }

        // Wait for level to be loaded
        if (level.state == LevelState.Loaded)
            _stateMachine.SwitchTo(ClientState.Playing);
    }

    private void EnterPlayingState() {
        GameDebug.Assert(_gameWorld == null && Game.Instance.levelManager.IsCurrentLevelLoaded());

        _gameWorld = new GameWorld("ClientWorld");

        _gameWorld.RegisterSceneEntities();

        m_resourceSystem = new BundledResourceManager(_gameWorld, "BundledResources/Client");

        _clientGameWorld = new ClientGameWorld(_gameWorld, _networkClient, _networkStatisticsClient, m_resourceSystem);

        m_LocalPlayer = _clientGameWorld.RegisterLocalPlayer(_networkClient.clientId);

        _networkClient.QueueEvent((ushort)GameNetworkEvents.EventType.PlayerReady, true, (ref NetworkWriter data) => { });
    }

    private void UpdatePlayingState() {
        // Handle disconnects
        if (!_networkClient.IsConnected) {
            _stateMachine.SwitchTo(ClientState.Leaving);
            return;
        }

        // (re)send client info if any of the configvars that contain clientinfo has changed
        if ((ConfigVar.DirtyFlags & ConfigVar.Flags.ClientInfo) == ConfigVar.Flags.ClientInfo) {
            _networkClient.UpdateClientConfig();
            ConfigVar.DirtyFlags &= ~ConfigVar.Flags.ClientInfo;
        }

        float frameDuration = m_lastFrameTime != 0 ? (float)(Game.frameTime - m_lastFrameTime) : 0;
        m_lastFrameTime = Game.frameTime;

        _clientGameWorld.Update(frameDuration);
        m_performGameWorldLateUpdate = true;
    }

    private void LeavePlayingState() {
        m_LocalPlayer = null;

        m_resourceSystem.Shutdown();

        _clientGameWorld.Shutdown();

        _gameWorld.Shutdown();
    }

    private void EnterLeavingState() {
    }

    private void UpdateLeavingState() {
    }

    public void Update() {
        _networkClient.Update(this, _clientGameWorld?.GetSnapshotConsumer());

        _networkClient.SendData();

        if (_clientGameWorld != null)
            _networkStatisticsClient.Update();

        _stateMachine.Update();
    }

    public void FixedUpdate() {
    }

    public void LateUpdate() {
        if (_gameWorld != null && m_performGameWorldLateUpdate) {
            m_performGameWorldLateUpdate = false;
            _clientGameWorld.LateUpdate(Time.deltaTime);
        }
    }

    public void Shutdown() {
        _networkClient.Disconnect();

        _gameWorld.Shutdown();
    }

    public void OnMapUpdate(ref NetworkReader reader) {
        m_LevelName = reader.ReadString();
        GameDebug.Log("map : " + m_LevelName);
        if (_stateMachine.CurrentState() != ClientState.Loading)//in case map update when loading
            _stateMachine.SwitchTo(ClientState.Loading);
    }

    public void OnEvent(int clientId, NetworkEvent info) {
    }

    public void OnConnect(int clientId) {}

    public void OnDisconnect(int clientId) {}

    private string m_LevelName;
    private bool m_performGameWorldLateUpdate;
    private double m_lastFrameTime;

    LocalPlayer m_LocalPlayer;
    BundledResourceManager m_resourceSystem;
}

