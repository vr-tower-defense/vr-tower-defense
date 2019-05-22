using UnityEngine;

class WavesEndState : GameState
{
    int _leftOverEnemies;

    private void Start()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            _leftOverEnemies++;
            enemy.AddComponent<EnemyDestroyDispatcher>();
        }
    }

    public void CheckAllEnemiesDestroyed()
    {
        _leftOverEnemies--;

        if (_leftOverEnemies != 0)
            return;

        // Emit OnResumeGame message to all game objects
        foreach (GameObject go in FindObjectsOfType<GameObject>())
        {
            go.SendMessage("OnGameWin", SendMessageOptions.DontRequireReceiver);
        }

        _gameManager.SetGameState(typeof(WinState));
    }
}
