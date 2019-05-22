using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New SpawnEnemies", menuName = "New SpawnEnemies", order = 1)]
public class SpawnEnemy :  Wave
{
    public GameObject Prefab;

    public override IEnumerator Spawn()
    {
        for (int i = 0; i < Repeat; i++)
        {
            Instantiate(Prefab);
            yield return new WaitForSeconds(Delay);
        }
    }
}
