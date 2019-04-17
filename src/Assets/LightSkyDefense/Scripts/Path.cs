using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;


public class Path : MonoBehaviour
{
    public Curve[] Curves;

    [SerializeField]
    private int _lineDivision = 100;

    private LineRenderer _lineRenderer;

    void Start()
    {
        var pathVectors = GetVector3sCordinatesFromPath(_lineDivision);
        DrawPath(pathVectors);
    }

    /// <summary>
    /// Add a curve to the Path
    /// </summary>
    public void AddCurve()
    {
        Curve newCurve;

        if (Curves.Length == 0)
            newCurve = new Curve();
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
    /// <param name="cordinatesAmountPerCurve"></param>
    public Vector3[] GetVector3sCordinatesFromPath(int cordinatesAmountPerCurve)
    {
        var newArray = new List<Vector3>();

        for (int i = 0; i < Curves.Length; i++)
        {
            var points = Handles.MakeBezierPoints(
                Curves[i].Start,
                Curves[i].End,
                Curves[i].StartTangent,
                Curves[i].EndTangent,
                cordinatesAmountPerCurve
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
}
