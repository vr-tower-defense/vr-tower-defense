using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemySteeringBehaviour : MonoBehaviour
{
    public float MaxSpeed = 1;

    private Enemy _enemy;

    void Start()
    {
        _enemy = GetComponent<Enemy>();
    }

    void FixedUpdate()
    {
        var targetPosition = 
            _enemy.PathFollower.transform.position + _enemy.PathFollower.offsetTranslation;

        var steeringForce = Arrive.Calculate(
            transform.position,
            _enemy.Rigidbody.velocity,
            targetPosition,
            MaxSpeed
        );

        _enemy.Rigidbody.AddForce(steeringForce);

        var deltaRotation = Quaternion.LookRotation(
            _enemy.Rigidbody.velocity * Time.deltaTime
        );

        _enemy.Rigidbody.MoveRotation(deltaRotation);
    }
}
