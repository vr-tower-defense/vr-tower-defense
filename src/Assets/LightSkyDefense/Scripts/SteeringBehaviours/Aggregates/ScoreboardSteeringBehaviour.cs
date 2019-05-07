using UnityEngine;
using Valve.VR.InteractionSystem;

public class ScoreboardSteeringBehaviour : MonoBehaviour
{
    public Vector3 Offset = new Vector3(-.5f, .25f, 1.5f);
    public float RotationSpeed = .3f;
    public float Mass = 1;
    public float MaxSpeed = 1;

    private Renderer _renderer;
    private Transform _headsetTransform;

    private Vector3 _velocity;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
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

        var steeringForce = Seek.Calculate(transform.position, targetPosition, MaxSpeed);

        // Calculate velocity
        _velocity = Vector3.ClampMagnitude(steeringForce / Mass * Time.fixedDeltaTime, MaxSpeed);

        // Add velocity to position
        transform.position += _velocity;

        // Add continuous rotation
        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            transform.eulerAngles.y + RotationSpeed,
            transform.eulerAngles.z
        );
    }
}
