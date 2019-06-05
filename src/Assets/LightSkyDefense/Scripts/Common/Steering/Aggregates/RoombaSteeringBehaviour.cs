using UnityEngine;
using Valve.VR.InteractionSystem;

public class RoombaSteeringBehaviour : MonoBehaviour
{
    public ISteering _wander;

    public float speed = 0.009f;

    private Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 translation = _wander.Calculate(gameObject);

        _rigidbody.AddForce(translation * speed, ForceMode.Impulse);

        if (_rigidbody.velocity == Vector3.zero)
            return;

        transform.rotation = Quaternion.LookRotation(_rigidbody.velocity, Vector3.up);
        transform.rotation *= Quaternion.Euler(-90, 0, 0);
    }
}
