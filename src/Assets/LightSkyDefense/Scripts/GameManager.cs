using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using Valve.VR.InteractionSystem;

public class GameManager : MonoBehaviour, IOnGameLossTarget
{
    private static bool _initializing = false;

    private readonly Type _defaultGameState = typeof(Waves);
    private MonoBehaviour _gameState { get; set; }

    [HideInInspector]
    public GameObject WayPointPrefab;

    private Path _path;
    public Path Path
    {
        get => _path ?? (_path = FindObjectOfType<Path>());
    }

    public string GameOverText = "Wasted!";
    public float FontQuality = 250;
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

        instance.WayPointPrefab = Resources.Load("Prefabs/PathWayPoint") as GameObject;

        _instance = instance;
        _initializing = false;
    }

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    private void Start()
    {
        SetGameState(_defaultGameState);
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
        _gameState = (MonoBehaviour)gameObject.AddComponent(gameState);
    }


    public void OnGameLoss()
    {
        var camera = Camera.main;

        if(camera == null)
            return;

        var gameLossDisplayObject = new GameObject();
        gameLossDisplayObject.name = "Game Over screen";
        var mesh = gameLossDisplayObject.AddComponent<TextMesh>();
        mesh.text = GameOverText;
        mesh.fontSize = FontQuality;
        mesh.color = Color.red;
        
        gameLossDisplayObject.transform.parent = camera.transform;
        gameLossDisplayObject.transform.rotation = new Quaternion(0,0,0,0);
        gameLossDisplayObject.transform.localScale = new Vector3(10f/ FontQuality, 10f / FontQuality);
        gameLossDisplayObject.transform.localPosition = new Vector3(-2,0.5f,2f);
        

        var greyScale = camera.gameObject.GetComponent<GreyscaleAfterEffect>();

        if(greyScale == null)
            return;

        greyScale.Active = true;


    }
}
