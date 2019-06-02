using System;
using UnityEngine;

public class EnemySteeringBehaviour : MonoBehaviour
{
    [Header("Movement properties")]
    [Range(0, 1)]
    public float MaxSpeed = .2f;

    [Header("Path follow properties")]
    [Range(0, 1)]
    public float HandoverDistance = .25f;

    [Header("Flocking properties")]
    public int FlockingLayerMask = (int)Layers.Enemies;

    [Range(0, 1)]
    public float CohesionRadius = .5f;

    [Range(0, 1)]
    public float SeparationMultiplier = .3f;

    [Header("Behaviour weights")]
    [Range(0, 1)]
    public float CohesionWeight = .5f;

    [Range(0, 1)]
    public float SeparationForce = .5f;

    [Range(0, 1)]
    public float AlignmentForce = .5f;

    [Range(0, 1)]
    public float PathFollowWeight = .5f;

    /// <summary>
    /// The rigidbody that any forces will be applied to
    /// </summary>
    private Rigidbody _rigidbody;

    /// <summary>
    /// Squared value of handover distance to avoid unecessary calculations
    /// </summary>
    private float _handoverDistanceSquared;

    /// <summary>
    /// The waypoint that is used for the path following at this moment
    /// </summary>
    private int _currentWaypointIndex;

    #region lifecycle methods

    void Awake()
    {
        _handoverDistanceSquared = HandoverDistance * HandoverDistance;

        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Change rotation to velocity direction
        var deltaRotation = Quaternion.LookRotation(_rigidbody.velocity);
        _rigidbody.MoveRotation(deltaRotation);
    }

    #endregion

    private void UpdatePathFollowRotation()
    {
        var closestWaypoint = Path.Instance.FindClosestWaypoint(transform.position);

        _rigidbody.MoveRotation(closestWaypoint.transform.rotation);
    }

    #region debugging

    /// <summary>
    /// Display the range when selected
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 0, 1, 0.1f);

        Gizmos.DrawSphere(transform.position, CohesionRadius);
        Gizmos.DrawSphere(transform.position, CohesionRadius * SeparationMultiplier);

        Gizmos.color = new Color(0, 1, 0);
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * HandoverDistance));
    }

    #endregion
}
