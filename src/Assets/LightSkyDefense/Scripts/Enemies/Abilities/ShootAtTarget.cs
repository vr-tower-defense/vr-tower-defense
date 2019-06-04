using System.Collections;
using UnityEngine;

public class ShootAtTarget : EnableOnTarget
{
    public FindTarget FindTarget;

    public Rigidbody Projectile;

    public Transform ProjectileSpawn;

    public float ProjectileSpeed = .5f;

    [Tooltip("The time between every shot")]
    public float Cooldown = 2;

    // Start is called before the first frame update
    private void OnEnable()
    {
        StartCoroutine(Shoot());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator Shoot()
    {
        if (!TargetInRange())
        {
            FindTarget.enabled = true;
            enabled = false;
            yield break;
        }

        var projectile = Instantiate(
            Projectile,
            ProjectileSpawn.position,
            ProjectileSpawn.rotation
        );

        var direction = Target.transform.position - ProjectileSpawn.position;

        projectile.AddForce(
            Vector3.Normalize(direction) * ProjectileSpeed,
            ForceMode.VelocityChange
        );

        yield return new WaitForSeconds(Cooldown);
        yield return Shoot();
    }

    private bool TargetInRange()
    {
        return Target != null &&
               FindTarget.Radius > Vector3.Distance(Target.transform.position, transform.position);
    }
}
