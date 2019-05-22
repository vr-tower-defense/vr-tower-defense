using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


public class CycleWave : MonoBehaviour, IWaveH
{
    public MonoBehaviour[] _subWaves;

    public int Repeat = 1;
    public int Delay = 1000;

    public IEnumerator Spawn()
    {
        for (int i = 0; i < Repeat; i++)
        {
            foreach (var mono in _subWaves)
            {
                if (mono is IWaveH wave)
                {
                    yield return wave.Spawn();
                    yield return new WaitForSeconds(Delay);
                }
            }
        }
    }
}

