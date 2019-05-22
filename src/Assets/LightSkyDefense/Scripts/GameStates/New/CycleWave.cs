﻿using System.Collections;
using UnityEngine;

public class CycleWave : WaveH
{
    public WaveH[] SubWaves;

    public int Delay = 5;
    public int Repeat = 1;

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
