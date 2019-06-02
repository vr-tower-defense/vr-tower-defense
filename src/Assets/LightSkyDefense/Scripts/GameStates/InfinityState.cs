using System.Collections;
using UnityEngine;

class InfinityState : GameState
{
    public GameObject Enemy;

    [Range(0, .5f)]
    public float OffsetRadius = .2f;

    [Min(0)]
    public float SpawnInterval = 1.5f;

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
        yield return new WaitForSeconds(SpawnInterval);

        // Set start position to first point in path
        var startPosition = Path.Instance[5].transform.position;

        // Add random offset to startPosition
        startPosition += Random.insideUnitSphere * OffsetRadius;

        Instantiate(
            Enemy,
            startPosition,
            Path.Instance[0].transform.rotation
        );

        yield return SpawnEnemies();
    }
}