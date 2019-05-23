using System;
using UnityEngine;

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

    [Tooltip("Transform objects that are used to spawn new projectiles")]
    public Transform[] ProjectileSpawns;

    [Tooltip("The layers that should be considered when checking for collisions")]
    public Layers DetectionLayerMask = Layers.Enemies;

    [Tooltip("The range in meter which used to check for collisions")]
    public float Range = .25f;

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

        // Disable all states and enable the current state
        foreach (var state in GetComponents<TowerState>())
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
            (int) DetectionLayerMask
        );
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
