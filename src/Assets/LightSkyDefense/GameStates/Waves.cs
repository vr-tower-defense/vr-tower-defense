using System;
using System.Collections;
using UnityEngine;

interface IWave
{
    IEnumerator Play(MonoBehaviour monoBehaviour);
}

public class Wave1 : MonoBehaviour, IWave
{
    public IEnumerator Play(MonoBehaviour monoBehaviour)
    {
        var enemyPrefab = Resources.Load("Prefabs/Enemy_1");

        // Create a new enemy every 20 seconds
        for(var i = 30; i > 0; i--)
        {
            yield return new WaitForSeconds(.25f);
            // Spawn more enemies
            Instantiate(enemyPrefab);
        }
    }
}

public class Waves : MonoBehaviour, IGameState
{
    Type[] PreconfiguredWaves { get; } = { typeof(Wave1) };

    public Waves()
    {
        Debug.Log($"GameState: {GetType()}");
    }

    public void Start()
    {
        StartCoroutine(WavesRoutine());
    }

    /// <summary>
    /// Iterates through the waves
    /// </summary>
    /// <returns></returns>
    private IEnumerator WavesRoutine()
    {
        // Create new predefined waves and iterate trough them
        foreach (var waveType in PreconfiguredWaves)
        {
            var wave = (IWave)gameObject.AddComponent(waveType);

            // We need to wait one tick to make sure that the component has initialized properly
            yield return new WaitForFixedUpdate();

            // iterate through the steps of wave
            var enumerator = wave.Play(this);
            enumerator.MoveNext();

            while (enumerator.Current != null)
            {
                yield return enumerator.Current;
                enumerator.MoveNext();
            }

            // Wait before next wave start
            yield return new WaitForFixedUpdate();

            Destroy((MonoBehaviour) wave);
        }
    }
}
