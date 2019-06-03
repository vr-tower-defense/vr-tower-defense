using UnityEngine;

public static class Pursuit
{
    public static Vector3 Calculate(Rigidbody rigidbody, Transform target)
    {
        var toEvader = target.position - rigidbody.position;

        var lookAheadTime = toEvader.magnitude;

        // Add turn around time
        const float coefficient = 0.5f;
        lookAheadTime += (
            Vector3.Dot(Vector3.Normalize(rigidbody.velocity), Vector3.Normalize(toEvader)) - 1)
            * -coefficient;

        // Seek to predicted position
        return Seek.Calculate(rigidbody.transform.position, target.position + rigidbody.velocity * lookAheadTime);
    }
}
