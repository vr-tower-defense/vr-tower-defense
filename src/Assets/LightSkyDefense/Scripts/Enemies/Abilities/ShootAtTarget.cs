using System.Collections;
using UnityEngine;

public class ShootAtTarget : MonoBehaviour
{
    public FindTarget FindTarget;

    public Rigidbody Projectile;

    public float ProjectileSpeed = .5f;

    [Tooltip("The time between every shot")]
    public float Cooldown = 2;

    [HideInInspector]
    public Collider Target;

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
        while (true)
        {
            var distanceToTarget = Vector3.Distance(Target.transform.position, transform.position);

            // Switch to FindTarget when target does not exist anymore
            if (Target == null || distanceToTarget > FindTarget.Radius)
            {
                FindTarget.enabled = true;
                enabled = false;
            }

            var projectile = Instantiate(
                Projectile,
                transform.position,
                transform.rotation
            );

            var direction = Target.transform.position - transform.position;

            projectile.AddForce(
                Vector3.Normalize(direction) * ProjectileSpeed,
                ForceMode.VelocityChange
            );

            yield return new WaitForSeconds(Cooldown);
        }
    }
}
