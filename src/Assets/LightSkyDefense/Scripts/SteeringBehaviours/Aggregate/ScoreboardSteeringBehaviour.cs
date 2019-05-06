using UnityEngine;
using Valve.VR.InteractionSystem;

public class ScoreboardSteeringBehaviour : MonoBehaviour
{
    public Vector3 Offset = new Vector3(-.5f, .25f, 1.5f);
    public float RotationSpeed = .2f;

    private Transform _headsetTransform;
    private Rigidbody _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _headsetTransform = Player.instance.headCollider.transform;
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Only apply steering force when not in field of view of player
        var steeringForce = Vector3.zero;
        var targetPosition = _headsetTransform.localToWorldMatrix.MultiplyPoint(Offset);

        steeringForce += Arrive.Calculate(_rigidbody, targetPosition);
        _rigidbody.velocity = steeringForce;

        // Add continuous rotation
        _rigidbody.transform.eulerAngles = new Vector3(
            _rigidbody.transform.eulerAngles.x,
            _rigidbody.transform.eulerAngles.y + RotationSpeed,
            _rigidbody.transform.eulerAngles.z
        );
    }
}
