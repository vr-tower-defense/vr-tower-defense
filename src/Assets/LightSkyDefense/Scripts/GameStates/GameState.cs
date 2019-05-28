using System;
using UnityEngine;

public abstract class GameState : MonoBehaviour
{
    [HideInInspector]
    public GameManager GameManager;

    /// <summary>
    /// Get GameManager for later use
    /// </summary>
    private void Awake()
    {
        GameManager = GetComponent<GameManager>();
    }

    /// <summary>
    /// Updates active game state
    /// </summary>
    public void SetGameState(GameState gameState)
    {
        if (gameState == null)
        {
            return;
        }

        foreach (var state in GameManager.GameStates)
        {
            state.enabled = false;
        }

        gameState.enabled = true;
        GameManager.CurrentState = gameState;
    }
}