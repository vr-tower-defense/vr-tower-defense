using UnityEngine;
using Valve.VR.InteractionSystem;

/// <summary>
/// Seek offset
/// </summary>
public class ScoreboardSteeringBehaviour : MonoBehaviour
{
    public Vector3 Offset = new Vector3(-.5f, .25f, 1.5f);
    public float RotationSpeed = 30f;

    private Rigidbody _rigidbody;
    private Transform _headsetTransform;


    // Start is called before the first frame update
    void Start()
    {
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

        var targetRotation = Quaternion.LookRotation(
            _headsetTransform.position - transform.position
        );

        // Make scoreboard look towards player
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            Quaternion.Euler(0, targetRotation.eulerAngles.y, 0),
            RotationSpeed
        );
    }
}