using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        foreach (var state in Enemy.EnemyStates)
        {
            state.enabled = false;
        }

        enemyState.enabled = true;

        Enemy.CurrentState = enemyState;
    }
}
