using UnityEngine;

public static class Evade
{
    public static Vector3 Calculate(
        Rigidbody rigidbody,
        Rigidbody targetRigidbody,
        float threatRange = 1,
        float maxSpeed = 1
    )
    {
        var toPursuer = targetRigidbody.position - rigidbody.position;

        // Only flee if the target is within 'panic distance'. Work in distance squared space.
        if (toPursuer.sqrMagnitude > threatRange * threatRange)
            return Vector3.zero;

        // The lookahead time is proportional to the distance between the pursuer
        // and the pursuer; and is inversely proportional to the sum of the
        // agents' velocities
        var lookAheadTime = toPursuer.magnitude / (maxSpeed + targetRigidbody.velocity.magnitude);

        var predictedPosition = (targetRigidbody.position + targetRigidbody.velocity) * lookAheadTime;

        //now flee away from predicted future position of the pursuer
        return Vector3.Normalize(rigidbody.position - predictedPosition) * maxSpeed - rigidbody.velocity;
    }
}
