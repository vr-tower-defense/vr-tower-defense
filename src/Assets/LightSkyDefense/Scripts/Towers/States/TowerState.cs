using System;
using UnityEngine;

public abstract class TowerState : MonoBehaviour
{
    private BaseTower _tower;

    public void SetTowerState(TowerState towerState)
    {
        _tower = GetComponent<BaseTower>();

        if (towerState == null)
        {
            return;
        }

        // Enable current behaviour
        enabled = false;

        // Enable given behaviour
        towerState.enabled = true;

        _tower.CurrentState = towerState;
    }
}
