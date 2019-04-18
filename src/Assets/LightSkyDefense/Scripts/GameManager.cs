using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public GameObject _wayPointPrefab;

    private Vector3[] _calculatedPathPoints = null;

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
        if (_instance == null && _initializing == false)
        {
            _initializing = true;

            GameManager instance = FindObjectOfType<GameManager>();
            if (instance == null)
            {
                //todo if player doesn't exist, create new gameobject Player?
                instance = GameObject.Find("Player").AddComponent<GameManager>();
            }

            instance._wayPointPrefab = Resources.Load("PathWayPoint") as GameObject;

            _instance = instance;
   
            _initializing = false;
        }
    }

    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public Vector3[] GetLevelPath()
    {
        if(_calculatedPathPoints==null)
        {
            return new Vector3[0];
        }
        else
        {
            return _calculatedPathPoints;
        }
    }

    public void SetPathPoints(Vector3[] pathPoints, GameObject parent)
    {
        _calculatedPathPoints = pathPoints;

        //Spawn road waypoints
        //Quaternion quaternion = new Quaternion();
        int i = 0;
        foreach (Vector3 pathPoint in GetLevelPath())
        {
            GameObject point = Instantiate(GameManager.Instance._wayPointPrefab);//, pathPoint, quaternion, parent.transform);
            point.transform.parent = parent.transform;
            point.transform.position = pathPoint;
            point.name = "Way" + (i++);
        }
    }

}
