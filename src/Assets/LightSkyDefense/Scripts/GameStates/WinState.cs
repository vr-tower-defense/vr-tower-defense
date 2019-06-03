using UnityEngine;
using Valve.VR.InteractionSystem;

class WinState : GameState
{
    public GameObject Prefab = null;

    public Vector3 Offset = new Vector3(0, 0, 1.5f);

    public void OnEnable()
    {   
        foreach (var gameObject in FindObjectsOfType<GameObject>())
        {
            gameObject.SendMessage("OnGameWin", SendMessageOptions.DontRequireReceiver);
        }

        var player = Player.instance.headCollider.transform;

        Instantiate(
            Prefab,
            player.position + (player.rotation * Offset),
            player.rotation
        );
    }
}
