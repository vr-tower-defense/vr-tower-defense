using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FollowPath", menuName = "SteeringBehaviour/FollowPath")]
public class FollowPath : ISteering
{
    [Tooltip("The maximal distance from the next waypoint from the progress position")]
    public float HandoverMargin = 0.0000001f;

    [Tooltip("The speed at which the path follower moves forward (every physics update - 16ms)")]
    public float FollowSpeed = .1f;

    // The target position on the path
    private Vector3 _progress;

    // The waypoint index to which _progress moves towards
    private int _currentWaypointIndex;

    /// <summary>
    /// Rigidbody is used to progress along the path using the velocity's magnitude
    /// </summary>
    private Rigidbody _rigidbody;
    private RigidbodySteering _rigidbodySteering;
    private bool _lost;

    public override void Initialize(GameObject gameObject)
    {
        var waypoint = Path.Instance.FindClosestWaypoint(gameObject.transform.position);

        _progress = waypoint.transform.position;
        _currentWaypointIndex = waypoint.transform.GetSiblingIndex();

        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _rigidbodySteering = gameObject.GetComponent<RigidbodySteering>();
    }

    public override Vector3 Calculate(GameObject gameObject)
    {
        if (_currentWaypointIndex >= Path.Instance.WaypointCount)
        {
            gameObject.BroadcastMessage("OnReachEndOfPath", SendMessageOptions.DontRequireReceiver);
        }

        var magnitudeSquaredFromPathProgressTracker = (
            gameObject.transform.position - _progress
        ).magnitude;

        UpdateProgression();

        // Seek to path point when we're too far away from it
        return _progress - gameObject.transform.position;
    }

    /// <summary>
    /// Update point that follows the path
    /// </summary>
    private void UpdateProgression()
    {
        if (_currentWaypointIndex >= Path.Instance.WaypointCount - 1)
        {
            return;
        }

        var targetWaypoint = Path.Instance[_currentWaypointIndex];

        // Move path progression point towards next waypoint
        _progress = Vector3.MoveTowards(
            _progress,
            targetWaypoint.transform.position,
            FollowSpeed * Time.deltaTime
        );

        var distanceToTargetWaypoint = Vector3.Distance(
            _progress,
            targetWaypoint.transform.position
        );

        if (distanceToTargetWaypoint <= HandoverMargin)
        {
            _currentWaypointIndex++;
        }
    }

    #region debugging

    public override void DrawGizmos(GameObject gameObject)
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_progress, 0.02f);

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(gameObject.transform.position, _progress);
    }

    #endregion
}
