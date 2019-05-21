using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelfHeal : MonoBehaviour
{
    public float HealInterval = 10;
    public float HealAmount = 30;

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

            Invoke("HealWithInterval", 10);
            return;
        }

        Invoke("HealWithInterval", 1);
    }

    private void HealEnemy(float healValue)
    {
        _damageable.UpdateHealth(healValue);
    }
}
