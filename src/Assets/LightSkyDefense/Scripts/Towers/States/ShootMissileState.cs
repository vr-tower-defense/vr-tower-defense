using System.Collections;
using UnityEngine;

public class ShootMissileState : TowerState
{
    [Header("Appearance properties")]
    [Tooltip("Speed at which the tower rotates in degrees")]
    [Range(0, 360)]
    public float RotationSpeed = 60;

    [Header("Shooting properties")]
    public GameObject Projectile;
    public float Cooldown = 2;
    public float EjectInterval = .25f;

    private Coroutine _coroutine;

    /// <summary>
    /// Start shooting missiles
    /// </summary>
    private void OnEnable()
    {
        Reload();

        _coroutine = StartCoroutine(ShootMissiles());
    }

    /// <summary>
    /// Stop shooting missiles
    /// </summary>
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void FixedUpdate()
    {
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

    /// <summary>
    /// Shoot missiles with an interval
    /// </summary>
    private IEnumerator ShootMissiles()
    {
        for (int i = 0; i < Tower.ProjectileSpawns.Length; i++)
        {
            // First child in projectile spawn will be missile game object
            var missile = Tower.ProjectileSpawns[i].GetChild(0);
            missile.SendMessage("OnEject");

            // Continue coroutine after eject interval
            yield return new WaitForSeconds(EjectInterval);
        }

        // Reload and invoke this method again after given cooldown
        Reload();

        yield return new WaitForSeconds(Cooldown);
        yield return ShootMissiles();
    }

    /// <summary>
    /// Spawns a new missile where the slot is empty
    /// </summary>
    private void Reload()
    {
        foreach (var spawn in Tower.ProjectileSpawns)
        {
            if (spawn.childCount > 0)
            {
                continue;
            }

            Instantiate(Projectile, spawn.position, spawn.rotation, spawn);
        }
    }
}
