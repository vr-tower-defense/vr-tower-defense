using UnityEngine;
using Valve.VR.InteractionSystem;

class WinState : GameState
{
    public void Start()
    {
        var camera = Camera.main;

        if (camera == null)
            return;

        GameObject gameWinPrefab = (GameObject)Resources.Load("Text/GameWinText", typeof(GameObject));

        var gameOverScreen = Instantiate(
            gameWinPrefab,
            Player.instance.headCollider.transform.position + (Player.instance.headCollider.transform.rotation * new Vector3(0, 0, 1.5f)),
            Player.instance.headCollider.transform.rotation
        );

        gameOverScreen.name = "game over screen";
    }
}
