using System;
using UnityEngine;

public abstract class TowerState : MonoBehaviour
{
    public void SetTowerState(Type towerState)
    {
        if(towerState == null)
        {
            throw new NullReferenceException();
        }

        gameObject.AddComponent(towerState);
        Destroy(this);
    }
}
