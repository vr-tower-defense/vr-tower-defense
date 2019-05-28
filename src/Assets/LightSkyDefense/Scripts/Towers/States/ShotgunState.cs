using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Animations;

public class ShotgunState : ShootBulletState
{
    [Header("Shotgun properties")]
    [Tooltip("The amount of bullets to be shot by each spawn point")]
    public int PelletCount = 5;

    [Tooltip("Speed varation to create a more realistic shotgun")]
    [Range(0, .3f)]
    public float ProjectileSpeedVariationRatio = .3f;

    [Tooltip("The bullet spread in degrees")]
    [Range(0, 45)]
    public int Spread;

    /// <summary>
    /// Shoot at the target and play the sound effect.
    /// </summary>
    protected override IEnumerator ShootProjectile()
    {
        for (int i = 0; i < PelletCount; i++)
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
        yield return ShootProjectile();
    }
}
