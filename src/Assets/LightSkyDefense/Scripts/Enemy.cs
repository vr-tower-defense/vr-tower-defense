using Assets;
using System.Linq;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Enemy : MonoBehaviour, IOnGameLossTarget
{
    public float MaxHealth = 100f;
    public float MovementSpeed = 0.3f;
    public float EnergyCapacity = 40f;
    public float CollisionDamage = 35f;

    public ParticleSystem ExplodeEffect;
    public ParticleSystem TeleportEffect;
    public AudioClip ExplodeSound;
    public AudioClip TeleportSound;

    public Credit Credit;
    public int CreditValue;

    private ParticleSystem _explodeEffectInstance = null;
    private ParticleSystem _teleportEffectInstance = null;
    public Rigidbody Rigidbody { get; private set; }

    private float _energyCharge = 0;
    private float _health = 100f;
    private float _potentialEnergy = 1f;
    private readonly float _rotationSpeed = 2f;
    private readonly float _potentialEnergyRange = 0.8f;

    //private int _lookAheadDistance = 5; // in waypoints
    //private int _waypointIndex = 0;
    private bool _lost = true;
    public PathFollower _pathFollower { get; private set; }
    private Path.PathPoint[] _pathPoints;

    private void Awake()
    {
        _pathFollower = GetComponentInChildren<PathFollower>();
    }

    private void Start()
    {
        _energyCharge = EnergyCapacity;
        _pathPoints = GameManager.Instance.Path.PathPoints;
        Rigidbody = GetComponent<Rigidbody>();
        Rigidbody.position = GameManager.Instance.Path.PathPoints[_pathFollower.PathPointIndex].Position;
    }

    private void FixedUpdate()
    {
        //var pathPoints = GameManager.Instance.Path.PathPoints;

        // Calculate energy potential
        //_potentialEnergy = 0.8f - Vector3.Distance(transform.position, pathPoints[_waypointIndex].Position);
        _potentialEnergy = _potentialEnergyRange - Vector3.Distance(transform.position, _pathPoints[_pathFollower.PathPointIndex].Position);

        if (_potentialEnergy >= 0)
        {
            Charge(0.1f);
        }
        else
        {
            DisCharge(0.1f);

            if (_potentialEnergy < _potentialEnergyRange/2)
            {
                _lost = true;
            }
        }

        if (_energyCharge < 0)
        {
            //Damage(30);
        }

        //
        //ApplySteeringForce(pathPoints.Select(p => p.Position).ToArray());

        RotateToVelocityDirection();
    }

    //void ApplySteeringForce(Vector3[] pathPoints)
    //{
    //    if (_waypointIndex > pathPoints.Length)
    //    {
    //        return;
    //    }

    //    // Following if statement isn't needed if we spawn the enemy in the correct place
    //    if (_waypointIndex == 0)
    //    {
    //        transform.position = pathPoints[_waypointIndex + _lookAheadDistance];
    //    }

    //    if (_lost)
    //    {
    //        // Lost so go back to last known waypoint
    //        _rigidbody.AddForce(_rigidbody.mass * (MovementSpeed * 2 * (pathPoints[_waypointIndex] - transform.position).normalized));
    //        return;
    //    }

    //    // Move enemy towards path
    //    _rigidbody.AddForce(_rigidbody.mass * (MovementSpeed * _potentialEnergy * (pathPoints[_waypointIndex + _lookAheadDistance] - transform.position).normalized));

    //    // Move enemy parallel to the path
    //    _rigidbody.AddForce(_rigidbody.mass * (MovementSpeed * (pathPoints[_waypointIndex] - pathPoints[_waypointIndex - _lookAheadDistance]).normalized));
    //}

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

        // Damage player
        var playerStats = Player.instance?.gameObject?.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.Lives--;
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
        
        if (foundIndex == (_pathPoints.Length-2))
        {
            Finish();
        }

        if (foundIndex>_pathFollower.PathPointIndex)
        {
            _pathFollower.PathPointIndex = foundIndex;
        }

        _lost = false;
    }

    private void RotateToVelocityDirection()
    {
        //var velocity = gameObject.GetComponent<Rigidbody>().velocity;
        var translationVector = _pathFollower.transform.position - _pathFollower.PreviousPosition;
        if (translationVector != Vector3.zero)
        {
            var lookAngle = Quaternion.LookRotation(translationVector);
                       
            //_rigidbody.rotation = lookAngle;;
            Rigidbody.rotation = Quaternion.RotateTowards(transform.rotation, lookAngle, _rotationSpeed);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //return;
        //var towerScript = collision.gameObject.GetComponent<TowerBehaviour>();
        var components = collision.collider.gameObject.GetComponents(typeof (Component));

        //Check for components like TowerBehaviour, Enemy, DestroyBullet, etc..
        foreach (Component c in components)
        {
            if (c is TowerBehaviour towerBehaviour)
            {
                towerBehaviour.Damage(CollisionDamage);
                Explode();
            }
            else if (c is Enemy enemy)
            {
                //enemy._pathFollower.AddOffset(collision.GetContact(0).normal, 0.02f);
            }
        }

        
        {
            //towerScript.Damage(CollisionDamage);
            //Explode();
        }


    }

    public void OnGameLoss()
    {
        enabled = false;
    }
}
