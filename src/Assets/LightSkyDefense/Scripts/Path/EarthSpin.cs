using UnityEngine;
using System.Collections;

public class EarthSpin : MonoBehaviour
{
    [Tooltip("Speed at which the Earth rotates")]
    public float RotationSpeed = 10f;

    void FixedUpdate()
    {
        transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime, Space.World);
    }
}