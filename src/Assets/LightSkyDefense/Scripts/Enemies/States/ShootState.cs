using System.Collections;
using UnityEngine;

public class ShootState : EnemyState
{
    public Rigidbody Projectile;
    public float ShootInterval = 6;
    public float LoadUpTime = 5;

    private Coroutine _coroutine;
    // Start is called before the first frame update
    private void OnEnable()
    {
        _coroutine = StartCoroutine(Shoot(ShootInterval));
    }

    private void OnDisable()
    {
        StopCoroutine(_coroutine);
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
        if (Enemy.TowersInRange.Count <= 0)
            return;

        var rand = Random.Range(0, Enemy.TowersInRange.Count);
        var target = Enemy.TowersInRange[rand].transform.position;

        var newProjectile = Instantiate(
            Projectile,
            transform.position,
            transform.rotation
        );

        newProjectile.velocity = target - transform.position;
    }
}
