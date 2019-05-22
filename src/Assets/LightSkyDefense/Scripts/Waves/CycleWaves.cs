using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New CycleWaves", menuName = "New CycleWaves", order = 1)]
public class CycleWaves : Wave
{
    public Wave[] SubWaves;

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
