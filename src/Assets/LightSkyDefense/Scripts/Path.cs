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
}
