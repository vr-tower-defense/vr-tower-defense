using UnityEngine;
using System;

[RequireComponent(typeof(LineRenderer))]
public class Path : MonoBehaviour
{
    #region configurable properties

    [Tooltip("List of curves that form a line")]
    public Curve[] Curves;

    [Tooltip("Amount of points to generate that form the line")]
    public int WaypointCount = 100;

    [Tooltip("The object that is instantiated as a waypoint")]
    public GameObject Prefab;

    #endregion

    #region singleton

    // Reference to path
    private static Path _path;

    // Path singleton
    [HideInInspector]
    public static Path Instance => _path ?? (_path = FindObjectOfType<Path>());

    #endregion

    public GameObject EndGoalPrefab;

    [HideInInspector]
    public GameObject EndGoalInstance;

    /// <summary>
    /// List of waypoints that represent the path
    /// </summary>
    private GameObject[] _wayPoints;

    private LineRenderer _lineRenderer;

    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        GenerateNewPath();

        SpawnEndGoal();
    }

    /// <summary>
    /// Generates a new path
    /// </summary>
    private void GenerateNewPath()
    {
        CreateWaypoints();
        DrawPath();
    }

    /// <summary>
    /// Returns waypoint that is closest to given position
    /// </summary>
    /// <returns></returns>
    public GameObject FindClosestWaypoint(Vector3 position)
    {
        GameObject closestWaypoint = null;
        var shortestDistanceSquared = float.MaxValue;

        foreach (var waypoint in _wayPoints)
        {
            var distanceSquared = (position - waypoint.transform.position).sqrMagnitude;

            if (distanceSquared > shortestDistanceSquared)
            {
                continue;
            }

            closestWaypoint = waypoint;
            shortestDistanceSquared = distanceSquared;
        }

        return closestWaypoint;
    }

    /// <summary>
    /// Add a curve to the Path
    /// </summary>
    public void AddCurve()
    {
        Curve newCurve;

        if (Curves.Length == 0)
        {
            newCurve = new Curve();
        }
        else
        {
            var newCurveOffset = new Vector3(1f, 1f, 1f);

            // Mirror newCurve to Previous endPoint
            var endPoint = Curves[Curves.Length - 1].End;
            newCurve = new Curve(endPoint, (endPoint + newCurveOffset), endPoint, (endPoint + newCurveOffset));
        }

        Array.Resize(ref Curves, Curves.Length + 1);

        Curves[Curves.Length - 1] = newCurve;
    }

    /// <summary>
    /// Returns an array of waypoints representing the bezier curve.
    /// </summary>
    private void CreateWaypoints()
    {
        _wayPoints = new GameObject[WaypointCount];

        // The step size between each waypoint
        // Note: this must be a double because the float type precision isn't precise enough
        var stepSize = 1d / (WaypointCount / Curves.Length);

        // Current waypoint index
        var waypointIndex = 0;

        // Loop through each curve to create a full path
        foreach (var curve in Curves)
        {
            // Calculate points using the 'le castelle' algorithm
            // Note: this must be a double because the float type precision isn't precise enough
            for (var interval = 0d; interval <= 1.0d; interval += stepSize)
            {
                var floatInterval = (float)interval;

                var ap1 = Vector3.Lerp(curve.Start, curve.StartTangent, floatInterval);
                var ap2 = Vector3.Lerp(curve.StartTangent, curve.EndTangent, floatInterval);
                var ap3 = Vector3.Lerp(curve.EndTangent, curve.End, floatInterval);

                var bp1 = Vector3.Lerp(ap1, ap2, floatInterval);
                var bp2 = Vector3.Lerp(ap2, ap3, floatInterval);

                var localPosition = Vector3.Lerp(bp1, bp2, floatInterval);

                // Create new points from intermediate values
                _wayPoints[waypointIndex] = Instantiate(
                    Prefab,
                    transform.position + localPosition,
                    Quaternion.identity,
                    transform
                );

                // We can't set the direction to the current waypoint when we're creating the first waypoint
                if (waypointIndex == 0)
                {
                    waypointIndex++;
                    continue;
                }

                // Update rotation of previous waypoint to make it look towards the current one
                _wayPoints[waypointIndex - 1].transform.LookAt(_wayPoints[waypointIndex].transform);

                // Increment waypoint counter
                waypointIndex++;
            }
        }
    }

    /// <summary>
    /// Draws the path
    /// </summary>
    private void DrawPath()
    {
        var positions = new Vector3[_wayPoints.Length];

        for (int i = 0; i < _wayPoints.Length; i++)
        {
            positions[i] = _wayPoints[i].transform.position;
        }

        _lineRenderer.positionCount = positions.Length;
        _lineRenderer.SetPositions(positions);
    }

    private void SpawnEndGoal()
    {
        var lineEnd = _wayPoints[_wayPoints.Length - 2];

        var earthPosition =
            lineEnd.transform.position +
            lineEnd.transform.forward * 0.5f;

        EndGoalInstance = Instantiate(EndGoalPrefab, earthPosition, Quaternion.identity);
    }

    #region operator overloading

    /// <summary>
    /// Array operator overloading to path
    /// Path.Instance[0] - returns first waypoint in path
    /// </summary>
    public GameObject this[int i]
    {
        get
        {
            if (_wayPoints.Length > 0 && _wayPoints.Length > i)
            {
                return _wayPoints[i];
            }

            return null;
        }
    }

    #endregion
}