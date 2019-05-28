using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IdleState : EnemyState
{
    public float ShootRange = 0.5f;

    private List<Collider> _objectsInRange;
    private bool _running = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!_running)
        {
            StartCoroutine(CheckForTowers(ShootRange));
        }
    }

    private IEnumerator CheckForTowers(float radius)
    {
        _running = true;
        while (_running)
        {
            _objectsInRange = Physics.OverlapSphere(transform.position, radius).ToList();

            for (var i = 0; i < _objectsInRange.Count; i++)
            {
                if (_objectsInRange[i].GetComponent<BaseTower>() != null &&
                    !Enemy.TowersInRange.Contains(_objectsInRange[i]))
                    Enemy.TowersInRange.Add(_objectsInRange[i]);
            }

            for (var j = 0; j < Enemy.TowersInRange.Count; j++)
            {
                if (!_objectsInRange.Contains(Enemy.TowersInRange[j]))
                    Enemy.TowersInRange.Remove(Enemy.TowersInRange[j]);
            }

            if (Enemy.CurrentState != null)
            {
                if (Enemy.TowersInRange.Count >= 1 && Enemy.CurrentState == Enemy.IdleState)
                {
                    Enemy.CurrentState.SetEnemyState(Enemy.ShootState);
                }

                if (Enemy.TowersInRange.Count < 1 && Enemy.CurrentState == Enemy.ShootState)
                {
                    Enemy.CurrentState.SetEnemyState(Enemy.IdleState);
                }
            }
            yield return new WaitForSeconds(1/90f);
        }
        _running = false;
    }
}
