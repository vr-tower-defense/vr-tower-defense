using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New SpawnWave", menuName = "New SpawnWave", order = 1)]
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
