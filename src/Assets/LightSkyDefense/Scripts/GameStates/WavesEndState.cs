using System.Collections;
using UnityEngine;

class WavesEndState : GameState
{
    public GameState WinState;

    [Min(0)]
    public int CheckInterval = 1;

    private void OnEnable()
    {
        StartCoroutine(CheckForEnemies());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator CheckForEnemies()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length < 1)
        {
            SetGameState(WinState);
        }

        yield return new WaitForSeconds(CheckInterval);
        yield return CheckForEnemies();
    }
}
