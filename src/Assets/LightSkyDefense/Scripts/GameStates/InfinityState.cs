using System.Collections;
using UnityEngine;

class InfinityState : GameState
{
    public GameObject Enemy;

    private void OnEnable()
    {
        StartCoroutine(SpawnEnemies());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator SpawnEnemies()
    {
        Instantiate(
            Enemy,
            Path.Instance[0],
            Quaternion.identity
        );

        yield return new WaitForSeconds(1.5f);
        yield return SpawnEnemies();
    }
}