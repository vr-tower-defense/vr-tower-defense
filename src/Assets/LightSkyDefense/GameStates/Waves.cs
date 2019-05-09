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
        var enemyPrefab = Resources.Load("Prefabs/Enemy");

        // Create a new enemy every 20 seconds
        for (var i = 10; i > 0; i--)
        {
            // Spawn more enemies
            Instantiate(enemyPrefab);
            yield return new WaitForSeconds(0.5f);
        }

        // Cooldown timeout
        yield return new WaitForSeconds(5);
    }
}

public class Wave2 : MonoBehaviour, IWave
{
    public IEnumerator Play(MonoBehaviour monoBehaviour)
    {
        var enemyPrefab = Resources.Load("Prefabs/Enemy");

        for (var i = 10; i > 0; i--)
        {
            Instantiate(enemyPrefab);
            yield return new WaitForSeconds(2f);
        }
    }
}

public class Waves : MonoBehaviour, IGameState
{
    Type[] PreconfiguredWaves { get; } = { typeof(Wave1), typeof(Wave2) };

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

            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }

            Destroy((MonoBehaviour)wave);
        }
    }
}
