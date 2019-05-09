using System;
using System.Collections.Generic;
using UnityEngine;

public static class WeightedTruncatedRunningSumWithPrioritization
{
    public static Vector3 Calculate(List<Tuple<Vector3, float>> steeringForces, float maxSpeed)
    {
        var steeringForceSum = Vector3.zero;
        var remainingSteeringForce = maxSpeed;

        foreach (var keyValuePair in steeringForces)
        {
            // Return steering force when we cannot add more speed
            if (steeringForceSum.magnitude > remainingSteeringForce)
            {
                return steeringForceSum;
            }

            var steeringForce = keyValuePair.Item1 * keyValuePair.Item2;
            var steeringForceMagnitude = steeringForce.magnitude;

            remainingSteeringForce -= steeringForceMagnitude;

            // Add steering force to steeringForceSum. Adds as much as possible when addition will exceed maxSpeed.
            steeringForceSum += (steeringForceMagnitude < remainingSteeringForce)
                ? steeringForce
                : (Vector3.Normalize(steeringForce) * remainingSteeringForce);
        }

        return steeringForceSum;
    }
}
