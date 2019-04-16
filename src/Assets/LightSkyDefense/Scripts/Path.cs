using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;


public class Path : MonoBehaviour
{
    public List<Curve> curves = new List<Curve>();

    void Start()
    {

    }

    public void AddCurve()
    {
        Curve newCurve = new Curve();
        curves.Add(newCurve);

        if (curves.Count == 0)
            newCurve = new Curve();

        //Mirror newCurve to Previous endPoint
        else
        {
            Vector3 endPoint = curves[curves.Count - 1].end;
            newCurve = new Curve(endPoint, endPoint, endPoint, endPoint);
        }

    }
}
