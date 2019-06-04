using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class Buildable : MonoBehaviour
{
    public float Price = 1;

    public GameObject Prefab;

    [Header("Position check properties")]
    public Material InvalidPositionMaterial;

    [Tooltip("The radius around the tower that cannot collide with any of the given layers")]
    public float Margin = .2f;

    [HideInInspector]
    public bool IsPositionValid = false;

    private Layers _layerMask = Layers.Path | Layers.Enemies | Layers.Towers;

    private Renderer _renderer;

    private Material _originalMaterial;

    private void Start()
    {
        // Store original material
        _renderer = GetComponentInChildren<Renderer>();
        _originalMaterial = _renderer.material;
    }

    private void FixedUpdate()
    {
        IsPositionValid = !Physics.CheckSphere(
            transform.position,
            Margin,
            (int)_layerMask
        );

        _renderer.material = IsPositionValid
            ? _originalMaterial
            : InvalidPositionMaterial;
    }

    /// <summary>
    /// Creates a new instance of the Prefab property and subtracts the given Cost from the players' funds
    /// </summary>
    private void OnBuild(Transform transform)
    {
        var playerStatistics = Player.instance.GetComponent<PlayerStatistics>();

        // Update player funds
        playerStatistics.UpdateFunds(-Price);

        Instantiate(
            Prefab,
            transform.position,
            transform.rotation
        );
    }

    #region debugging

    /// <summary>
    /// Display the range when selected
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.1f);
        Gizmos.DrawSphere(transform.position, Margin);
    }

    #endregion
}
