using System.Collections;
using UnityEngine;

public class BossShootState : EnemyState
{
    public Rigidbody Projectile;
    public float ShootInterval = 4;
    public float LoadUpTime = 3;

    // Start is called before the first frame update
    private void OnEnable()
    {
        StartCoroutine(Shoot(ShootInterval));
    }

    private void OnDisable()
    {
        StopCoroutine(Shoot(ShootInterval));
    }

    private IEnumerator Shoot(float interval)
    {
        yield return new WaitForSeconds(LoadUpTime);

        while (true)
        {
            ShootAtTower();
            yield return new WaitForSeconds(interval);
        }
    }

    private void ShootAtTower()
    {
        var newProjectile = Instantiate(
            Projectile,
            transform.Find("Cylinder").Find("MissileSpawnPoint").position,
            transform.rotation
        );
        newProjectile.SendMessage("OnEject");
        newProjectile.velocity = transform.forward * 1.25f;
    }
}
