using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wander", menuName = "SteeringBehaviour/Wander")]
public class Wander : ISteering
{
    public float WanderRadius = .2f;
    public float WanderDistance = .2f;
    public float WanderJitter = 5f;

    private Vector3 _wanderTarget = Vector3.zero;

    public override void Initialize(GameObject gameObject)
    { }

    public override Vector3 Calculate(GameObject gameObject)
    {
        var addToPerimeter = Random.onUnitSphere * (WanderJitter * Time.deltaTime);

        // Update local wander target
        _wanderTarget += addToPerimeter;
        _wanderTarget = Vector3.Normalize(_wanderTarget);
        _wanderTarget *= WanderRadius;

        var wanderSpherePosition =
            gameObject.transform.position +
            gameObject.transform.forward * WanderDistance;

        var worldSpaceTarget = wanderSpherePosition + _wanderTarget;

        return worldSpaceTarget - gameObject.transform.position;
    }

    public override void DrawGizmos(GameObject gameObject)
    {
        // Draw wander sphere
        var wanderSpherePosition =
            gameObject.transform.position +
            gameObject.transform.forward * WanderDistance;

        Gizmos.color = new Color(0, 1, 1, .1f);
        Gizmos.DrawSphere(wanderSpherePosition, WanderRadius);

        // Draw wander target
        var worldSpaceTarget = wanderSpherePosition + _wanderTarget;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(worldSpaceTarget, .02f);
    }
}
