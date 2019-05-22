using UnityEngine;

public class IdleRotationState : TowerState
{
    [SerializeField]
    [Tooltip("Degrees rotated per second")]
    public float RotationSpeed = 35f;

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
        if (Tower.TargetsInRange.Count > 0)
        {
            SetTowerState(Tower.ActiveState);
        }
    }
}
