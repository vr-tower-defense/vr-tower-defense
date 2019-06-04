using System.Collections;
using UnityEngine;

class InfinityState : GameState
{
#pragma warning disable 0649
    public GameObject Enemy;
#pragma warning restore 0649

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
        var startTransform = Path.Instance[0].transform;

        // Add random offset to startPosition
        var startPosition = startTransform.position + (Random.insideUnitSphere * OffsetRadius);

        Instantiate(
            Enemy,
            startPosition,
            startTransform.rotation
        );

        yield return SpawnEnemies();
    }
}