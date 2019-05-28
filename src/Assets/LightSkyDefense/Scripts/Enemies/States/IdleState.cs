using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IdleState : EnemyState
{
    public float ShootRange = 0.5f;

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
            Enemy.TowersInRange = new List<Collider>(Physics.OverlapSphere(transform.position, radius, (int)Layers.Towers));
            
            if (Enemy.TowersInRange.Count >= 1 && Enemy.CurrentState == Enemy.IdleState)
            {
                Enemy.CurrentState.SetEnemyState(Enemy.ShootState);
            }

            if (Enemy.TowersInRange.Count < 1 && Enemy.CurrentState == Enemy.ShootState)
            {
                Enemy.CurrentState.SetEnemyState(Enemy.IdleState);
            }

            yield return new WaitForSeconds(1/90f);
        }
        _running = false;
    }
}
