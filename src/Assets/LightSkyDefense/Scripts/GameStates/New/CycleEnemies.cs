using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using Object = UnityEngine.Object;

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

