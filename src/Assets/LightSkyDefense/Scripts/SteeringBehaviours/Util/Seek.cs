using UnityEngine;

public class Seek
{
    /// <summary>
    /// Set a velocity that will make the agent move the world target
    /// </summary>
    public static Vector3 Calculate(Vector3 position, Vector3 targetPosition, float maxSpeed = 1)
    {
        var steringForce = targetPosition - position;

        // Can't divide by zero so we just return Vector2.Zero
        if (steringForce == Vector3.zero)
            return Vector3.zero;
            
        return Vector3.Normalize(steringForce) * maxSpeed;
    }
}
