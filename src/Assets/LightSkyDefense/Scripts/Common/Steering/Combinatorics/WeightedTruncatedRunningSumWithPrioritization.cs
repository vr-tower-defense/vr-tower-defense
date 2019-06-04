using System;
using UnityEngine;

public static class WeightedTruncatedRunningSumWithPrioritization
{
    public static Vector3 Calculate(Tuple<Vector3, float>[] steeringForces, float maxSpeed)
    {
        var accumulatedForce = Vector3.zero;
        var remainingMagnitude = maxSpeed;

        foreach (var weightedSteeringForce in steeringForces)
        {
            var steeringForce = weightedSteeringForce.Item1 * weightedSteeringForce.Item2;

            // Calculate how much steering force has been used so far
            var magnitudeSoFar = accumulatedForce.magnitude;

            // Calculate how much steering force remains to be used
            remainingMagnitude -= magnitudeSoFar;

            // Return the force if there is no more force left to add
            if (remainingMagnitude <= 0)
            {
                return accumulatedForce;
            };

            // Calculate the magnitude of the force we want to add
            var magnitudeToAdd = steeringForce.magnitude;

            // If the magnitude of the sum of ForceToAdd and the running total
            // does not exceed the maximum force available to this vehicle, just
            // add together. Otherwise add as much of the ForceToAdd vector is
            // possible without going over the max.
            accumulatedForce += magnitudeToAdd < remainingMagnitude
                ? steeringForce
                : Vector3.Normalize(steeringForce) * remainingMagnitude;
        }

        return accumulatedForce;
    }
}
