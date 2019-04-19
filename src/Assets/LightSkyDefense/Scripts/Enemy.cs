using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float MaxHealth = 100f;
    public float MovementSpeed = 0.3f;
    public float EnergyCapacity = 40f;

    public ParticleSystem ExplodeEffect;
    public ParticleSystem TeleportEffect;
    public AudioClip ExplodeSound;
    public AudioClip TeleportSound;

    public Credit Credit;
    public int CreditValue;

    private ParticleSystem _explodeEffectInstance = null;
    private ParticleSystem _teleportEffectInstance = null;
    private Rigidbody _rigidbody;

    private float _energyCharge = 0;
    private float _health = 100f;
    private float _potentialEnergy = 1f;

    private int _lookAheadDistance = 5; // in waypoints
    private int _waypointIndex = 0;
    private bool _lost = true;

    void Start()
    {
        _energyCharge = EnergyCapacity;
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    void FixedUpdate()
    {
        // Calculate energy potential
        _potentialEnergy = 0.8f - Vector3.Distance(transform.position, GameManager.Instance.GetLevelPath()[_waypointIndex]);
        
        if(_potentialEnergy>=0)
        {
            Charge(0.1f);
        }
        else
        {
            DisCharge(0.1f);

            if (_potentialEnergy < 0.4f)
            {
                _lost = true;
            }
        }

        if (_energyCharge < 0)
        {
            Damage(30);
        }

        //
        ApplySteeringForce();
    }

    void ApplySteeringForce()
    {
        if (_waypointIndex >= GameManager.Instance.GetLevelPath().Length)
        {
            return;
        }

        // Following if statement isn't needed if we spawn the enemy in the correct place
        if (_waypointIndex == 0)
        {
            transform.position = GameManager.Instance.GetLevelPath()[_waypointIndex + _lookAheadDistance];
        }

        if (_lost)
        {
            // Lost so go back to last known waypoint
            _rigidbody.AddForce(_rigidbody.mass * (MovementSpeed * 2 * (GameManager.Instance.GetLevelPath()[_waypointIndex] - transform.position).normalized));
            return;
        }

        // Move enemy towards path
        _rigidbody.AddForce(_rigidbody.mass * (MovementSpeed * _potentialEnergy * (GameManager.Instance.GetLevelPath()[_waypointIndex + _lookAheadDistance] - transform.position).normalized));

        // Move enemy parallel to the path
        _rigidbody.AddForce(_rigidbody.mass * (MovementSpeed * (GameManager.Instance.GetLevelPath()[_waypointIndex] - GameManager.Instance.GetLevelPath()[_waypointIndex - _lookAheadDistance]).normalized));
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
    public void DisCharge(float amount)
    {
        _energyCharge -= amount;
    }


    /// <summary>
    /// This function kills the enemy and starts the explosion particle system and sound effect
    /// </summary>
    public void Explode()
    {
        //if not in the world, instantiate
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
        AudioSource.PlayClipAtPoint(ExplodeSound, this.gameObject.transform.position);

        // Destroy after particle (emit) duration + maximum particle lifetime
        Destroy(
            _explodeEffectInstance.gameObject,
            _explodeEffectInstance.main.duration + _explodeEffectInstance.main.startLifetime.constantMax
        );

        // Kill enemy (if Explode() called when the enemy was still alive)
        Destroy(gameObject);

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
        AudioSource.PlayClipAtPoint(TeleportSound, this.gameObject.transform.position);

        // Destroy after particle (emit) duration + maximum particle lifetime
        Destroy(
            _teleportEffectInstance.gameObject, 
            _teleportEffectInstance.main.duration + _teleportEffectInstance.main.startLifetime.constantMax
        );

        // Teleport enemy
        Destroy(gameObject);
    }

    /// <summary>
    /// When the enemy collides with a waypoint collider, update the waypoint index
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerEnter(Collider col)
    {
        if (!col.gameObject.name.StartsWith("Way"))
        {
            return;
        }

        _waypointIndex = Mathf.Clamp(
            int.Parse(col.gameObject.name.Substring(3)) + 1,
            _lookAheadDistance, 
            GameManager.Instance.GetLevelPath().Length - _lookAheadDistance - 1
        );

        _lost = false;

        if(_waypointIndex == (GameManager.Instance.GetLevelPath().Length - _lookAheadDistance - 1))
        {
            Finish();
        }
    }

}
