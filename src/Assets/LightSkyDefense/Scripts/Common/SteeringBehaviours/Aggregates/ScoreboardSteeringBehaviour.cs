using UnityEngine;
using Valve.VR.InteractionSystem;

/// <summary>
/// Seek offset
/// </summary>
public class ScoreboardSteeringBehaviour : MonoBehaviour
{
    public Vector3 Offset = new Vector3(-.5f, .25f, 1.5f);
    public float RotationSpeed = .3f;

    private Renderer _renderer;
    private Rigidbody _rigidbody; 
    private Transform _headsetTransform;

    private Vector3 _velocity;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();

        _headsetTransform = Player.instance.headCollider.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var targetPosition = _headsetTransform.localToWorldMatrix.MultiplyPoint(Offset);

        targetPosition.y = Mathf.Clamp(
            targetPosition.y,
            _headsetTransform.position.y - .2f,
            _headsetTransform.position.y + .2f
        );

        _rigidbody.AddForce(
            Seek.Calculate(transform.position, targetPosition),
            ForceMode.Acceleration
        );

        // Add continuous rotation
        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            transform.eulerAngles.y + RotationSpeed,
            transform.eulerAngles.z
        );
    }
}
