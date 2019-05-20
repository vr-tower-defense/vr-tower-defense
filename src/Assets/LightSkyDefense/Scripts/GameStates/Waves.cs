using System;
using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

interface IWave
{
    IEnumerator Play(MonoBehaviour monoBehaviour);
}

public class Wave1 : MonoBehaviour, IWave
{
    public IEnumerator Play(MonoBehaviour monoBehaviour)
    {
        var AoeEnemy = Resources.Load("Prefabs/Enemies/AoEHealEnemy");
        var Enemy = Resources.Load("Prefabs/Enemies/Enemy");

        // Create a new enemy every 20 seconds
        for (var i = 10; i > 0; i--)
        {
            // Spawn more enemies
            if (i % 2 == 0)
            {
                Instantiate(Enemy, GameManager.Instance.Path.PathPoints[0], Quaternion.identity);
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                Instantiate(AoeEnemy, GameManager.Instance.Path.PathPoints[0], Quaternion.identity);
                yield return new WaitForSeconds(0.5f);
            }
        }

        // Cooldown timeout
        yield return new WaitForSeconds(5);
    }
}

public class Wave2 : MonoBehaviour, IWave
{
    public IEnumerator Play(MonoBehaviour monoBehaviour)
    {
        var enemyPrefab = Resources.Load("Prefabs/Enemies/SplitEnemy");

        for (var i = 10; i > 0; i--)
        {
            Instantiate(enemyPrefab, GameManager.Instance.Path.PathPoints[0], Quaternion.identity);
            yield return new WaitForSeconds(1f);
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
        int waveAmount = PreconfiguredWaves.Length;
        int waveCounter = 0;

        // Create new predefined waves and iterate trough them
        foreach (var waveType in PreconfiguredWaves)
        {
            waveCounter++;

            var wave = (IWave)gameObject.AddComponent(waveType);

            // We need to wait one tick to make sure that the component has initialized properly
            yield return new WaitForFixedUpdate();

            // Iterate through the steps of wave
            var enumerator = wave.Play(this);

            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }

            // If last wave
            if (waveAmount == waveCounter)
            {
                var gameManager = Player.instance.GetComponent<GameManager>();
                gameManager.LastEnemiesTrigger();
            }

            Destroy((MonoBehaviour)wave);
        }
    }
}
