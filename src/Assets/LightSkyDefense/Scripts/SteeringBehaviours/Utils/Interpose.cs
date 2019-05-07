using UnityEngine;

public class Interpose
{
    public static Vector3 Calculate(
        Rigidbody rigidbody, 
        Transform transformOne, 
        Transform transformTwo, 
        float maxSpeed = 1
    )
    {
        return Arrive.Calculate(
            rigidbody.position,
            rigidbody.velocity,
            (transformOne.position + transformTwo.position) / 2,
            maxSpeed
        );
    }
}
