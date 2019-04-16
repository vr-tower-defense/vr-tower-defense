using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;


public class Path : MonoBehaviour
{
    public Curve[] curves;

    void Start()
    {

    }

    public void AddCurve()
    {
        Curve newCurve;

        if (curves.Length == 0)
            newCurve = new Curve();
        else
        {
            //Mirror newCurve to Previous endPoint
            Vector3 endPoint = curves[curves.Length - 1].end;
            newCurve = new Curve(endPoint, endPoint, endPoint, endPoint);
        }

        Array.Resize(ref curves, curves.Length + 1);

        curves[curves.Length - 1] = newCurve;
    }

    public Vector3[] GetVector3sCordinatesFromPath(int cordinatesAmountPerCurve)
    {
        Vector3[] returnPoints = new Vector3[0];

        for (int i = 0; i < curves.Length; i++)
        {
            Vector3[] points = Handles.MakeBezierPoints(
               curves[i].start,
               curves[i].end,
               curves[i].startTangent,
               curves[i].endTangent,
               cordinatesAmountPerCurve);

            var newArray = new List<Vector3>();
            newArray.AddRange(returnPoints);
            newArray.AddRange(points);

            returnPoints = newArray.ToArray();
        }

        return returnPoints;
    }
}
