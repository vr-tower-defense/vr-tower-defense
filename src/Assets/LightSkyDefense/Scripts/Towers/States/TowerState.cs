using System;
using UnityEngine;

public abstract class TowerState : MonoBehaviour
{
    public void SetTowerState(MonoBehaviour monoBehaviour)
    {
        // Enable current behaviour
        enabled = false;

        // Enable given behaviour
        monoBehaviour.enabled = true;
    }
}
