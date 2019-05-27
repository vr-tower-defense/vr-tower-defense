using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Damageable))]
[RequireComponent(typeof(SpawnCreditOnDie))]
public class Enemy : MonoBehaviour
{

    #region states

    [Header("States")]
    public EnemyState IdleState;

    public EnemyState ShootState;

    [Header("Setup")]
    [Tooltip("The first state that is applied to the enemy")]
    public EnemyState InitialState;

    [HideInInspector]
    public EnemyState CurrentState;

    [HideInInspector]
    public EnemyState[] EnemyStates;

    #endregion

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

    public List<Collider> TowersInRange { get; } = new List<Collider>();

    private List<Collider> _objectsInRange;

    private ParticleSystem _teleportEffectInstance = null;

    private Vector3[] _pathPoints;

    private Scoreboard _scoreboard;

    private float _energyCharge = 0;
    private float _potentialEnergy = 1f;
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
        _scoreboard = GameObject.Find("Scoreboard").GetComponent<Scoreboard>();

        Rigidbody = GetComponent<Rigidbody>();
        Rigidbody.position = GameManager.Instance.Path.PathPoints[PathFollower.PathPointIndex];

        // Save intial state reference to current state field
        CurrentState = InitialState;
        EnemyStates = GetComponents<EnemyState>();

        // Disable all states and enable the current state
        foreach (var state in GetComponents<TowerState>())
        {
            state.enabled = false;
        }

        CurrentState.enabled = true;

        CheckForTowers(transform.position, 0.2f);
    }

    private void FixedUpdate()
    {
        EnergyBehaviour();

        CheckForTowers(transform.position, 0.2f);
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

    private void CheckForTowers(Vector3 center, float radius)
    {
        _objectsInRange = Physics.OverlapSphere(center, radius).ToList();

        for (var i = 0; i < _objectsInRange.Count; i++)
        {
            if (_objectsInRange[i].GetComponent<BaseTower>() != null && !TowersInRange.Contains(_objectsInRange[i]))
                TowersInRange.Add(_objectsInRange[i]);
        }

        for (var j = 0; j < TowersInRange.Count; j++)
        {
            if (!_objectsInRange.Contains(TowersInRange[j]))
                TowersInRange.Remove(TowersInRange[j]);
        }

        if (CurrentState != null)
        {
            if (TowersInRange.Count >= 1 && CurrentState == IdleState)
            {
                CurrentState.SetEnemyState(ShootState);
            }

            if (TowersInRange.Count < 1 && CurrentState == ShootState)
            {
                CurrentState.SetEnemyState(IdleState);
            }
        }
    }

    /// <summary>
    /// This kills the enemy and starts the explosion particle system and sound effect
    /// </summary>
    public void OnDie()
    {
        // If not in the world, instantiate
        var explodeEffectInstance = Instantiate(
            ExplodeEffect,
            transform.position,
            new Quaternion()
        );

        // Play effect
        explodeEffectInstance.Play();

        // Play sound effect
        SoundUtil.PlayClipAtPointWithRandomPitch(
            ExplodeSound,
            this.gameObject.transform.position,
            0.5f,
            1.5f
        );

        // Destroy after particle (emit) duration + maximum particle lifetime
        Destroy(
            explodeEffectInstance.gameObject,
            explodeEffectInstance.main.duration + explodeEffectInstance.main.startLifetime.constantMax
        );

        _scoreboard.SendMessage("PointGain", PointValue, SendMessageOptions.RequireReceiver);
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

    /// <summary>
    /// Delete PathFollower when enemy is destroyed
    /// </summary>
    void OnDestroy()
    {
        if (GameManager.IsQuitting)
        {
            return;
        }

        Destroy(PathFollower?.gameObject);
    }
}