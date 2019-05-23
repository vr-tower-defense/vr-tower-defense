using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class WavesState : GameState
{
    public Wave[] Waves;
    private Coroutine _spawn;
    public void Start()
    {
        _spawn = StartCoroutine(Play());

    }

    public IEnumerator Play()
    {
        yield return new WaitForFixedUpdate();
        foreach (var wave in Waves)
        {
            FindObjectsOfType<GameObject>()
                .ForEach(obj => obj.BroadcastMessage(
                    "OnWaveStarted",
                    SendMessageOptions.DontRequireReceiver
                ));

            yield return wave.Spawn();
        }
        _gameManager.SetGameState(typeof(WavesEndState));
    }
}
