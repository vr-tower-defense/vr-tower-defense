﻿using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR.InteractionSystem;

public class PlayerStatistics : MonoBehaviour
{
    [SerializeField]
    private readonly int InitialLives = 5;

    [SerializeField]
    private readonly int InitialFunds = 20;

    [HideInInspector]
    public int Lives { get; private set; }

    [HideInInspector]
    public float Funds { get; private set; }

    /// <summary>
    /// Set the initial values
    /// </summary>
    public void Start()
    {
        Lives = InitialLives;
        Funds = InitialFunds;
    }

    /// <summary>
    /// Updates the players' funds
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>Boolean indicating whether action was successful or not</returns>
    public bool UpdateFunds(float amount)
    {
        var tempFunds = Funds + amount;

        if(tempFunds < 0)
        {
            return false;
        }

        Funds = tempFunds;
        return true;
    }

    /// <summary>
    /// Update the players' lives
    /// </summary>
    /// <param name="amount"></param>
    public void UpdateLives(int amount)
    {
        Lives += amount;

        if (Lives > 0)
        {
            return;
        }

        gameObject
            .scene
            .GetRootGameObjects()
            .ForEach(target =>
                ExecuteEvents.Execute<IOnGameLoseTarget>(
                    target,
                    null,
                    (handler, _) => handler.OnGameLose()
                )
            );
    }
}
