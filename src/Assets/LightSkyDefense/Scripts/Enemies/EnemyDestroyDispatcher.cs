using UnityEngine;
using Valve.VR.InteractionSystem;

public class EnemyDestroyDispatcher : MonoBehaviour
{
    private void OnDestroy()
    {
        if (GameManager.IsQuitting) return;

        // Not very clean?
        foreach (GameObject go in FindObjectsOfType<GameObject>())
        {
            go.SendMessage("CheckAllEnemiesDestroyed", SendMessageOptions.DontRequireReceiver);
        }
    }
}

