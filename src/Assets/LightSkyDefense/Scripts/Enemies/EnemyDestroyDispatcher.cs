using UnityEngine;
using Valve.VR.InteractionSystem;

public class EnemyDestroyDispatcher : MonoBehaviour
{
    private void OnDestroy()
    {
        if (GameManager.IsQuitting) return;

        var gameManager = Player.instance.GetComponent<GameManager>();
        gameManager.CheckAllEnemiesDestroyed();
    }
}

