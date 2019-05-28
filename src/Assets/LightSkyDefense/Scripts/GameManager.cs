using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region singleton

    // Reference to path
    private static GameManager _gameManager;

    // GameManager singleton
    [HideInInspector]
    public static GameManager Instance => _gameManager ?? (_gameManager = FindObjectOfType<GameManager>());

    #endregion

    /// <summary>
    /// Boolean indicating whether the application is about to quit
    /// </summary>
    [HideInInspector]
    public static bool IsQuitting { get; private set; } = false;

    #region game states

    [HideInInspector]
    public GameState[] GameStates;

    [HideInInspector]
    public GameState CurrentState;

    /// <summary>
    /// The first game state that is added to the game managers' game object
    /// </summary>
    public GameState InitialState;
    public GameState LoseState;

    #endregion

    #region lifecyle functions

    /// <summary>
    /// Initialze the game state
    /// </summary>
    public void Awake()
    {
        // Save intial state reference to current state field
        CurrentState = InitialState;
        GameStates = GetComponents<GameState>();

        // Disable all states and enable the current state
        foreach (var state in GameStates)
        {
            state.enabled = false;
        }

        CurrentState.enabled = true;
    }

    /// <summary>
    /// Invoked when the application is about to quit. We set
    /// the IsQuitting variable to true, to avoid missing references errors in OnDestroy methods.
    /// </summary>
    private void OnApplicationQuit()
    {
        IsQuitting = true;
    }

    #endregion

    /// <summary>
    /// Sets game state to LoseState
    /// </summary>
    public void OnGameLose()
    {
        CurrentState.SetGameState(LoseState);
    }
}

