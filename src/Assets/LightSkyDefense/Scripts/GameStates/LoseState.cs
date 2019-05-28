using UnityEngine;
using Valve.VR.InteractionSystem;

class LoseState : GameState
{
    private readonly Vector3 _ofset = new Vector3(0, 0, 1.5f);

    private void Start()
    {
        var camera = Camera.main;   

        if (camera == null)
            return;

        GameObject gameOverPrefab = (GameObject)Resources.Load("Text/GameOverText", typeof(GameObject));

        var playerTransform = Player.instance.headCollider.transform;
        var gameOverScreen = Instantiate(
            gameOverPrefab, playerTransform.position + (playerTransform.rotation * _ofset),
            Quaternion.Euler(new Vector3(0, camera.gameObject.transform.rotation.y, 0))
        );

        gameOverScreen.name = "lose screen";

        var greyScale = camera.gameObject.GetComponent<GreyscaleAfterEffect>();

        if (greyScale == null)
            return;

        greyScale.Active = true;
    }
}
