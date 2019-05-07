using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR.InteractionSystem;

public class PlayerStats : MonoBehaviour
{
    private int _lives;
    private int _credits;


    public int StartLives = 5;
    public int StartCredits = 20;

    public int Lives
    {
        get => lives;
        set
        {
            lives = value;
            if (lives > 0)
                return;

            GameObject[] targets = gameObject.scene.GetRootGameObjects();
            targets.ForEach(t => ExecuteEvents.Execute<IOnGameLossTarget>(t, null, ((handler, _) => handler.OnGameLoss())));
        }
    }

    public int Credits
    {
        get => credits;
        set => credits = value;
    }

    public void Start()
    {
        Lives = StartLives;
        Credits = StartCredits;
    }
}
