using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class SpawnWave : WaveH
{
    public WaveH Wave;

    public override IEnumerator Spawn()
    {
        for (int i = 0; i < Repeat; i++)
        {
            yield return Wave.Spawn();
            yield return new WaitForSeconds(Delay);
        }
    }
}