using UnityEngine;
using Valve.VR.InteractionSystem;

public class EnemyDestroyDispatcher : MonoBehaviour
{
    private void OnDestroy()
    {
        if (GameManager.IsQuitting) return;

        // Emit OnResumeGame message to all game objects
        foreach (GameObject go in FindObjectsOfType<GameObject>())
        {
            go.SendMessage("CheckAllEnemiesDestroyed", SendMessageOptions.DontRequireReceiver);
        }
    }
}

