using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private readonly Type _defaultGameState = typeof(Waves);
    private MonoBehaviour _gameState { get; set; }

    private GameObject _wayPointPrefab;

    private Vector3[] _calculatedPathPoints;
    public Vector3[] CalculatedPathPoints
    {
        get => _calculatedPathPoints ?? (new Vector3[0]);
        set => _calculatedPathPoints = value;
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

    private static bool _initializing = false;
    public static void Initialize()
    {
        if (_instance != null || _initializing != false)
        {
            return;
        }

        _initializing = true;

        var instance = FindObjectOfType<GameManager>();
        if (instance == null)
        {
            //todo if player doesn't exist, create new gameobject Player?
            instance = GameObject.Find("Player").AddComponent<GameManager>();
        }

        instance._wayPointPrefab = Resources.Load("Prefabs/PathWayPoint") as GameObject;

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
    /// Spawn road waypoints
    /// </summary>
    /// <param name="pathPoints"></param>
    /// <param name="parent"></param>
    public void SetPathPoints(Vector3[] pathPoints, GameObject parent)
    {
        CalculatedPathPoints = pathPoints;

        for (int i = 0; i < CalculatedPathPoints.Length; i++)
        {
            var pathPoint = CalculatedPathPoints[i];

            var point = Instantiate(Instance._wayPointPrefab);
            point.transform.parent = parent.transform;
            point.transform.position = pathPoint;

            // TODO Find solution to avoid usage of strings when finding nodes during enemy steering.
            point.name = "Way" + (i++);
        }
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

    /// <summary>
    ///  
    /// </summary>
    public void LastEnemiesTrigger()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            enemy.AddComponent<EnemyDestroyDispatcher>();
        }
    }

    public void CheckAllEnemiesDestroyed()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");   
        if (enemies.Length == 0)
        {
            Debug.Log("End");
        }
    }
}
