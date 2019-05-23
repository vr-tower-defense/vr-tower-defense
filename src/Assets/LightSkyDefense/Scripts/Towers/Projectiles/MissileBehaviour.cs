using UnityEngine;

public interface IMissileState
{
    void FixedUpdate();

    void OnCollisionEnter(Collision collision);
}

public class Ejected : IMissileState
{
    private readonly MissileBehaviour _missile;

    private readonly Rigidbody _rigidbody;
    private Transform _target;

    public Ejected(MissileBehaviour missile)
    {
        _missile = missile;
        _rigidbody = _missile.GetComponent<Rigidbody>();

        // Remove parent transform to set the missile free
        _missile.transform.parent = null;

        // Eject missile from missile launcher
        _rigidbody.AddForce(
            _missile.transform.forward * _missile.EjectForce,
            ForceMode.Impulse
        );

        // Always destroy the missile when it doesn't explode within a certain time
        MonoBehaviour.Destroy(_missile.gameObject, _missile.TimeAlive);
    }

    /// <summary>
    /// Make the missile move towards the given target
    /// </summary>
    public void FixedUpdate()
    {
        if (_target == null)
        {
            var collider = FindClosestTarget(
                _missile.transform.position,
                _missile.DetectionRange,
                Layers.Enemies
            );

            _target = collider?.transform;
        }

        var direction = _target != null
            ? Vector3.Normalize(_target.transform.position - _missile.transform.position)
            : _rigidbody.transform.forward;

        // Make missile overshoot
        var force = direction * _missile.MaxSpeed;

        _rigidbody.AddForce(force, ForceMode.Acceleration);
        _missile.transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
    }

    /// <summary>
    /// Apply aoe damage to objects with the damageable script
    /// </summary>
    /// <param name="collision"></param>
    public void OnCollisionEnter(Collision collision)
    {
        Collider[] colliders = Physics.OverlapSphere(
            _missile.transform.position,
            _missile.AreaOfEffectRadius,
            (int) _missile.CollisionLayerMask
        );

        foreach (var collider in colliders)
        {
            var damagable = collider
               .gameObject
               .GetComponent<Damageable>();

            var enemyDistance = Vector3.Distance(
                _missile.transform.position,
                collider.transform.position
            );

            damagable?.UpdateHealth(
                // Calculate the damage that should be applied to the enemy
                -_missile.DamageCurve.Evaluate(enemyDistance)
            );
        }

        MonoBehaviour.Destroy(_missile.gameObject);
    }

    private Collider FindClosestTarget(Vector3 position, float radius, Layers layerMask = Layers.Default)
    {
        var colliders = Physics.OverlapSphere(position, radius, (int) layerMask);

        Collider targetCollider = null;
        var minimalDistance = float.MaxValue;

        foreach(var collider in colliders)
        {
            var distance = Vector3.Distance(position, collider.transform.position);

            if (distance < minimalDistance)
            {
                minimalDistance = distance;
                targetCollider = collider;
            }
        }

        return targetCollider;
    }
}

public class MissileBehaviour : MonoBehaviour
{
    [Tooltip("The amount of damage that is applied to a target that collides with this gameObject")]
    public AnimationCurve DamageCurve;

    [Tooltip("The radius in which enemies should be to be affected")]
    public float AreaOfEffectRadius = 0.1f;

    [Tooltip("The range in which an enemy should be in")]
    public float DetectionRange = .25f;
    public float MaxSpeed = .5f;

    [Tooltip("The layers that should be considered when checking for collisions")]
    public Layers CollisionLayerMask = Layers.Enemies;

    [Tooltip("The time before a bullet is removed from the scene")]
    public float TimeAlive = 8;

    [Tooltip("The force that is applied to eject the missile from the tower")]
    public float EjectForce = 1;

    //
    private IMissileState _missileState;

    /// <summary>
    /// Forwards FixedUpdate to missile state
    /// </summary>
    private void FixedUpdate()
    {
        _missileState?.FixedUpdate();
    }

    /// <summary>
    /// Forwards OnCollisionEnter to missile state
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
        _missileState?.OnCollisionEnter(collision);
    }

    /// <summary>
    /// Sets missile state to ejected
    /// </summary>
    void OnEject()
    {
        _missileState = new Ejected(this);
    }
}
