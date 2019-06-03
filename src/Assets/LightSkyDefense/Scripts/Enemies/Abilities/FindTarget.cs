using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTarget : MonoBehaviour
{
    public ShootAtTarget ShootAtTarget;

    [Tooltip("Radius in which a target must be before it is considered a threat")]
    public float Radius;

    [Tooltip("Layer bitmask, set to Towers by default")]
    public int LayerBitMask = (int)Layers.Towers;

    private void Awake()
    {
        // Disable shooting state on awake by default
        ShootAtTarget.enabled = false;
    }

    private void FixedUpdate()
    {
        var TargetsInRange = Physics.OverlapSphere(transform.position, Radius, LayerBitMask);

        // Abort function when there isn't any target in the overlap sphere
        if (TargetsInRange.Length < 1)
        {
            return;
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

        // Set new target
        ShootAtTarget.Target = nearestTarget;

        // Change active script
        ShootAtTarget.enabled = true;
        enabled = false;
    }
}
