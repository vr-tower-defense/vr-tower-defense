using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(BoxCollider))]
public class ShotgunBehaviour : MonoBehaviour, IOnGameLossTarget, IOnGameWinTarget
{

    [Tooltip("The amount of bullets to be shot by each spawn point")]
    public int Bullets = 1;

    [Tooltip("The bullet spread in degrees")]
    [Range(0, 90)]
    public int Spread;

    [Tooltip("This is the total damage this tower would do if every bullet hit an enemy")]
    public int Damage = 1;

    [Tooltip("This is the total amount of seconds between shots")]
    public float ShootInterval = 3;

    public Rigidbody Projectile;

    public float RotationSpeed = 1;

    public float ProjectileSpeed = 10;

    private Transform[] _spawnpoints;
    private Vector3 _center;

    private Rigidbody _activeTarget;

    private HashSet<Collider> _enemySet;

    private IEnumerator _coroutine;

    // Start is called before the first frame update
    void Start()
    {
        _enemySet = new HashSet<Collider>();

        _spawnpoints = gameObject.transform.GetComponentsInChildren<Transform>()
            .Where(t => t.gameObject.name == "Spawnpoint")
            .ToArray();


        if (Bullets == 0 || _spawnpoints.Length == 0 || Projectile == null)
        {
            enabled = false;
            return;
        }

        Projectile.GetComponent<BulletBehaviour>().BulletDamage = (float)Damage / (float)Bullets;
        // Start new coroutine to shoot projectiles
        _coroutine = ShootWithInterval(ShootInterval);
        StartCoroutine(_coroutine);

    }
    private void FixedUpdate()
    {
        RotateTowardsEnemy();
        //ShootProjectile();
    }

    /// <summary>
    /// Called whenever a colliding gameobject enters the tower's detection radius
    /// </summary>
    private void OnTriggerEnter(Collider target)
    {
        var enemyScript = target.gameObject.GetComponent<Enemy>();

        // If it's not an enemy, then there's no reason to keep going
        if (enemyScript == null) return;

        _enemySet.Add(target);
    }

    /// <summary>
    /// Called when a colliding gameobject leaves the tower's detection radius
    /// </summary>
    private void OnTriggerExit(Collider target)
    {
        var enemyScript = target.gameObject.GetComponent<Enemy>();

        // If it's not an enemy, then there's no reason to keep going
        if (enemyScript == null) return;

        _enemySet.Remove(target);
    }

    private IEnumerator ShootWithInterval(float waitTime)
    {
        while (true)
        {
            ShootProjectile();
            yield return new WaitForSeconds(waitTime);
        }
    }

    /// <summary>
    /// If there's no target, don't shoot, else, aim and shoot at the target (and play the sound effect).
    /// </summary>
    private void ShootProjectile()
    {
        if (_activeTarget == null) return;

        int spawnpoint = 0;
        for (int i = 0; i < Bullets; i++)
        {

            var newProjectile = Instantiate(
                Projectile,
                _spawnpoints[spawnpoint].position,
                transform.rotation
            );
            newProjectile.velocity = transform.forward * ProjectileSpeed;

            spawnpoint++;
            if (spawnpoint >= _spawnpoints.Length) spawnpoint = 0;
        }
    }


    /// <summary>
    /// Finds first target in list
    /// </summary>
    /// <returns></returns>
    private bool FindTarget(out Rigidbody targetTransform)
    {
        // Checks for the enemy that came in closest after his last Target.
        foreach (var targetCollider in _enemySet)
        {
            if (targetCollider == null) continue;

            targetTransform = targetCollider.GetComponent<Rigidbody>();
            return true;
        }

        targetTransform = null;
        return false;
    }

    /// <summary>
    /// Used to rotate towards an enemy on the Y-axis before shooting.
    /// </summary>
    private void RotateTowardsEnemy()
    {

        var hasTarget = FindTarget(out var targetTransform);

        // Update new active target
        _activeTarget = targetTransform;

        // Find first target in list
        if (!hasTarget) return;

        var targetDistance = Vector3.Distance(transform.position, _activeTarget.position);
        var travelTime = targetDistance / ProjectileSpeed;

        var targetDisplacement = _activeTarget.velocity * travelTime;

        var predictedlookRotation = Quaternion.LookRotation(
            (_activeTarget.position + targetDisplacement) - transform.position,
            Vector3.forward
        );

        // Rotate our transform a step closer to the target's.
        transform.rotation = Quaternion.RotateTowards(transform.rotation, predictedlookRotation, RotationSpeed);
        _center = _spawnpoints.Select(t => t.position).AverageBy(Vector3.zero, (v, w) => v + w, (v, count) => v / count);

    }

    public void OnGameLoss()
    {
        enabled = false;
    }

    public void OnGameWin()
    {
        enabled = false;
    }
}
