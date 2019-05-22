using UnityEngine;

class WavesEnd : GameState
{
    int _leftOverEnemies;

    private void Awake()
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
        Debug.Log(_leftOverEnemies + " : Enemies");
        _leftOverEnemies--;

        if (_leftOverEnemies != 0)
            return;

        // Emit OnResumeGame message to all game objects
        foreach (GameObject go in FindObjectsOfType<GameObject>())
        {
            go.SendMessage("OnGameWin", SendMessageOptions.DontRequireReceiver);
        }
    }

}
