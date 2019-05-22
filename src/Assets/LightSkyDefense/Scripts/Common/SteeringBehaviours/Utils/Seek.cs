using UnityEngine;

public static class Seek
{
    /// <summary>
    /// Set a velocity that will make the agent move the world target
    /// </summary>
    public static Vector3 Calculate(Vector3 position, Vector3 targetPosition)
    {
        return targetPosition - position;
    }
}
