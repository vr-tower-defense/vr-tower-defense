using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class WavesState : GameState
{
    public Wave[] Waves;

    public void Start()
    {
        _gameManager = GameManager.Instance;

        StartCoroutine(CycleWaves());
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

        _gameManager.SetGameState(typeof(WavesEndState));
    }
}
