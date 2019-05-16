using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class GameOver : MonoBehaviour, IOnGameLossTarget
{
    public GameObject GameOverPrefab;
    public bool Lost = false;


    public void OnGameLoss()
    {
        if (Lost)
            return;
        Lost = true;
        var camera = Camera.main;

        if (camera == null)
            return;

        var a = Instantiate(
            GameOverPrefab,
            Player.instance.headCollider.transform.position + (Player.instance.headCollider.transform.rotation * new Vector3(0, 0, 1.5f)),
            Player.instance.headCollider.transform.rotation
        );
        a.name = "game over screen";



        var greyScale = camera.gameObject.GetComponent<GreyscaleAfterEffect>();

        if (greyScale == null)
            return;

        greyScale.Active = true;
    }
}
