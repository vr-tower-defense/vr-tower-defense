using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeightedSteeringBehaviour
{
    public ISteering SteeringBehaviour;

    [Range(0, 10)]
    public float Weight = 1f;

    public bool DrawGizmos = true;
}

public class RigidbodySteering : MonoBehaviour
{
    public float MaxSpeed = .5f;

    public WeightedSteeringBehaviour[] WeightedSteeringBehaviours;

    private Rigidbody _rigidbody;
    private float _squaredMaxSpeed;

    private WeightedSteeringBehaviour[] _weightedSteeringBehaviours;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _squaredMaxSpeed = MaxSpeed * MaxSpeed;

        _weightedSteeringBehaviours = new WeightedSteeringBehaviour[WeightedSteeringBehaviours.Length];

        for (int i = 0; i < WeightedSteeringBehaviours.Length; i++)
        {
            var weightedSteeringBehaviour = (WeightedSteeringBehaviour)WeightedSteeringBehaviours[i];

            // We need to create new instance of the Scriptable object because it is shared between gameObjects
            _weightedSteeringBehaviours[i] = new WeightedSteeringBehaviour
            {
                SteeringBehaviour = Instantiate(weightedSteeringBehaviour.SteeringBehaviour),
                Weight = weightedSteeringBehaviour.Weight,
                DrawGizmos = weightedSteeringBehaviour.DrawGizmos
            };

            _weightedSteeringBehaviours[i].SteeringBehaviour.Initialize(gameObject);
        }
    }

    private void FixedUpdate()
    {
        var steeringForces = new Tuple<Vector3, float>[_weightedSteeringBehaviours.Length];

        for (int i = 0; i < _weightedSteeringBehaviours.Length; i++)
        {
            var weightedSteeringBehaviour = _weightedSteeringBehaviours[i];

            steeringForces[i] = new Tuple<Vector3, float>(
                weightedSteeringBehaviour.SteeringBehaviour.Calculate(gameObject),
                weightedSteeringBehaviour.Weight
            );
        }

        var steeringForce = WeightedTruncatedRunningSumWithPrioritization.Calculate(
            steeringForces,
            MaxSpeed
        );

        _rigidbody.AddForce(steeringForce);

        // No need to update rotation
        if (_rigidbody.velocity == Vector3.zero)
        {
            return;
        }

        // Rotate enemy towards velocity direction
        var deltaRotation = Quaternion.LookRotation(_rigidbody.velocity);
        _rigidbody.MoveRotation(deltaRotation);
    }

    #region debugging

    private void OnDrawGizmosSelected()
    {
        foreach (var weightedSteeringBehaviour in _weightedSteeringBehaviours)
        {
            if (!weightedSteeringBehaviour.DrawGizmos)
            {
                continue;
            }

            weightedSteeringBehaviour.SteeringBehaviour.DrawGizmos(gameObject);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + _rigidbody.velocity);
    }

    #endregion
}
