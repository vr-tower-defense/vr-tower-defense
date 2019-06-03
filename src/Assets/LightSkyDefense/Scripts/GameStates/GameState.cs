using System;
using UnityEngine;

public abstract class GameState : MonoBehaviour
{
    /// <summary>
    /// Updates active game state
    /// </summary>
    public void SetGameState(GameState gameState)
    {
        if (gameState == null)
        {
            return;
        }

        foreach (var state in GameManager.Instance.GameStates)
        {
            state.enabled = false;
        }

        gameState.enabled = true;
        GameManager.Instance.CurrentState = gameState;
    }
}