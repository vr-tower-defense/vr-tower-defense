﻿using UnityEngine;
using Valve.VR.InteractionSystem;

class LoseState : GameState
{
    private void Start()
    {
        var camera = Camera.main;   

        if (camera == null)
            return;

        GameObject gameOverPrefab = (GameObject)Resources.Load("GameOverText", typeof(GameObject));

        var gameOverScreen = Instantiate(
            gameOverPrefab,
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
