﻿using UnityEngine;

public abstract class TowerState : MonoBehaviour
{
    protected BaseTower Tower;

    /// <summary>
    /// Get BaseTower for later use
    /// </summary>
    private void Awake()
    {
        Tower = GetComponent<BaseTower>();
    }

    public void SetTowerState(TowerState towerState)
    {
        if (towerState == null)
        {
            return;
        }

        enabled = false;
        towerState.enabled = true;

        Tower.CurrentState = towerState;
    }
}