using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class WavesState : GameState
{
    public Wave[] Waves;

    public GameState WavesEndState;

    public void OnEnable()
    {
        StartCoroutine(CycleWaves());
    }

    public void OnDisable()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// Cycles through each wave
    /// </summary>
    public IEnumerator CycleWaves()
    {
        foreach (var wave in Waves)
        {
            yield return wave.Start();
        }

        SetGameState(WavesEndState);
    }
}
