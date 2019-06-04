using System.Collections;
using UnityEngine;

public class ShootMissileAtTarget : EnableOnTarget
{
    public FindTarget FindTarget;

    public Rigidbody Projectile;

    public Transform ProjectileSpawn;

    public float ShootSpeed = 1.25f;

    [Min(0)]
    public float ShootInterval = 4;

    [Min(0)]
    public float LoadUpTime = 3;

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
        yield return new WaitForSeconds(LoadUpTime);

        var distanceToTarget = Vector3.Distance(Target.transform.position, transform.position);

        // Switch to FindTarget when target does not exist anymore
        if (Target == null || distanceToTarget > FindTarget.Radius)
        {
            FindTarget.enabled = true;
            enabled = false;
            yield break;
        }

        var newProjectile = Instantiate(
            Projectile,
            ProjectileSpawn.position,
            ProjectileSpawn.rotation
        );

        newProjectile.SendMessage("OnEject", SendMessageOptions.RequireReceiver);
        newProjectile.velocity = ProjectileSpawn.forward * ShootSpeed;

        yield return new WaitForSeconds(ShootInterval);
        yield return Shoot();
    }
}
