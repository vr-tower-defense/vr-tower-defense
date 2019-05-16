using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class BaseTower : MonoBehaviour, IOnGameLoseTarget, IOnGameWinTarget
{
    #region states

    public MonoBehaviour IdleState;
    public MonoBehaviour ActiveState;
    public MonoBehaviour CelebrationState;
    public MonoBehaviour CondemnState;

    #endregion

    [HideInInspector]
    public HashSet<Enemy> TargetsInRange { get; } = new HashSet<Enemy>();

    /// <summary>
    /// Set initial state
    /// </summary>
    private void Start()
    {
        IdleState.enabled = true;
        ActiveState.enabled = false;
        CelebrationState.enabled = false;
        CondemnState.enabled = false;
    }

    /// <summary>
    /// Adds target to list when target is withing tower's range
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<Enemy>();

        if(enemy == null)
        {
            return;
        }

        TargetsInRange.Add(enemy);
    }

    /// <summary>
    /// Removes target from list when target is withing tower's range
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerLeave(Collider other)
    {
        var enemy = other.GetComponent<Enemy>();

        if(enemy == null)
        {
            return;
        }

        TargetsInRange.Remove(enemy);
    }

    /// <summary>
    /// Update the tower state when the player wins
    /// </summary>
    public void OnGameWin()
    {
        var currentState = gameObject.GetComponent<TowerState>();
        currentState.SetTowerState(CelebrationState);
    }

    /// <summary>
    /// Update the tower state when the player lost
    /// </summary>
    public void OnGameLose()
    {
        var currentState = gameObject.GetComponent<TowerState>();
        currentState.SetTowerState(CondemnState);
    }
}
