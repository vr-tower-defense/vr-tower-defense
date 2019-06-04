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
                _missile.CollisionLayerMask
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
        // Create new explosion effect instance on missile position
        MonoBehaviour.Instantiate(
            _missile.ExplosionEffect,
            _missile.transform.position,
            _missile.transform.rotation
        );

        var colliders = Physics.OverlapSphere(
            _missile.transform.position,
            _missile.ExplosionRange,
            (int)_missile.CollisionLayerMask
        );

        foreach (var collider in colliders)
        {
            var damageable = collider
               .GetComponent<Damageable>();

            var rigidbody = collider
               .GetComponent<Rigidbody>();

            var enemyDistance = Vector3.Distance(
                _missile.transform.position,
                collider.transform.position
            );

            var damage = _missile.DamageCurve.Evaluate(enemyDistance);
            damageable?.UpdateHealth(-damage);

            rigidbody?.AddExplosionForce(
                _missile.ExplosionPower,
                _missile.transform.position,
                _missile.ExplosionRange
            );
        }

        MonoBehaviour.Destroy(_missile.gameObject);
    }

    private Collider FindClosestTarget(Vector3 position, float radius, LayerMask layerMask)
    {
        var colliders = Physics.OverlapSphere(position, radius, layerMask);

        Collider targetCollider = null;
        var minimalDistance = float.MaxValue;

        foreach (var collider in colliders)
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
    [Header("Explosion properties")]
    [Tooltip("The amount of damage that is applied to a target that collides with this gameObject")]
    public AnimationCurve DamageCurve;

    [Tooltip("The power that an explosion will have")]
    public float ExplosionPower = 10f;

    [Tooltip("The radius in which enemies should be to be affected")]
    public float ExplosionRange = .2f;

    [Tooltip("The effect that should play when the missile explodes")]
    public ParticleSystem ExplosionEffect;

    [Header("Behaviour properties")]
    [Tooltip("The range in which an enemy should be in")]
    public float DetectionRange = .25f;

    [Tooltip("The layers that should be considered when checking for collisions")]
    public LayerMask CollisionLayerMask = (int)Layers.Enemies;

    [Tooltip("The time before a bullet is removed from the scene")]
    public float TimeAlive = 8;

    [Tooltip("The force that is applied to eject the missile from the tower")]
    public float EjectForce = 1;

    [Tooltip("The speed at which the missile will fly")]
    public float MaxSpeed = .5f;

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

    #region debugging

    /// <summary>
    /// Display the range when selected
    /// </summary>
    void OnDrawGizmosSelected()
    {
        // Draw detection range helper
        Gizmos.color = new Color(1, 1, 0, 0.1f);
        Gizmos.DrawSphere(transform.position, DetectionRange);

        // Draw explosion range helper
        Gizmos.color = new Color(1, 0, 0, 0.1f);
        Gizmos.DrawSphere(transform.position, ExplosionRange);
    }

    #endregion
}
