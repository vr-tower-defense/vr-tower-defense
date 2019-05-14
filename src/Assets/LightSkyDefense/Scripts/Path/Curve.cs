using UnityEngine;

[System.Serializable]
public class Curve
{
    public Vector3 Start;
    public Vector3 End;
    public Vector3 StartTangent;
    public Vector3 EndTangent;

    public Curve()
    {
        var cleanVector = new Vector3(0, 0, 0);

        Start = cleanVector;
        End = cleanVector;
        StartTangent = cleanVector;
        EndTangent = cleanVector;
    }

    public Curve(Vector3 start, Vector3 end, Vector3 startTangent, Vector3 endTangent)
    {
        Start = start;
        End = end;
        StartTangent = startTangent;
        EndTangent = endTangent;
    }
}
