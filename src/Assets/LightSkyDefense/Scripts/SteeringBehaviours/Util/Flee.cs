using UnityEngine;

public class Flee
{
    public static Vector3 Calculate(
        Rigidbody rigidbody, 
        Vector3 targetPosition,
        float panicDistanceSqr = 1,
        float maxSpeed = 1
    )
    {
        var distance = (rigidbody.position - targetPosition).sqrMagnitude;

        // Only flee if the target is within 'panic distance'.
        if (distance > panicDistanceSqr)
        {
            return Vector3.zero;
        }

        return Vector3.Normalize(rigidbody.position - targetPosition) * maxSpeed - rigidbody.velocity;
    }
}
