using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;


public class Path : MonoBehaviour
{

    [SerializeField]
    private GameObject lineGeneratorPrefab;

    [SerializeField]
    private int lineDivision = 100;

    public Curve[] curves;
   
    void Start()
    {
        Vector3[] pathVectors = GetVector3sCordinatesFromPath(lineDivision);
        DrawPath(pathVectors);
    }

    /// <summary>
    /// Add a curve to the Path
    /// </summary>
    public void AddCurve()
    {
        Curve newCurve;

        if (curves.Length == 0)
            newCurve = new Curve();
        else
        {
            // Mirror newCurve to Previous endPoint
            Vector3 newCurveOffset = new Vector3(1f, 1f, 1f);
            Vector3 endPoint = curves[curves.Length - 1].end;
            newCurve = new Curve(endPoint, (endPoint + newCurveOffset), endPoint, (endPoint + newCurveOffset));
        }

        Array.Resize(ref curves, curves.Length + 1);

        curves[curves.Length - 1] = newCurve;
    }

    public Vector3[] GetVector3sCordinatesFromPath(int cordinatesAmountPerCurve)
    {
        var newArray = new List<Vector3>();

        for (int i = 0; i < curves.Length; i++)
        {
            Vector3[] points = Handles.MakeBezierPoints(
                curves[i].start,
                curves[i].end,
                curves[i].startTangent,
                curves[i].endTangent,
                cordinatesAmountPerCurve
            );

            newArray.AddRange(points);
        }

        return newArray.ToArray();
    }

    private void DrawPath(Vector3[] pathCordinates)
    {
        GameObject newLineGen = Instantiate(lineGeneratorPrefab);
        LineRenderer renderer = newLineGen.GetComponent<LineRenderer>();

        renderer.positionCount = pathCordinates.Length;
        renderer.SetPositions(pathCordinates);

    }
}
