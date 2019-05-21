using UnityEngine;
using Valve.VR.InteractionSystem;

public class GameOver : MonoBehaviour
{
    public GameObject GameOverPrefab;

    public void OnGameLoss()
    {
        var camera = Camera.main;

        if (camera == null)
            return;

        var gameOverScreen = Instantiate(
            GameOverPrefab,
            Player.instance.headCollider.transform.position + (Player.instance.headCollider.transform.rotation * new Vector3(0, 0, 1.5f)),
            Player.instance.headCollider.transform.rotation
        );

        gameOverScreen.name = "game over screen";
        
        var greyScale = camera.gameObject.GetComponent<GreyscaleAfterEffect>();

        if (greyScale == null)
            return;

        greyScale.Active = true;
    }
}
