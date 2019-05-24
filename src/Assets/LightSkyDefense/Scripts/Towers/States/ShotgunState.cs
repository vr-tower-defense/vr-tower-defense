using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Animations;

public class ShotgunState : TowerState
{

    [Tooltip("The amount of bullets to be shot by each spawn point")]
    public int Bullets = 5;

    [Tooltip("The bullet spread in degrees")]
    [Range(0, 45)]
    public int Spread;

    [Tooltip("This amount of seconds between shots")]
    public float Cooldown = 1;

    [Tooltip("Speed at which the tower rotates in degrees")]
    public float RotationSpeed = 35;

    [Tooltip("speed of the shot bullets")]
    public float ProjectileSpeed = 1;


    [Tooltip("The projectile that should be instantiate")]
    public Rigidbody Projectile;

    private Coroutine _coroutine;

    #region lifecycle methods

    /// <summary>
    /// Start shooting
    /// </summary>
    private void OnEnable()
    {
        _coroutine = StartCoroutine(ShootProjectiles());
    }

    /// <summary>
    /// Stop shooting
    /// </summary>
    private void OnDisable()
    {
        if (_coroutine == null)
        {
            return;
        }

        StopCoroutine(_coroutine);
    }

    /// <summary>
    /// Used to rotate towards an enemy. This works like an offset pursuit steering behaviour, 
    /// but we only use the target position for the rotation.
    /// </summary>
    private void FixedUpdate()
    {
        // Check if there are any enemies to shoot at
        if (Tower.TargetsInRange.Length < 1)
        {
            SetTowerState(Tower.IdleState);
            return;
        }

        Vector3 lookDirection = Vector3.zero;

        foreach (var collider in Tower.TargetsInRange)
        {
            lookDirection += collider.transform.position;
        }

        lookDirection /= Tower.TargetsInRange.Length;

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            Quaternion.LookRotation(lookDirection - transform.position),
            RotationSpeed * Time.deltaTime
        );
    }

    #endregion

    /// <summary>
    /// Shoot at the target and play the sound effect.
    /// </summary>
    private IEnumerator ShootProjectiles()
    {
        for (int i = 0; i < Bullets; i++)
        {
            var currentSpawn = Tower.ProjectileSpawns[i % Tower.ProjectileSpawns.Length];

            // Instatiate new projectile
            var projectile = Instantiate(
                Projectile,
                currentSpawn.position,
                currentSpawn.rotation
            );

            // Create random local rotation
            var localRotation = Quaternion.Euler(
                Random.Range(-Spread, Spread),
                Random.Range(-Spread, Spread),
                1
            );

            // Multiply random rotation with spawn direction to create shoot direction
            var shootDirection = localRotation * currentSpawn.forward;

            // Apply force to projectile in shoot direction
            projectile.AddForce(
                shootDirection * ProjectileSpeed,
                ForceMode.VelocityChange
            );
        }

        // Invoke this method again after given cooldown
        yield return new WaitForSeconds(Cooldown);
        yield return ShootProjectiles();
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
}
