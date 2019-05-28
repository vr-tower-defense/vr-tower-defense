using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class Path : MonoBehaviour
{
    [Tooltip("List of curves that form a line")]
    public Curve[] Curves;

    [Tooltip("Object that is used as waypoints along the path")]
    public GameObject WayPoint;

    [Tooltip("Smootheness of rendered line")]
    public int LineDivision = 100;

    [Tooltip("Smootheness factor of Steering Behaviours using path")]
    public int PathPointDivider = 1;

    [HideInInspector]
    public Vector3[] _points;

    // Reference to path
    private static Path _path;

    // Path singleton
    [HideInInspector]
    public static Path Instance => _path ?? (_path = FindObjectOfType<Path>());

    /// <summary>
    /// Array operator overloading to path
    ///
    /// Path.Instance[0] <- returns first point in path
    /// </summary>
    public Vector3 this[int i]
    {
        get
        {
            if (_points.Length > 0)
            {
                return _points[i];
            }

            return Vector3.zero;
        }
    }

    /// <summary>
    /// Returns the amount of points in the path
    /// </summary>
    public int PointCount => _points.Length;

    void Start()
    {
        // Get the path points, convert the coordinates to world space and spawn WayPointPrefab in the world
        var pathVectors = GetVector3sCoordinatesFromPath(LineDivision);

        _points = new Vector3[pathVectors.Length / PathPointDivider];

        GameObject point = null;

        for (int i = 0; i < pathVectors.Length / PathPointDivider; i++)
        {
            var transformedPoint = transform.TransformPoint(pathVectors[i * PathPointDivider]);

            point = Instantiate(
                WayPoint,
                transformedPoint,
                Quaternion.identity,
                transform
            );
            point.name = "Way" + i;

            _points[i] = transformedPoint;
        }

        point.transform.localScale *= 5;

        DrawPath(pathVectors);
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
    /// Returns a Vector3 array from combined curves
    /// </summary>
    /// <param name="coordinatesAmountPerCurve"></param>
    public Vector3[] GetVector3sCoordinatesFromPath(int coordinatesAmountPerCurve)
    {
        var newArray = new List<Vector3>();

        for (int i = 0; i < Curves.Length; i++)
        {
            var points = MakeBezierPoints(
                Curves[i].Start,
                Curves[i].StartTangent,
                Curves[i].EndTangent,
                Curves[i].End,
                coordinatesAmountPerCurve
            );

            newArray.AddRange(points);
        }

        return newArray.ToArray();
    }

    /// <summary>
    /// Draws the path
    /// </summary>
    /// <param name="pathCordinates"></param>
    private void DrawPath(Vector3[] pathCordinates)
    {
        var renderer = GetComponent<LineRenderer>();

        renderer.positionCount = pathCordinates.Length;
        renderer.SetPositions(pathCordinates);
    }

    /// <summary>
    /// Retuns an array of points to representing the bezier curve.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="startTangent"></param>
    /// <param name="endTangent"></param>
    /// <param name="end"></param>
    /// <param name="coordinatesAmountPerCurve"></param>
    /// <returns></returns>
    private Vector3[] MakeBezierPoints(
        Vector3 start,
        Vector3 startTangent,
        Vector3 endTangent,
        Vector3 end,
        int coordinatesAmountPerCurve
    )
    {
        List<Vector3> curve = new List<Vector3>();

        var stepping = 1.0f / coordinatesAmountPerCurve;

        for (var x = 0.0f; x <= 1.0f; x += stepping)
        {
            var ap1 = Vector3.Lerp(start, startTangent, x);
            var ap2 = Vector3.Lerp(startTangent, endTangent, x);
            var ap3 = Vector3.Lerp(endTangent, end, x);

            var bp1 = Vector3.Lerp(ap1, ap2, x);
            var bp2 = Vector3.Lerp(ap2, ap3, x);

            curve.Add(Vector3.Lerp(bp1, bp2, x));
        }

        return curve.ToArray();
    }
}