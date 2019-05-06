using System;
using UnityEngine;

public class Arrive
{
    public const float DecelerationSpeed = 1;
    private const float DecelerationTweaker = 0.3f;

    public static Vector3 Calculate(
        Rigidbody rigidbody, 
        Vector3 targetPosition, 
        float maxSpeed = 1
    )
    {
        var distanceToTarget = targetPosition - rigidbody.position;

        // Calculate the distance to the target
        var dist = distanceToTarget.magnitude;

        if (dist <= 0.05)
        {
            return Vector3.zero;
        }

        // Calculate the speed required to reach the target given the desired deceleration
        var speed = Mathf.Ceil(dist / (DecelerationSpeed * DecelerationTweaker));

        // make sure the velocity does not exceed the max
        speed = Mathf.Min(speed, maxSpeed);

        // From here proceed just like Seek except we don't need to normalize 
        // the ToTarget vector because we have already gone to the trouble
        // of calculating its length: dist. 
        var desiredVelocity = distanceToTarget * speed / dist;

        return desiredVelocity - rigidbody.velocity;
    }
}
