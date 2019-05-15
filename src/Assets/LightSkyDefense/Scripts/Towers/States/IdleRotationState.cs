using UnityEngine;

public class IdleRotationState : TowerState
{
    [SerializeField]
    private float _rotationSpeed = .75f;
    private Quaternion _currentTarget;

    private void Start()
    {
        _currentTarget = Random.rotation;
    }

    private void FixedUpdate()
    {
        if(Quaternion.Angle(transform.rotation, _currentTarget) < 10)
        {
            _currentTarget = Random.rotation;
        }

        transform.rotation = Quaternion.LerpUnclamped(transform.rotation, _currentTarget, _rotationSpeed * Time.deltaTime);
    }
}
