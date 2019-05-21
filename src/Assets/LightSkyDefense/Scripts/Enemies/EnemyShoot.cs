using System.Collections;
using System.Collections.Generic;
using Assets;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public Rigidbody Projectile;
    public float ShootInterval = 6;

    private bool _shootActive = false;
    private IEnumerator _coroutine;
    private readonly List<Collider> _towerSet = new List<Collider>();

    // Start is called before the first frame update
    void Start()
    {
        _coroutine = Shoot(ShootInterval);
    }

    private void OnTriggerEnter(Collider target)
    {
        var towerScript = target.gameObject.GetComponent<TowerBehaviour>();

        if (towerScript == null) return;

        _towerSet.Add(target);

        if (_towerSet.Count > 0 && !_shootActive)
        {
            _shootActive = true;
            StartCoroutine(_coroutine);
        }
    }

    private void OnTriggerExit(Collider target)
    {
        var towerScript = target.gameObject.GetComponent<Enemy>();

        if (towerScript == null) return;

        _towerSet.Remove(target);

        if (_towerSet.Count == 0 && _shootActive)
        {
            _shootActive = false;
            StopCoroutine(_coroutine);
        }
    }

    private IEnumerator Shoot(float interval)
    {
        yield return new WaitForSeconds(5);
        while (true)
        {
            ShootAtTower();
            yield return new WaitForSeconds(interval);
        }
    }

    private void ShootAtTower()
    {
        var rand = Random.Range(0, _towerSet.Count);
        var tower = _towerSet[rand];
        var target = tower.transform.position;

        var newProjectile = Instantiate(
            Projectile,
            transform.position,
            transform.rotation
        );

        newProjectile.velocity = target - transform.position;
    }
}
