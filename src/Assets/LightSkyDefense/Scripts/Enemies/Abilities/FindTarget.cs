﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTarget : MonoBehaviour
{
    public EnableOnTarget HasTarget;

    [Tooltip("Radius in which a target must be before it is considered a threat")]
    public float Radius;

    [Tooltip("Layer bitmask, set to Towers by default")]
    public LayerMask LayerBitMask = (int)Layers.Towers;

    private void Awake()
    {
        // Disable shooting state on awake by default
        HasTarget.enabled = false;
    }

    private void FixedUpdate()
    {
        var nearestTarget = Find();

        if (nearestTarget == null)
            return;

        // Set new target
        HasTarget.Target = nearestTarget;

        // Change active script
        HasTarget.enabled = true;
        enabled = false;
    }

    public Collider Find()
    {
        var TargetsInRange = Physics.OverlapSphere(
            transform.position,
            Radius, 
            LayerBitMask);

        // Abort function when there isn't any target in the overlap sphere
        if (TargetsInRange.Length < 1)
        {
            return null;
        }

        // Find target that is near this game object
        Collider nearestTarget = null;
        float minimalDistance = float.MaxValue;

        foreach (var target in TargetsInRange)
        {
            var distance = Vector3.Distance(transform.position, target.transform.position);

            if (distance < minimalDistance)
            {
                minimalDistance = distance;
                nearestTarget = target;
            }
        }

        return nearestTarget; 
    }
    #region debugging

    /// <summary>
    /// Display the range
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 0, 1, 0.1f);
        Gizmos.DrawSphere(transform.position, Radius);
    }

    #endregion
}
