using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(LineRenderer))]
public class Path : MonoBehaviour
{
    public class PathPoint
    {
        // Tail
        public Vector3 Position;
        // Translation Vector
        public Vector3 DirectionVector;

        public PathPoint(Vector3 position, Vector3 directionVector)
        {
            Position = position;
            DirectionVector = directionVector;
        }
    }

    [Tooltip("List of curves that form a line")]
    public Curve[] Curves;

    [Tooltip("Smootheness of rendered line")]
    public int LineDivision = 100;

    [HideInInspector]
    public PathPoint[] PathPoints;

    void Start()
    {
        // Get the path points, convert the coordinates to world space and spawn WayPointPrefab in the world

        var pathVectors = GetVector3sCoordinatesFromPath(LineDivision);

        PathPoints = new PathPoint[pathVectors.Length];

        GameObject point = null;

        for (int i = 0; i < pathVectors.Length - 1; i++)
        {
            var pathPoint = transform.TransformPoint(pathVectors[i]);
            var nextPathPoint = transform.TransformPoint(pathVectors[i + 1]);
            point = Instantiate(GameManager.Instance.WayPointPrefab, pathPoint, Quaternion.identity, transform);
            point.name = "Way" + i;
            PathPoints[i] = new PathPoint(pathPoint, nextPathPoint - pathPoint);
        }

        point.transform.localScale *= 5;

        PathPoints[pathVectors.Length - 1] = new PathPoint(pathVectors[pathVectors.Length - 1], PathPoints[pathVectors.Length - 2].DirectionVector);

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
    private Vector3[] MakeBezierPoints(Vector3 start, Vector3 startTangent, Vector3 endTangent, Vector3 end, int coordinatesAmountPerCurve)
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
