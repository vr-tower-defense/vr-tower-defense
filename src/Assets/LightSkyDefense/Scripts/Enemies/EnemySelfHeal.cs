using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class EnemySelfHeal : MonoBehaviour
{
    public float HealCooldown = 5;
    public float HealAmount = 0.3f;

    private const float _checkTimer = 1;

    private Damageable _damageable;

    // Start is called before the first frame update
    void Start()
    {
        _damageable = gameObject.GetComponent<Damageable>();

        HealWithInterval();
    }

    private void HealWithInterval()
    {
        if (_damageable.Health < _damageable.MaxHealth - HealAmount)
        {
            HealEnemy(HealAmount);

            Invoke("HealWithInterval", HealCooldown);
            return;
        }

        Invoke("HealWithInterval", _checkTimer);
    }

    private void HealEnemy(float healValue)
    {
        _damageable.UpdateHealth(healValue);
    }
}
