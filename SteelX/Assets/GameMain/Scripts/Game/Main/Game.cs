using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public struct GameTime
{
    /// <summary>Number of ticks per second.</summary>
    public int tickRate {
        get { return m_tickRate; }
        set {
            m_tickRate = value;
            tickInterval = 1.0f / m_tickRate;
        }
    }

    /// <summary>Length of each world tick at current tickrate, e.g. 0.0166s if ticking at 60fps.</summary>
    public float tickInterval { get; private set; }     // Time between ticks
    public int tick;                    // Current tick   
    public float tickDuration;          // Duration of current tick

    public GameTime(int tickRate) {
        this.m_tickRate = tickRate;
        this.tickInterval = 1.0f / m_tickRate;
        this.tick = 1;
        this.tickDuration = 0;
    }

    public float TickDurationAsFraction {
        get { return tickDuration / tickInterval; }
    }

    public void SetTime(int tick, float tickDuration) {
        this.tick = tick;
        this.tickDuration = tickDuration;
    }

    public float DurationSinceTick(int tick) {
        return (this.tick - tick) * tickInterval + tickDuration;
    }

    public void AddDuration(float duration) {
        tickDuration += duration;
        int deltaTicks = Mathf.FloorToInt(tickDuration * (float)tickRate);
        tick += deltaTicks;
        tickDuration = tickDuration % tickInterval;
    }

    public static float GetDuration(GameTime start, GameTime end) {
        if (start.tickRate != end.tickRate) {
            GameDebug.LogError("Trying to compare time with different tick rates (" + start.tickRate + " and " + end.tickRate + ")");
            return 0;
        }

        float result = (end.tick - start.tick) * start.tickInterval + end.tickDuration - start.tickDuration;
        return result;
    }

    int m_tickRate;
}

