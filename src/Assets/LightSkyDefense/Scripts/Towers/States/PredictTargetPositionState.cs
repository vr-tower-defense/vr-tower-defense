using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PredictTargetPositionState : TowerState
{
    [Header("Rotations")]
    [Tooltip("The speed at wich a projectile flies")]
    public Vector3 Upwards = Vector3.forward;
    public float RotationSpeed = 1;

    [Header("Shooting")]
    public Rigidbody Projectile;
    public Transform ProjectileSpawn;
    public float ProjectileSpeed = 4f;
    public float ShootInterval = .25f;

    public AudioSource AudioSource;
    public AudioClip ShootSound;

    private BaseTower _tower;
    private Rigidbody _activeTarget;

    private void Start()
    {
        _tower = GetComponent<BaseTower>();

        StartCoroutine(ShootWithInterval());
    }

    /// <summary>
    /// While the tower is alive and there are enemies nearby, the tower will aim, shoot and then wait for a small period of time.
    /// </summary>
    private IEnumerator ShootWithInterval()
    {
        while (true)
        {
            ShootProjectile();
            yield return new WaitForSeconds(ShootInterval);
        }
    }

    /// <summary>
    /// If there's no target, don't shoot, else, aim and shoot at the target (and play the sound effect).
    /// </summary>
    private void ShootProjectile()
    {
        if (_activeTarget == null) return;

        var newProjectile = Instantiate(
            Projectile,
            ProjectileSpawn.position,
            ProjectileSpawn.rotation
        );

        newProjectile.AddForce(transform.forward * ProjectileSpeed, ForceMode.VelocityChange);

        AudioSource?.PlayOneShotWithRandomPitch(ShootSound, 0.5f, 1.5f);
    }

    /// <summary>
    /// Finds first target in list
    /// </summary>
    /// <returns></returns>
    private bool FindTarget(out Rigidbody targetTransform)
    {
        // Checks for the enemy that came in closest after his last Target.
        foreach (var enemy in _tower.TargetsInRange)
        {
            if (enemy == null) continue;

            targetTransform = enemy.GetComponent<Rigidbody>();
            return true;
        }

        targetTransform = null;
        return false;
    }

    /// <summary>
    /// Used to rotate towards an enemy. This works like an offset pursuit steering behaviour, 
    /// but we only use the target position for the rotation.
    /// </summary>
    private void FixedUpdate()
    {
        var hasTarget = FindTarget(out _activeTarget);

        // Don't rotate when target does not exist
        if (!hasTarget) return;

        var targetDistance = Vector3.Distance(
            transform.position,
            _activeTarget.position
        );

        var travelTime = targetDistance / ProjectileSpeed;
        var targetDisplacement = _activeTarget.velocity * travelTime;

        var predictedlookRotation = Quaternion.LookRotation(
            (_activeTarget.position + targetDisplacement) - transform.position,
            Upwards
        );

        // Rotate our transform a step closer to the target's.
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            predictedlookRotation,
            RotationSpeed
        );
    }
}
