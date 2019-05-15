using Assets;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Enemy : MonoBehaviour
{
    public float MaxHealth = 100f;
    public float EnergyCapacity = 40f;
    public float CollisionDamage = 35f;
    public float PointValue = 10f;

    public ParticleSystem ExplodeEffect;
    public ParticleSystem TeleportEffect;

    public AudioClip ExplodeSound;
    public AudioClip TeleportSound;

    public float ChargeSpeed = 0.1f;
    public float DischargeSpeed = 0.1f;

    public PathFollower PathFollower { get; private set; }
    public Rigidbody Rigidbody { get; private set; }

    private ParticleSystem _explodeEffectInstance = null;
    private ParticleSystem _teleportEffectInstance = null;

    private Path.PathPoint[] _pathPoints;

    private float _energyCharge = 0;
    private float _health = 100f;
    private float _potentialEnergy = 1f;
    private readonly float _rotationSpeed = 2f;
    private readonly float _potentialEnergyRange = 0.8f;

    private void Awake()
    {
        PathFollower = new GameObject("PathFollower").AddComponent<PathFollower>();
        PathFollower.transform.parent = transform;
    }

    private void Start()
    {
        _energyCharge = EnergyCapacity;
        _pathPoints = GameManager.Instance.Path.PathPoints;

        Rigidbody = GetComponent<Rigidbody>();
        Rigidbody.position = GameManager.Instance.Path.PathPoints[PathFollower.PathPointIndex].Position;
    }

    private void FixedUpdate()
    {
        EnergyBehaviour();

        RotateToVelocityDirection();
    }

    public float GetHealth()
    {
        return _health;
    }

    /// <summary>
    /// This function will remove the specified dmgAmount from the enemy's health
    /// </summary>
    /// <param name="amount"></param>
    public void Damage(float amount)
    {
        _health -= amount;

        if (_health > 0)
        {
            return;
        }

        Explode();
        Destroy(gameObject);
    }

    /// <summary>
    /// This function will add the specified healtAmount to the enemy's health
    /// </summary>
    /// <param name="amount"></param>
    public void Heal(float amount)
    {
        _health = Mathf.Clamp(
            _health + amount,
            0,
            MaxHealth
        );
    }

    /// <summary>
    /// This function will add the specified chargeAmount to the enemy's charge
    /// </summary>
    /// <param name="amount"></param>
    public void Charge(float amount)
    {
        _energyCharge = Mathf.Clamp(
            _energyCharge + amount,
            0,
            EnergyCapacity
        );
    }

    /// <summary>
    /// This function will remove the specified chargeAmount from the enemy's charge
    /// </summary>
    /// <param name="amount"></param>
    public void Discharge(float amount)
    {
        _energyCharge -= amount;
    }

    /// <summary>
    /// This function kills the enemy and starts the explosion particle system and sound effect
    /// </summary>
    public void Explode()
    {
        // If not in the world, instantiate
        if (_explodeEffectInstance == null)
        {
            _explodeEffectInstance = Instantiate(
                ExplodeEffect,
                transform.position,
                new Quaternion()
            );
        }

        // Play effect
        _explodeEffectInstance.Play();

        // Play sound effect
        SoundUtil.PlayClipAtPointWithRandomPitch(ExplodeSound, this.gameObject.transform.position, 0.5f, 1.5f);

        // Destroy after particle (emit) duration + maximum particle lifetime
        Destroy(
            _explodeEffectInstance.gameObject,
            _explodeEffectInstance.main.duration + _explodeEffectInstance.main.startLifetime.constantMax
        );

        // Kill enemy (if Explode() called when the enemy was still alive)
        Destroy(gameObject);

        GameObject.Find("Scoreboard")?.GetComponent<Scoreboard>()?.PointGain(PointValue);
    }

    /// <summary>
    /// This function removes the enemy and starts the teleport particle system and sound effect
    /// </summary>
    public void Finish()
    {
        // If not in the world, instantiate
        if (_teleportEffectInstance == null)
        {
            _teleportEffectInstance = Instantiate(TeleportEffect, transform.position, new Quaternion());
        }

        // Play effect
        _teleportEffectInstance.Play();

        // Play sound effect
        SoundUtil.PlayClipAtPointWithRandomPitch(TeleportSound, this.gameObject.transform.position, 0.5f, 1.5f);

        // Destroy after particle (emit) duration + maximum particle lifetime
        Destroy(
            _teleportEffectInstance.gameObject,
            _teleportEffectInstance.main.duration + _teleportEffectInstance.main.startLifetime.constantMax
        );

        // Damage player
        var playerStatistics = Player.instance?.gameObject?.GetComponent<PlayerStatistics>();

        if (playerStatistics != null)
        {
            playerStatistics.Lives--;
        }

        // Teleport enemy
        Destroy(gameObject);
    }

    /// <summary>
    /// When the enemy collides with a waypoint collider, update the waypoint index
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerEnter(Collider collider)
    {
        if (!collider.gameObject.name.StartsWith("Way"))
        {
            return;
        }

        var foundIndex = int.Parse(collider.gameObject.name.Substring(3));

        if (foundIndex == (_pathPoints.Length - 2))
        {
            Finish();
        }

        if (foundIndex > PathFollower.PathPointIndex)
        {
            PathFollower.PathPointIndex = foundIndex;
        }
    }

    /// <summary>
    /// Calculates the potential energy for the enemy and applies the effects (charge, discharge, damage
    /// </summary>
    private void EnergyBehaviour()
    {
        // Calculate energy potential
        _potentialEnergy = _potentialEnergyRange - Vector3.Distance(transform.position, _pathPoints[PathFollower.PathPointIndex].Position);

        if (_potentialEnergy >= 0)
        {
            Charge(ChargeSpeed);
        }
        else
        {
            Discharge(DischargeSpeed);
        }

        if (_energyCharge < 0)
        {
            Explode();
        }
    }

    private void RotateToVelocityDirection()
    {
        var translationVector = PathFollower.transform.position - PathFollower.PreviousPosition;

        if (translationVector != Vector3.zero)
        {
            var lookAngle = Quaternion.LookRotation(translationVector);

            //Alternative if performance becomes an issue: _rigidbody.rotation = lookAngle;
            Rigidbody.rotation = Quaternion.RotateTowards(transform.rotation, lookAngle, _rotationSpeed);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        var towerScript = collision.gameObject.GetComponent<TowerBehaviour>();

        if (towerScript == null) return;

        towerScript.Damage(CollisionDamage);
        Explode();
    }
}
