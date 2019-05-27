using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    protected Enemy Enemy;

    /// <summary>
    /// Get BaseTower for later use
    /// </summary>
    private void Awake()
    {
        Enemy = GetComponent<Enemy>();
    }

    public void SetEnemyState(EnemyState enemyState)
    {
        if (enemyState == null)
        {
            return;
        }

        enabled = false;
        enemyState.enabled = true;

        Enemy.CurrentState = enemyState;
    }
}
