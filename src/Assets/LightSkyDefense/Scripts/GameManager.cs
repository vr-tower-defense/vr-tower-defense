using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR.InteractionSystem;

public class GameManager : MonoBehaviour, IOnGameLossTarget
{
    [HideInInspector]
    public GameObject WayPointPrefab;

    public string GameOverText = "Wasted!";
    public float FontQuality = 250;
    public bool Lost = false;

    private static bool _initializing = false;
    private readonly Type _defaultGameState = typeof(Waves);
    private MonoBehaviour _gameState { get; set; }
    private int _lastWaveEnemiesAmount = 0;

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
        if(Lost)
            return;
        Lost = true;
        var camera = Camera.main;

        if (camera == null)
            return;



        var gameLossDisplayObject = new GameObject();
        gameLossDisplayObject.name = "Game Over screen";

        gameLossDisplayObject.transform.position = Player.instance.headCollider.transform.position +
                                                   (Player.instance.headCollider.transform.rotation *
                                                    new Vector3(0, 0, 1.5f));

        gameLossDisplayObject.transform.rotation = Player.instance.headCollider.transform.rotation ;

        var mesh = gameLossDisplayObject.AddComponent<TextMesh>();
        mesh.text = GameOverText;
        mesh.fontSize = Mathf.FloorToInt(FontQuality);
        mesh.color = Color.red;
        mesh.transform.localScale = new Vector3(10f / FontQuality, 10f / FontQuality);



        var greyScale = camera.gameObject.GetComponent<GreyscaleAfterEffect>();

        if (greyScale == null)
            return;

        greyScale.Active = true;
    }

    /// <summary>
    /// Adds a destroy dispatcher to all enemies that are left in the game.
    /// </summary>
    public void LastEnemiesTrigger()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            _lastWaveEnemiesAmount++;
            enemy.AddComponent<EnemyDestroyDispatcher>();
        }
    }

    /// <summary>
    /// Checks if all enemies are dead, 
    /// this function only gets triggerd when a enemy dies on last Wave.
    /// </summary>
    public void CheckAllEnemiesDestroyed()
    {
        _lastWaveEnemiesAmount--;

        if (_lastWaveEnemiesAmount != 0)
            return;

        GameObject[] targets = gameObject.scene.GetRootGameObjects();
        targets.ForEach(t => ExecuteEvents.Execute<IOnGameWinTarget>(t, null, ((handler, _) => handler.OnGameWin())));
    }
}

