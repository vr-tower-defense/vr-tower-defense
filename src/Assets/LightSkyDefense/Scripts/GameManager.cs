using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject _wayPointPrefab;

    private Vector3[] _calculatedPathPoints;
    private Vector3[] CalculatedPathPoints {
        get => _calculatedPathPoints == null ? new Vector3[0] : _calculatedPathPoints;
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

        instance._wayPointPrefab = Resources.Load("PathWayPoint") as GameObject;

        _instance = instance;
        _initializing = false;
    }

    /// <summary>
    /// Spawn road waypoints
    /// </summary>
    /// <param name="pathPoints"></param>
    /// <param name="parent"></param>
    public void SetPathPoints(Vector3[] pathPoints, GameObject parent)
    {
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

}
