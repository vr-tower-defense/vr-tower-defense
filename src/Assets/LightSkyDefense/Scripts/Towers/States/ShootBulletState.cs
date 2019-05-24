﻿using System.Collections;
using UnityEngine;

public class ShootBulletState : TowerState
{
    [Header("Rotation")]
    [Tooltip("The speed at wich a projectile flies")]
    public float RotationSpeed = 1;

    [Tooltip("Angle in degrees in which an enemy should be from the shooting direction")]
    public float AngleTreshold = 5;

    [Header("Shooting")]
    public Rigidbody Projectile;
    public float ProjectileSpeed = 4f;
    public float Cooldown = .25f;

    private Rigidbody _activeTarget;

    private Coroutine _coroutine;

    #region lifecycle methods

    /// <summary>
    /// Start shooting missiles
    /// </summary>
    private void OnEnable()
    {
        // Make sure that the target is not empty
        _activeTarget = FindTarget();

        _coroutine = StartCoroutine(ShootProjectile());
    }

    /// <summary>
    /// Stop shooting missiles
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
        _activeTarget = FindTarget();

        var targetDistance = Vector3.Distance(
            transform.position,
            _activeTarget.position
        );

        var travelTime = targetDistance / ProjectileSpeed;
        var targetDisplacement = _activeTarget.velocity * travelTime;

        var predictedlookRotation = Quaternion.LookRotation(
            (_activeTarget.position + targetDisplacement) - transform.position
        );

        // Rotate our transform a step closer to the target's.
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            predictedlookRotation,
            RotationSpeed * Time.deltaTime
        );
    }

    #endregion

    /// <summary>
    /// If there's no target, don't shoot, else, aim and shoot at the target (and play the sound effect).
    /// </summary>
    private IEnumerator ShootProjectile()
    {
        // Check if there are any enemies to shoot at
        if (Tower.TargetsInRange.Length < 1)
        {
            SetTowerState(Tower.IdleState);
            yield break;
        }

        var targetDirection = Vector3.Angle(
            _activeTarget.position - transform.position,
            transform.forward
        );

        // Check if enemy is in front of tower
        if (targetDirection > AngleTreshold)
        {
            yield return new WaitForSeconds(Cooldown);
            yield return ShootProjectile();
        }

        foreach (var spawn in Tower.ProjectileSpawns)
        {
            var newProjectile = Instantiate(
                Projectile,
                spawn.position,
                spawn.rotation
            );

            newProjectile.AddForce(
                transform.forward * ProjectileSpeed,
                ForceMode.VelocityChange
            );

            // Wait before shooting again shot
            yield return new WaitForSeconds(Cooldown);
        }

        // Invoke this method recursively
        yield return ShootProjectile();
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
