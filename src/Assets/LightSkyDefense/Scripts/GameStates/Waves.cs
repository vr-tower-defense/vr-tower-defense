using System;
using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;
using System.Collections.Generic;
using System.Linq;

public abstract class Wave : MonoBehaviour
{
    private static GameObject _enemyPrefab;
    public static GameObject EnemyPrefab
    {
        get
        {
            return _enemyPrefab ?? (_enemyPrefab = (GameObject)Resources.Load("Prefabs/Enemy"));
        }
    }

    protected List<GameObject> _spawnedEnemies = new List<GameObject>();

    public abstract IEnumerator Play(MonoBehaviour monoBehaviour);

    protected IEnumerator waitForLastEnemy()
    {
        bool allDead = false;

        while (!allDead)
        {
            yield return new WaitForSeconds(4);

            allDead = true;
            for (int i = 0; i < _spawnedEnemies.Count; i++)
            {
                if (_spawnedEnemies[i] != null)
                {
                    allDead = false;
                    break;
                }
            }
        }
    }
}

public class Wave1 : Wave
{
    public override IEnumerator Play(MonoBehaviour monoBehaviour)
    {
        // Extra wait in the first wave so the player has some time to look around.
        yield return new WaitForSeconds(5);

        // Spawn single enemy.
        _spawnedEnemies.Add(Instantiate(EnemyPrefab));

        // Wait until all enemies killed.
        yield return StartCoroutine(waitForLastEnemy());

        // Cooldown timeout
        yield return new WaitForSeconds(5);
    }


}

public class Wave2 : Wave
{
    public override IEnumerator Play(MonoBehaviour monoBehaviour)
    {
        for (var i = 10; i > 0; i--)
        {
            Instantiate(EnemyPrefab, GameManager.Instance.Path.PathPoints[0], Quaternion.identity);
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

            var wave = (Wave)gameObject.AddComponent(waveType);

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