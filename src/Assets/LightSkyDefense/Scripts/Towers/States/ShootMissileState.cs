using System.Collections;
using UnityEngine;

public class ShootMissileState : TowerState
{
    [Header("Appearance properties")]
    public float RotationSpeed = 1;

    [Header("Shooting properties")]
    public GameObject Projectile;
    public float ShootCooldown = 2;
    public float EjectInterval = .25f;

    [Header("Detecion properties")]
    public float DetectionRadius = .5f;
    public LayerMask DetectionLayerMask = LayerMask.Enemies;

    private Quaternion _randomRotation;
    private Coroutine _coroutine;

    /// <summary>
    /// Start shooting missiles
    /// </summary>
    private void OnEnable()
    {
        Reload();

        _coroutine = StartCoroutine(ShootMissiles());
        _randomRotation = Random.rotation;
    }

    /// <summary>
    /// Stop shooting missiles
    /// </summary>
    private void OnDisable()
    {
        StopCoroutine(_coroutine);
    }

    private void FixedUpdate()
    {
        if(transform.rotation == _randomRotation)
        {
            _randomRotation = Random.rotation;
        }

        Quaternion.RotateTowards(
            transform.rotation, 
            _randomRotation, 
            RotationSpeed * Time.deltaTime
        );
    }

    /// <summary>
    /// Shoot missiles with an interval
    /// </summary>
    private IEnumerator ShootMissiles()
    {
        // Check if there are any enemies to shoot at
        if(!Physics.CheckSphere(transform.position, DetectionRadius, (int) DetectionLayerMask))
        {
            SetTowerState(Tower.IdleState);
            yield break;
        }

        for (int i = 0; i < Tower.ProjectileSpawns.Length; i++)
        {
            // First child in projectile spawn will be missile game object
            var missile = Tower.ProjectileSpawns[i].GetChild(0);
            missile.SendMessage("OnEject");

            // Continue coroutine after eject interval
            yield return new WaitForSeconds(EjectInterval);
        }

        Reload();

        yield return new WaitForSeconds(ShootCooldown);
        yield return ShootMissiles();
    }

    /// <summary>
    /// Spawns a new missile where the slot is empty
    /// </summary>
    private void Reload()
    {
        foreach(var spawn in Tower.ProjectileSpawns)
        {
            if (spawn.childCount > 0)
            {
                continue;
            }

            Instantiate(Projectile, spawn.position, spawn.rotation, spawn);
        }
    }
}
