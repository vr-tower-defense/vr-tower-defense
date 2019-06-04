using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnStep", menuName = "Waves/SpawnStep", order = 1)]
public class SpawnStep : WaveStep
{
    public GameObject Enemy;

    [Range(0, 1)]
    public float OffsetRadius = .2f;

    public override IEnumerator Run()
    {
        // Set start position to first point in path
        var startPosition = Path.Instance[0].transform.position;

        // Add random offset to startPosition
        startPosition += Random.insideUnitSphere * OffsetRadius;

        yield return Instantiate(
            Enemy,
            startPosition,
            Path.Instance[0].transform.rotation
        );
    }
}

