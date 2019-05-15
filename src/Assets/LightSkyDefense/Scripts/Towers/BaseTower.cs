using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    #region states

    public Type IdleState { get; protected set; }
    public Type ActiveState { get; protected set; }
    public Type CelebrationState { get; protected set; }
    public Type CondemnState { get; protected set; }

    #endregion

    [HideInInspector]
    public List<GameObject> TargetsInRange { get; } = new List<GameObject>();

    /// <summary>
    /// Set initial state
    /// </summary>
    private void Start()
    {
        if(IdleState == null)
        {
            throw new NullReferenceException();
        }

        gameObject.AddComponent(IdleState);
    }

    /// <summary>
    /// Adds target to list when target is withing tower's range
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        TargetsInRange.Add(other.gameObject);
    }

    /// <summary>
    /// Removes target from list when target is withing tower's range
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerLeave(Collider other)
    {
        TargetsInRange.Remove(other.gameObject);
    }

    /// <summary>
    /// Update the tower state when the player lost
    /// </summary>
    public void OnGameLoss()
    {
        var currentState = gameObject.GetComponent<TowerState>();
        currentState.SetTowerState(CondemnState);
    }

    /// <summary>
    /// Update the tower state when the player wins
    /// </summary>
    public void OnGameWin()
    {
        var currentState = gameObject.GetComponent<TowerState>();
        currentState.SetTowerState(CelebrationState);
    }
}
