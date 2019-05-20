using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAoEHeal : MonoBehaviour
{
    public float HealInterval = 3;
    public float HealAmount = 10;

    private IEnumerator _coroutine;
    private readonly HashSet<Collider> _enemySet = new HashSet<Collider>();

    // Start is called before the first frame update
    void Start()
    {
        _coroutine = AoEHealWithInterval(HealInterval);
        StartCoroutine(_coroutine);
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

    private IEnumerator AoEHealWithInterval(float interval)
    {
        while (true)
        {
                HealEnemies(HealAmount);
                yield return new WaitForSeconds(interval);
        }
    }

    private void HealEnemies(float healValue)
    {
        foreach (var enemy in _enemySet)
        {
            enemy.gameObject.GetComponent<Enemy>().Heal(healValue);
        }
    }
}
