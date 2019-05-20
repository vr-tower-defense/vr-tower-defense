using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelfHeal : MonoBehaviour
{
    public float HealInterval = 10;
    public float HealAmount = 30;
    private IEnumerator _coroutine;
    private Enemy _currentEnemy;

    // Start is called before the first frame update
    void Start()
    {
        _coroutine = HealWithInterval(HealInterval);
        StartCoroutine(_coroutine);
    }

    private IEnumerator HealWithInterval(float interval)
    {
        _currentEnemy = gameObject.GetComponent<Enemy>();
        while (true)
        {
            if (_currentEnemy.GetHealth() < _currentEnemy.MaxHealth - HealAmount)
            {
                HealEnemy(HealAmount);
                yield return new WaitForSeconds(interval);
            }
            yield return new WaitForSeconds(1);
        }
    }

    private void HealEnemy(float healValue)
    {
        _currentEnemy.Heal(healValue);
    }
}
