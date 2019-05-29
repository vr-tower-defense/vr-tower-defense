using UnityEngine;
using Valve.VR.InteractionSystem;

class WinState : GameState
{
    private readonly Vector3 _ofset = new Vector3(0, 0, 1.5f);

    public void Start()
    {
        var camera = Camera.main;

        if (camera == null)
            return;

        GameObject gameWinPrefab = (GameObject)Resources.Load("Text/GameWinText", typeof(GameObject));
        var playerTransform = Player.instance.headCollider.transform;
        var gameOverScreen = Instantiate(
            gameWinPrefab,
            playerTransform.position + (playerTransform.rotation * _ofset),
            Quaternion.Euler(new Vector3(0, camera.gameObject.transform.rotation.y, 0))
            );
        

        gameOverScreen.name = "win screen";
    }
}
