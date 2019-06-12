using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class ShootBulletState : TowerState
{
    [Header("Rotation")]
    [Tooltip("Speed at which the tower rotates in degrees")]
    [Range(0, 360)]
    public float RotationSpeed = 60;

    [Tooltip("Angle in degrees in which an enemy should be from the shooting direction")]
    [Range(0, 180)]
    public float AngleThreshold = 10;

    [Header("Shooting")]
    public Rigidbody Projectile;
    public float ProjectileSpeed = 4f;
    public float Cooldown = .25f;

    private Rigidbody _target;

    private bool _coroutineStarted;

    #region lifecycle methods

    /// <summary>
    /// Stop shooting
    /// </summary>
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// - Finds new target when not locked onto one yet
    /// - Finds new target when current target is our of range
    /// - Update tower rotation
    /// - Start/stop shooting when target enters/leaves field of view
    /// </summary>
    private void FixedUpdate()
    {
        if (
            _target == null ||
            Vector3.Distance(_target.position, transform.position) > Tower.Range
        )
        {
            _target = FindTarget();
        }

        UpdateRotation();
        EnableShootRoutineWhenTargetIsInFov();
    }

    #endregion

    /// <summary>
    /// Finds first target in list
    /// </summary>
    /// <returns></returns>
    private Rigidbody FindTarget()
    {
        // Checks for the enemy that came in closest after his last Target.
        return Tower.TargetsInRange.FirstOrDefault()?.GetComponent<Rigidbody>();
    }

    private void EnableShootRoutineWhenTargetIsInFov()
    {
        // Angle between target and center of tower
        var targetAngle = Vector3.Angle(
            _target.position - transform.position,
            transform.forward
        );

        // Only start shooting when enemy is in front of tower, stop shooting otherwise
        if (targetAngle <= AngleThreshold && !_coroutineStarted)
        {
            StartCoroutine(ShootProjectile());
            _coroutineStarted = true;
        }
        else if (targetAngle > AngleThreshold && _coroutineStarted)
        {
            StopAllCoroutines();
            _coroutineStarted = false;
        }
    }

    /// <summary>
    /// Used to rotate towards an enemy. This works like an offset pursuit steering behaviour,
    /// but we only use the target position for the rotation.
    /// </summary>
    private void UpdateRotation()
    {
        var targetDistance = Vector3.Distance(
            transform.position,
            _target.position
        );

        var travelTime = targetDistance / ProjectileSpeed;
        var targetDisplacement = _target.velocity * travelTime;

        var predictedlookRotation = Quaternion.LookRotation(
            (_target.position + targetDisplacement) - transform.position
        );

        // Rotate our transform a step closer to the target's.
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            predictedlookRotation,
            RotationSpeed * Time.deltaTime
        );
    }

    /// <summary>
    /// Calculate the predicted target positionIf there's no target, don't shoot, else, aim and shoot at the target (and play the sound effect).
    /// </summary>
    protected virtual IEnumerator ShootProjectile()
    {
        // Wait before we start shooting
        yield return new WaitForSeconds(Cooldown);

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
        }

        // Invoke this method recursively
        yield return ShootProjectile();
    }
}
