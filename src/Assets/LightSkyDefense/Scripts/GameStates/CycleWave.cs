using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New CycleWave", menuName = "New CycleWave", order = 1)]
public class CycleWave : WaveH
{
    public WaveH[] SubWaves;

    public override IEnumerator Spawn()
    {
        for (int i = 0; i < Repeat; i++)
        {
            foreach (var wave in SubWaves)
            {
                yield return wave.Spawn();
                yield return new WaitForSeconds(Delay);
            }
        }
    }
}
