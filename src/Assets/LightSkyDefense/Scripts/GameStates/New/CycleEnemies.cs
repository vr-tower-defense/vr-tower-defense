using System.Collections;
using UnityEngine;

public class CycleEnemies : WaveH
{
    public GameObject[] Enemies;

    public override IEnumerator Spawn()
    {
        for (int i = 0; i < Repeat; i++)
        {
            foreach (var prefab in Enemies)
            {
                Instantiate(prefab);
                yield return new WaitForSeconds(Delay);
            }
        }
    }
}
