using System;
using UnityEngine;

public static class WeightedTruncatedSum
{
    public static Vector3 Calculate(Tuple<Vector3, float>[] steeringForces, float maxSpeed)
    {
        var steeringForce = Vector3.zero;
            
        foreach (var keyValuePair in steeringForces)
        {
            steeringForce += keyValuePair.Item1 * keyValuePair.Item2;
        }

        return Vector3.ClampMagnitude(steeringForce, maxSpeed);
    }
}
