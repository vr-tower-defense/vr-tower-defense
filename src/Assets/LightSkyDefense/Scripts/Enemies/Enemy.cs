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

    public Credit Credit;
    public int CreditValue = 5;

    public float ChargeSpeed = 0.1f;
    public float DischargeSpeed = 0.1f;

    public PathFollower PathFollower { get; private set; }
    public Rigidbody Rigidbody { get; private set; }

    private ParticleSystem _explodeEffectInstance = null;
    private ParticleSystem _teleportEffectInstance = null;

    private Vector3[] _pathPoints;

    private float _energyCharge = 0;
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
        Rigidbody.position = GameManager.Instance.Path.PathPoints[PathFollower.PathPointIndex];
    }

    private void FixedUpdate()
    {
        EnergyBehaviour();

        RotateToVelocityDirection();
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

        // Spawn Credit
        Credit.Value = CreditValue;

        Instantiate(
            Credit,
            gameObject.transform.position,
            gameObject.transform.rotation
        );
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
        var playerStatistics = Player
            .instance
            .GetComponent<PlayerStatistics>();

        playerStatistics?.UpdateLives(-1);

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
            PathFollower.UpdatePathPointIndex(foundIndex);
        }
    }

    /// <summary>
    /// Calculates the potential energy for the enemy and applies the effects (charge, discharge, damage
    /// </summary>
    private void EnergyBehaviour()
    {
        // Calculate energy potential
        _potentialEnergy = _potentialEnergyRange - Vector3.Distance(transform.position, _pathPoints[PathFollower.PathPointIndex]);

        if (_potentialEnergy >= 0)
        {
            Charge(ChargeSpeed);
        }
        else
        {
            Discharge(DischargeSpeed);
        }

        if (_energyCharge <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void RotateToVelocityDirection()
    {
        var translationVector = PathFollower.transform.position - PathFollower.PreviousPosition;

        if (translationVector == Vector3.zero)
        {
            return;
        }

        var lookAngle = Quaternion.LookRotation(translationVector);

        Rigidbody.rotation = Quaternion.RotateTowards(
            transform.rotation,
            lookAngle, 
            _rotationSpeed
        );
    }

    void OnCollisionEnter(Collision collision)
    {
        var other = collision
            .gameObject
            .GetComponent<Damagable>();

        if (other == null) return;

        // If other is enemy ignore the rest of this function
        if (other.GetComponent<Enemy>()) return;

        other.UpdateHealth(-CollisionDamage);
        Destroy(gameObject);
    }


    /// <summary>
    /// Delete PathFollower when enemy is destroyed
    /// </summary>
    void OnDestroy()
    {
        if (GameManager.IsQuitting)
        {
            return;
        }

        // When health is <= 0 explode enemy
        if(GetComponent<Damagable>().Health <= 0)
        {
            GameObject.Find("Scoreboard")
                ?.GetComponent<Scoreboard>()
                ?.PointGain(PointValue);

            Explode();
        }

        Destroy(PathFollower?.gameObject);
    }
}