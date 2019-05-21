using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAoEHeal : MonoBehaviour
{
    public float HealInterval = 3;
    public float HealAmount = 10;

    private readonly HashSet<Collider> _enemySet = new HashSet<Collider>();

    // Start is called before the first frame update
    void Start()
    {
        AoEHealWithInterval();
    }

    private void OnTriggerEnter(Collider target)
    {
        var enemyScript = target.gameObject.GetComponent<Enemy>();

        if (enemyScript == null) return;

        _enemySet.Add(target);
    }

    private void OnTriggerExit(Collider target)
    {
        var enemyScript = target.gameObject.GetComponent<Enemy>();

        if (enemyScript == null) return;

        _enemySet.Remove(target);
    }

    private void AoEHealWithInterval()
    {
        HealEnemies(HealAmount);
        Invoke("AoEHealWithInterval", HealInterval);
    }

    private void HealEnemies(float amount)
    {
        foreach (var enemy in _enemySet)
        {
            var damageable = enemy.gameObject.GetComponent<Damageable>();

            damageable.UpdateHealth(amount);
        }
    }
}
