using System;
using UnityEngine;

public abstract class GameState : MonoBehaviour
{
    protected GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GetComponent<GameManager>();
    }

    public void SetGameState(Type gameState)
    {
        _gameManager.SetGameState(gameState);
    }
}