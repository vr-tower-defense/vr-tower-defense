using System;
using UnityEngine;

public abstract class TowerState : MonoBehaviour
{
    private BaseTower _tower;

    private void Start()
    {
        _tower = GetComponent<BaseTower>();
    }

    public void SetTowerState(TowerState towerState)
    {
        // Enable current behaviour
        enabled = false;

        // Enable given behaviour
        towerState.enabled = true;

        _tower.CurrentState = towerState;
    }
}
