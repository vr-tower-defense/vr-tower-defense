﻿using System;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class BaseTower : MonoBehaviour
{
    #region states

    [Header("States")]
    public TowerState IdleState;
    public TowerState ActiveState;
    public TowerState CelebrationState;
    public TowerState CondemnState;
    public JamState JamState;

    [Header("Setup")]
    [Tooltip("The first state that is applied to the enemy")]
    public TowerState InitialState;

    [HideInInspector]
    public TowerState CurrentState;

    [HideInInspector]
    public TowerState[] TowerStates;

    #endregion

    [Tooltip("Transform objects that are used to spawn new projectiles")]
    public Transform[] ProjectileSpawns;

    [Tooltip("The layers that should be considered when checking for collisions")]
    public LayerMask DetectionLayerMask = (int)Layers.Enemies;

    [Tooltip("The range in meters which is used to check for collisions")]
    public float Range = .5f;

    public Buildable Preview;

    [HideInInspector]
    public Collider[] TargetsInRange { get; private set; } = new Collider[0];

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
        TowerStates = GetComponents<TowerState>();

        // Disable all states and enable the current state
        foreach (var state in TowerStates)
        {
            state.enabled = false;
        }

        CurrentState.enabled = true;
    }

    private void FixedUpdate()
    {
        TargetsInRange = Physics.OverlapSphere(
            transform.position,
            Range,
            (int)DetectionLayerMask
        );

        if (TargetsInRange.Length < 1 && CurrentState == ActiveState)
        {
            CurrentState.SetTowerState(IdleState);
        }

        if (TargetsInRange.Length > 0 && CurrentState == IdleState)
        {
            CurrentState.SetTowerState(ActiveState);
        }
    }

    /// <summary>
    /// Update the tower state when the player wins
    /// </summary>
    public void OnGameWin()
    {
        CurrentState.SetTowerState(CelebrationState);
    }

    /// <summary>
    /// Update the tower state when the player lost
    /// </summary>
    public void OnGameLose()
    {
        CurrentState.SetTowerState(CondemnState);
    }

    /// <summary>
    /// Update the tower state when the tower gets hit by a jammer
    /// </summary>
    public void OnJam(float jamTime)
    {
        JamState.JamTime = jamTime;
        CurrentState.SetTowerState(JamState);
    }
    /// <summary>
    /// Invoked by Damageable when the tower hits 0 hp
    /// </summary>
    public void OnDie()
    {
        Destroy(gameObject);
    }
    /// <summary>
    /// Invoked by Deletable when the player holds down the trigger
    /// </summary>
    public void OnDelete()
    {
        // Increment funds on tower delete
        var playerStatistics = Player.instance.GetComponent<PlayerStatistics>();
        playerStatistics.UpdateFunds(Preview.Price);

        Destroy(gameObject);
    }

    #region debugging

    /// <summary>
    /// Display the range when selected
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.1f);
        Gizmos.DrawSphere(transform.position, Range);
    }

    #endregion
}
