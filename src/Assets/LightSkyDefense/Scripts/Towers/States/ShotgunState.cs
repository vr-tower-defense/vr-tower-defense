using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Animations;

public class ShotgunState : TowerState
{
    [Header("Behaviour")]
    [Tooltip("speed of the shot bullets")]
    public float ProjectileSpeed = 1;

    [Tooltip("Speed varation to create a more realistic shotgun")]
    [Range(0, .3f)]
    public float ProjectileSpeedVariationRatio = .3f;

    [Tooltip("The projectile that should be instantiate")]
    public Rigidbody Projectile;

    [Tooltip("The amount of bullets to be shot by each spawn point")]
    public int Bullets = 5;

    [Tooltip("The bullet spread in degrees")]
    [Range(0, 45)]
    public int Spread;

    [Tooltip("This amount of seconds between shots")]
    public float Cooldown = 1;

    [Header("Aiming")]
    [Tooltip("Speed at which the tower rotates in degrees")]
    [Range(0, 360)]
    public float RotationSpeed = 60;

    [Tooltip("Angle in degrees in which an enemy should be from the shooting direction")]
    [Range(0, 180)]
    public float AngleTreshold = 10;

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
    /// Aim at average of all targets in range
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

            // Instantiate new projectile
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
            var speedVariation = Random.Range(-ProjectileSpeedVariationRatio, ProjectileSpeedVariationRatio);

            // Apply force to projectile in shoot direction
            projectile.AddForce(
                shootDirection * (ProjectileSpeed + speedVariation),
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
