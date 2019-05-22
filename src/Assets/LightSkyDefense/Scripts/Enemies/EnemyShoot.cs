using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public Rigidbody Projectile;
    public float ShootInterval = 6;
    public float LoadUpTime = 5;

    private bool _shootActive = false;
    private IEnumerator _coroutine;
    private List<Collider> _objectsInRange;
    private readonly List<Collider> _towersInRange = new List<Collider>();

    // Start is called before the first frame update
    private void Start()
    {
        _coroutine = Shoot(ShootInterval);
        CheckForTowers(transform.position, 0.2f);
    }

    void FixedUpdate()
    {
        CheckForTowers(transform.position, 0.2f);
    }

    private void CheckForTowers(Vector3 center, float radius)
    {
        _objectsInRange = Physics.OverlapSphere(center, radius).ToList();

        for (var i = 0; i < _objectsInRange.Count; i++)
        {
            if (_objectsInRange[i].GetComponent<BaseTower>() != null && !_towersInRange.Contains(_objectsInRange[i]))
                _towersInRange.Add(_objectsInRange[i]);
        }

        for (var j = 0; j < _towersInRange.Count; j++)
        {
            if (!_objectsInRange.Contains(_towersInRange[j]))
                _towersInRange.Remove(_towersInRange[j]);
        }

        if (_towersInRange.Count >= 1 && _shootActive) return;

        if (_towersInRange.Count >= 1)
        {
            _shootActive = true;
            StartCoroutine(_coroutine);
            return;
        }

        _shootActive = false;
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
        var rand = Random.Range(0, _towersInRange.Count);
        var tower = _towersInRange[rand];
        var target = tower.transform.position;

        var newProjectile = Instantiate(
            Projectile,
            transform.position,
            transform.rotation
        );

        newProjectile.velocity = target - transform.position;
    }
}
