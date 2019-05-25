using UnityEngine;

public class IdleRotationState : TowerState
{
    [Tooltip("Speed at which the tower rotates in degrees")]
    [Range(0, 360)]
    public float RotationSpeed = 30;

    private Quaternion _randomRotation;

    /// <summary>
    /// Create a new random rotation to start the idle behaviour
    /// </summary>
    private void OnEnable()
    {
        _randomRotation = Random.rotation;
    }

    /// <summary>
    /// Rotate the tower and check if any enemies are in range
    /// </summary>
    private void FixedUpdate()
    {
        if (Quaternion.Angle(transform.rotation, _randomRotation) < 1)
        {
            _randomRotation = Random.rotation;
        }

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            _randomRotation,
            RotationSpeed * Time.deltaTime
        );

        // Set active state when enemies are in range
        if (Tower.TargetsInRange.Length > 0)
        {
            SetTowerState(Tower.ActiveState);
        }
    }
}
