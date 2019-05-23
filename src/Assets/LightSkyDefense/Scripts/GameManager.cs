using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR.InteractionSystem;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Boolean indicating whether the application is about to quit
    /// </summary>
    public static bool IsQuitting { get; private set; } = false;

    [HideInInspector]
    public GameObject WayPointPrefab;

    private static bool _initializing = false;
    private MonoBehaviour _gameState { get; set; }

    private Path _path;
    public Path Path
    {
        get => _path ?? (_path = FindObjectOfType<Path>());
    }

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Initialize();
            }

            return _instance;
        }
    }

    public static void Initialize()
    {
        if (_instance != null || _initializing)
        {
            return;
        }

        _initializing = true;

        //todo if player doesn't exist, create new gameobject Player?
        var instance = FindObjectOfType<GameManager>() ?? GameObject.Find("Player").AddComponent<GameManager>();

        instance.WayPointPrefab = Resources.Load<GameObject>("Prefabs/PathWayPoint");

        _instance = instance;
        _initializing = false;
    }

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    private void Start()
    {
        _gameState = FindObjectOfType<WavesState>();
        if (_gameState == null) _gameState = gameObject.AddComponent<InfinityState>();
    }

    /// <summary>
    /// Invoked when the application is about to quit. We set 
    /// the IsQuitting variable to true, to avoid missing references errors in OnDestroy methods.
    /// </summary>
    private void OnApplicationQuit()
    {
        IsQuitting = true;
    }

    /// <summary>
    /// Used to pause the game
    /// </summary>
    public void Pause()
    {
        _gameState.enabled = false;
    }

    /// <summary>
    /// Used to resume the game
    /// </summary>
    public void Resume()
    {
        _gameState.enabled = true;
    }

    /// <summary>
    /// Used to switch between game states
    /// </summary>
    /// <param name="gameState"></param>
    public void SetGameState(Type gameState)
    {
        // Remove old game state
        Destroy(_gameState);

        // Create new game state
        _gameState = (GameState)gameObject.AddComponent(gameState);
    }

    public void OnGameLose()
    {
        SetGameState(typeof(LoseState));
    }
}

