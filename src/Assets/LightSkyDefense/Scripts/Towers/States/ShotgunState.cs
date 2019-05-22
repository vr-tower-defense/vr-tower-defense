using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

public class ShotgunState : TowerState
{

    [Tooltip("The amount of bullets to be shot by each spawn point")]
    public int Bullets = 1;

    [Tooltip("The bullet spread in degrees")]
    [Range(0, 45)]
    public int Spread;

    [Tooltip("This is the total damage this tower would do if every bullet hit an enemy")]
    public int Damage = 1;

    [Tooltip("This is the total amount of seconds between shots")]
    public float ShootInterval = 1;

    [Tooltip("The shell to be shot")]
    public Rigidbody Projectile;

    [Tooltip("The up direction")]
    public Vector3 Upwards = Vector3.forward;

    [Tooltip("Speed at which the tower rotates")]
    public float RotationSpeed = 1;

    [Tooltip("speed of the shot bullets")]
    public float ProjectileSpeed = 1;

    public Transform[] Spawnpoints;

    private Vector3 _center;

    private Rigidbody _activeTarget;

    private HashSet<Collider> _enemySet;

    private IEnumerator _coroutine;

    private void Start()
    {

        if (Bullets == 0 || Spawnpoints.Length == 0 || Projectile == null)
        {
            enabled = false;
            return;
        }

        var projectile = Projectile.GetComponent<ShotgunBulletsBehaviour>();
        if (projectile != null) projectile.BulletDamage = (float)Damage / (float)Bullets;
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
        var lookRotation = Quaternion.LookRotation(
            (_activeTarget.position) - transform.position,
            Upwards
        );

        int spawnpoint = 0;
        for (int i = 0; i < Bullets; i++)
        {
            var offset = Quaternion.Euler(
                Random.Range(-Spread, Spread),
                Random.Range(-Spread, Spread),
                1 
            );

            var rotation = lookRotation * offset;

            var newProjectile = Instantiate(
                Projectile,
                Spawnpoints[spawnpoint].position,
                rotation
            );

            newProjectile.AddForce((rotation * transform.forward ) * ProjectileSpeed, ForceMode.VelocityChange);

            spawnpoint++;
            if (spawnpoint >= Spawnpoints.Length) spawnpoint = 0;
        }
    }
    
    /// <summary>
    /// Finds first target in list
    /// </summary>
    /// <returns></returns>
    private Rigidbody FindTarget()
    {
        // Checks for the enemy that came in closest after his last Target.
        foreach (var enemy in Tower.TargetsInRange)
        {
            if (enemy == null) continue;

            return enemy.GetComponent<Rigidbody>();
        }

        return null;
    }

    /// <summary>
    /// Used to rotate towards an enemy. This works like an offset pursuit steering behaviour, 
    /// but we only use the target position for the rotation.
    /// </summary>
    private void FixedUpdate()
    {
        _activeTarget = FindTarget();

        // Don't rotate when target does not exist
        if (_activeTarget == null) return;

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
