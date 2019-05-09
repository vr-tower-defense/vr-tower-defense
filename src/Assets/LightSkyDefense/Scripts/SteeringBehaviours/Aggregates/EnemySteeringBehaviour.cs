using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        var targetPosition = _enemy._pathFollower.transform.position +_enemy._pathFollower.offsetTranslation;

        var steeringForce = Arrive.Calculate(transform.position, _enemy.Rigidbody.velocity, targetPosition, MaxSpeed);

        // Calculate velocity
        _enemy.Rigidbody.AddForce(Vector3.ClampMagnitude(steeringForce, MaxSpeed));
        //_enemy.Rigidbody.AddForce(Vector3.ClampMagnitude(steeringForce / _enemy.Rigidbody.mass * Time.fixedDeltaTime, MaxSpeed));
        //_enemy.Rigidbody.position += steeringForce*0.01f;

    }
}
