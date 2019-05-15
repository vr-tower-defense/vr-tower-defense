using UnityEngine;
using Valve.VR.InteractionSystem;

public class EnemyDestroyDispatcher : MonoBehaviour
{
    private void OnDestroy()
    {
        if (!Application.isPlaying) return;

        var gameManager = Player.instance.GetComponent<GameManager>();
        gameManager.CheckAllEnemiesDestroyed();
    }
}

