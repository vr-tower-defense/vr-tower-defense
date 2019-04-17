using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class Path : MonoBehaviour
{
    [Tooltip("List of curves that form a line")]
    public Curve[] Curves;

    [Tooltip("Smootheness of rendered line")]
    public int LineDivision = 100;

    void Start()
    {
        var pathVectors = GetVector3sCordinatesFromPath(LineDivision);
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
    public Vector3[] GetVector3sCordinatesFromPath(int coordinatesAmountPerCurve)
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
    private Vector3[] MakeBezierPoints(Vector3 start, Vector3 startTangent,Vector3 endTangent, Vector3 end, int coordinatesAmountPerCurve)
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
