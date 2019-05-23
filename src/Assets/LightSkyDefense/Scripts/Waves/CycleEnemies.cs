using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New CycleEnemies", menuName = "New CycleEnemies", order = 1)]
public class CycleEnemies : Wave
{
    public GameObject[] Enemies;

    public override IEnumerator Spawn()
    {
        for (int i = 0; i < Repeat; i++)
        {
            foreach (var prefab in Enemies)
            {
                Instantiate(prefab, GameManager.Instance.Path.PathPoints[0], Quaternion.identity);
                yield return new WaitForSeconds(Cooldown);
            }
        }
    }
}