[DefaultExecutionOrder(-1000)]
public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }
    public WeakAssetReference movableBoxPrototype;

    [ConfigVar(Name = "server.tickrate", DefaultValue = "60", Description = "Tickrate for server", Flags = ConfigVar.Flags.ServerInfo)]
    public static ConfigVar serverTickRate;
    [ConfigVar(Name = "config.mousesensitivity", DefaultValue = "2.5", Description = "Mouse sensitivity", Flags = ConfigVar.Flags.Save)]
    public static ConfigVar configMouseSensitivity;
    [ConfigVar(Name = "config.inverty", DefaultValue = "0", Description = "Invert y mouse axis", Flags = ConfigVar.Flags.Save)]
    public static ConfigVar configInvertY;

    public static readonly string k_BootConfigFilename = "boot.cfg";

    public System.Diagnostics.Stopwatch Clock { get; private set; }
    private long m_StopwatchFrequency;

    public static GameConfiguration config;
    public static InputSystem inputSystem;
    public LevelManager levelManager;
    public Camera bootCamera;

    public static class Input
    {
        [Flags]
        public enum Blocker
        {
            None = 0,
            Console = 1,
            Chat = 2,
            Debug = 4,
        }
        static Blocker blocks;

        public static void SetBlock(Blocker b, bool value) {
            if (value)
                blocks |= b;
            else
                blocks &= ~b;
        }

        internal static float GetAxisRaw(string axis) {
            return blocks != Blocker.None ? 0.0f : UnityEngine.Input.GetAxisRaw(axis);
        }

        internal static bool GetKey(KeyCode key) {
            return blocks != Blocker.None ? false : UnityEngine.Input.GetKey(key);
        }

        internal static bool GetKeyDown(KeyCode key) {
            return blocks != Blocker.None ? false : UnityEngine.Input.GetKeyDown(key);
        }

        internal static bool GetMouseButton(int button) {
            return blocks != Blocker.None ? false : UnityEngine.Input.GetMouseButton(button);
        }

        internal static bool GetKeyUp(KeyCode key) {
            return blocks != Blocker.None ? false : UnityEngine.Input.GetKeyUp(key);
        }
    }

    public static double frameTime;
    public interface IGameLoop
    {
        bool Init(string[] args);
        void Update();
        void FixedUpdate();
        void LateUpdate();
        void Shutdown();
    }

    private void Awake() {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        ConfigVar.Init();
        Application.targetFrameRate = -1;//default is to have no frame caps

        var commandLineArgs = new List<string>(System.Environment.GetCommandLineArgs());
#if UNITY_STANDALONE_LINUX || UNITY_SERVER
        IsHeadless = true;
#else
        IsHeadless = commandLineArgs.Contains("-batchmode");
        QualitySettings.vSyncCount = 0;//need to make the target frame rate work even in headless mode
#endif
        InitConsole(IsHeadless, commandLineArgs);

        if(IsHeadless)
            Application.targetFrameRate = 60;

        RegisterConsoleCommands();
        Console.SetOpen(true);

        m_StopwatchFrequency = System.Diagnostics.Stopwatch.Frequency;
        Clock = new System.Diagnostics.Stopwatch();
        Clock.Start();

        levelManager = new LevelManager();
        levelManager.Init();

        inputSystem = new InputSystem();

        //Instantiate here to avoid making changes to asset file.
        config = Instantiate((GameConfiguration)Resources.Load("GameConfiguration"));

        PushCamera(bootCamera);
    }

    private void Update()
    {
        if (_requestedGameLoopTypes.Count > 0) {
            // Multiple running gameloops only allowed in editor
#if !UNITY_EDITOR
            ShutdownGameLoops();
#endif
            bool initSucceeded = true;
            for (int i = 0; i < _requestedGameLoopTypes.Count; i++) {
                try {
                    IGameLoop gameLoop = (IGameLoop)System.Activator.CreateInstance(_requestedGameLoopTypes[i]);
                    initSucceeded = gameLoop.Init(_requestedGameLoopArgs[i]);
                    if (!initSucceeded)
                        break;

                    _gameLoops.Add(gameLoop);
                }catch(System.Exception e) {
                    GameDebug.Log(string.Format("Game loop initialization threw exception : ({0})\n{1}", e.Message, e.StackTrace));
                }
            }

            if (!initSucceeded) {
                ShutdownGameLoops();

                GameDebug.Log("Game loop initialization failed ... reverting to boot loop");
            }

            _requestedGameLoopArgs.Clear();
            _requestedGameLoopTypes.Clear();
        }

        // Verify if camera was somehow destroyed and pop it
        if (m_CameraStack.Count > 1 && m_CameraStack[m_CameraStack.Count - 1] == null) {
            PopCamera(null);
        }

        frameTime = (double)Clock.ElapsedTicks / m_StopwatchFrequency;
        
        try {
            if (!_errorState) {
                foreach (var gameLoop in _gameLoops) {
                    gameLoop.Update();
                }
                levelManager.Update();
            }
        } catch(System.Exception e) {
            HandleGameloopException(e);
        }

        Console.ConsoleUpdate();

        WindowFocusUpdate();
    }

    private void FixedUpdate() {
        try {
            if (!_errorState) {
                foreach (var gameLoop in _gameLoops) {
                    gameLoop.FixedUpdate();
                }
            }
        } catch (System.Exception e) {
            HandleGameloopException(e);
        }
    }

    private void LateUpdate() {
        try {
            if (!_errorState) {
                foreach (var gameLoop in _gameLoops) {
                    gameLoop.LateUpdate();
                }
            }
        } catch (System.Exception e) {
            HandleGameloopException(e);
        }

        Console.ConsoleLateUpdate();
    }

    private void InitConsole(bool isHeadless, List<string> commandLineArgs) {
        if (IsHeadless) {
#if UNITY_STANDALONE_WIN
            string consoleTitle = "SteelX" + " [" + System.Diagnostics.Process.GetCurrentProcess().Id + "]";
            var consoleRestoreFocus = commandLineArgs.Contains("-consolerestorefocus");

            var consoleUI = new ConsoleTextWin(consoleTitle, consoleRestoreFocus);
#elif UNITY_STANDALONE_LINUX
            var consoleUI = new ConsoleTextLinux();
#else
            UnityEngine.Debug.Log("WARNING: starting without a console");
            var consoleUI = new ConsoleNullUI();
#endif
            Console.Init(consoleUI);
        } else {//in-game UI
            var consoleUI = Instantiate(Resources.Load<ConsoleGUI>("Prefabs/ConsoleGUI"));
            DontDestroyOnLoad(consoleUI);
            Console.Init(consoleUI);
        }
    }

    private void HandleGameloopException(System.Exception e) {
        GameDebug.Log("EXCEPTION " + e.Message + "\n" + e.StackTrace);
        Console.SetOpen(true);
        _errorState = true;
    }

    public void RequestGameLoop(System.Type type, string[] args) {
        GameDebug.Assert(typeof(IGameLoop).IsAssignableFrom(type));

        _requestedGameLoopTypes.Add(type);
        _requestedGameLoopArgs.Add(args);
        GameDebug.Log("Game loop " + type + " requested");
    }

    private void ShutdownGameLoops() {
        foreach (var gameLoop in _gameLoops) {
            gameLoop.Shutdown();
        }
        _gameLoops.Clear();
    }

    public static T GetGameLoop<T>() where T : class {
        if (Instance == null)
            return null;
        foreach (var gameLoop in Instance._gameLoops) {
            T result = gameLoop as T;
            if (result != null)
                return result;
        }
        return null;
    }

    private void OnDestroy() {
        Console.RemoveCommandsWithTag(this.GetHashCode());
        Console.Shutdown();
    }

    public Camera TopCamera() {
        var c = m_CameraStack.Count;
        return c == 0 ? null : m_CameraStack[c - 1];
    }

    public void PushCamera(Camera cam) {
        if (m_CameraStack.Count > 0)
            SetCameraEnabled(m_CameraStack[m_CameraStack.Count - 1], false);
        m_CameraStack.Add(cam);
        SetCameraEnabled(cam, true);
        //m_ExposureReleaseCount = 10;
    }

    public void PopCamera(Camera cam) {
        GameDebug.Assert(m_CameraStack.Count > 1, "Trying to pop last camera off stack!");
        GameDebug.Assert(cam == m_CameraStack[m_CameraStack.Count - 1]);
        if (cam != null)
            SetCameraEnabled(cam, false);
        m_CameraStack.RemoveAt(m_CameraStack.Count - 1);
        SetCameraEnabled(m_CameraStack[m_CameraStack.Count - 1], true);
    }

    void SetCameraEnabled(Camera cam, bool enabled) {
        cam.enabled = enabled;
        var audioListener = cam.GetComponent<AudioListener>();
        if (audioListener != null) {
            audioListener.enabled = enabled;
        }
    }

    //Commands
    //=======================================================
    private void RegisterConsoleCommands() {
        Console.AddCommand("serve", CmdServe, "Start server listening", this.GetHashCode());
        Console.AddCommand("client", CmdClient, "client: Enter client mode.", this.GetHashCode());
        Console.AddCommand("preview", CmdPreview, "Start preview mode");
        Console.AddCommand("boot", CmdBoot, "Go back to boot loop", this.GetHashCode());

        Console.AddCommand("quit", CmdQuit, "Quits", this.GetHashCode());
    }

    private void CmdClient(string[] args) {
        RequestGameLoop(typeof(ClientGameLoop), args);
    }

    private void CmdServe(string[] args) {
        RequestGameLoop(typeof(ServerGameLoop), args);
    }

    void CmdPreview(string[] args) {
        RequestGameLoop(typeof(PreviewGameLoop), args);
    }

    void CmdBoot(string[] args) {
        levelManager.UnloadLevel();

        ShutdownGameLoops();
    }

    private void CmdQuit(string[] args) {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    //=======================================================

    public static void RequestMousePointerLock() {
        s_bMouseLockFrameNo = Time.frameCount + 1;
    }

    public static void SetMousePointerLock(bool locked) {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
        s_bMouseLockFrameNo = Time.frameCount; // prevent default handling in WindowFocusUpdate overriding requests
    }

    public static bool GetMousePointerLock() {
        return Cursor.lockState == CursorLockMode.Locked;
    }

    void WindowFocusUpdate() {
        bool lockWhenClicked = !Console.IsOpen();

        if (s_bMouseLockFrameNo == Time.frameCount) {
            SetMousePointerLock(true);
            return;
        }

        if (lockWhenClicked) {
            // Default behaviour when no menus or anything. Catch mouse on click, release on escape.
            if (UnityEngine.Input.GetMouseButtonUp(0) && !GetMousePointerLock())
                SetMousePointerLock(true);

            if (UnityEngine.Input.GetKeyUp(KeyCode.Escape) && GetMousePointerLock())
                SetMousePointerLock(false);
        } else {
            // When menu or console open, release lock
            if (GetMousePointerLock()) {
                SetMousePointerLock(false);
            }
        }
    }

    public static bool IsHeadless { get; private set; }
    private bool _errorState;

    public static int GameLoopCount {
        get { return Instance == null ? 0 : 1; }
    }

    // Global camera handling
    List<Camera> m_CameraStack = new List<Camera>();

    private List<IGameLoop> _gameLoops = new List<IGameLoop>();
    private List<System.Type> _requestedGameLoopTypes = new List<System.Type>();
    private List<string[]> _requestedGameLoopArgs = new List<string[]>();

    static int s_bMouseLockFrameNo;
}
