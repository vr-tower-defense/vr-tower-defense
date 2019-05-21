using System;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(SphereCollider))]
public class BaseTower : MonoBehaviour
{
    #region states

    [Header("States")]
    public TowerState IdleState;
    public TowerState ActiveState;
    public TowerState CelebrationState;
    public TowerState CondemnState;

    [Header("Setup")]
    [Tooltip("The first state that is applied to the enemy")]
    public TowerState InitialState;

    [HideInInspector]
    public TowerState CurrentState;

    #endregion

    [HideInInspector]
    public HashSet<Enemy> TargetsInRange { get; } = new HashSet<Enemy>();

    /// <summary>
    /// Set initial state
    /// </summary>
    private void Start()
    {
        if (InitialState == null)
        {
            throw new ArgumentNullException();
        }

        // Save intial state reference to current state field
        CurrentState = InitialState;

        // Disable all states and enable the current state
        foreach (var state in GetComponents<TowerState>())
        {
            state.enabled = false;
        }

        CurrentState.enabled = true;
    }

    /// <summary>
    /// Adds target to list when target is withing tower's range
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<Enemy>();

        if (enemy == null)
        {
            return;
        }

        TargetsInRange.Add(enemy);

        CurrentState.SetTowerState(ActiveState);
    }

    /// <summary>
    /// Removes target from list when target is withing tower's range
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        var enemy = other.GetComponent<Enemy>();

        if (enemy == null)
        {
            return;
        }

        TargetsInRange.Remove(enemy);

        if (TargetsInRange.Count < 1)
        {
            CurrentState.SetTowerState(IdleState);
        }
    }

    /// <summary>
    /// Update the tower state when the player wins
    /// </summary>
    public void OnGameWin()
    {
        var currentState = gameObject.GetComponents<TowerState>();
        currentState.ForEach(st =>  st.SetTowerState(CelebrationState));
    }

    /// <summary>
    /// Update the tower state when the player lost
    /// </summary>
    public void OnGameLose()
    {
        var currentState = gameObject.GetComponents<TowerState>();
        currentState.ForEach(st => st.SetTowerState(CelebrationState));
    }
}
