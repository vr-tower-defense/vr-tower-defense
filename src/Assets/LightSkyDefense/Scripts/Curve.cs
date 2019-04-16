using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Curve
{
    public Vector3 start;
    public Vector3 end;
    public Vector3 startTangent;
    public Vector3 endTangent;

    public Curve()
    {
        Vector3 cleanVector = new Vector3(0, 0, 0);

        start = cleanVector;
        end = cleanVector;
        startTangent = cleanVector;
        endTangent = cleanVector;
    }

    public Curve(Vector3 _start, Vector3 _end, Vector3 _startTangent, Vector3 _endTangent)
    {
        start = _start;
        end = _end;
        startTangent = _startTangent;
        endTangent = _endTangent;
    }
}
