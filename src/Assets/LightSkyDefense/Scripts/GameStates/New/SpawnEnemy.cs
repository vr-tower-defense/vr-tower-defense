using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class SpawnEnemy :  WaveH
{
    public Object Prefab;

    public override IEnumerator Spawn()
    {
        for (int i = 0; i < Repeat; i++)
        {
            Instantiate(Prefab);
            yield return new WaitForSeconds(Delay);
        }
    }
}