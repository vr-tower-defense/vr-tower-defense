using UnityEngine;

public class IdleRotationState : TowerState
{
    public float RotationSpeed = 3;

    private Quaternion _currentTarget;

    private void FixedUpdate()
    {
        if(transform.rotation == _currentTarget)
        {
            _currentTarget = Random.rotation;
        }

        Quaternion.RotateTowards(transform.rotation, _currentTarget, RotationSpeed);
    }
}
