using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Flocking", menuName = "SteeringBehaviour/Flocking")]
public class Flocking : ISteering
{
    [Header("Movement properties")]
    [Range(0, 1)]
    public float MaxSpeed = .2f;

    [Tooltip("The maximum number of agents that are considered to calculate the forces. A higher number will result in a more precise behaviour but will use more cpu cycles")]
    public int MaxNeighbours = 5;

    [Header("Flocking properties")]
    public int FlockingLayerMask = (int)Layers.Enemies;

    [Range(0, 1)]
    public float CohesionRadius = .5f;

    [Range(0, 1)]
    public float SeparationMultiplier = .3f;

    [Header("Behaviour weights")]
    [Range(0, 1)]
    public float CohesionWeight = .5f;

    [Range(0, 1)]
    public float SeparationForce = .5f;

    [Range(0, 1)]
    public float AlignmentForce = .5f;

    private float _seperationRadiusSquared;

    /// <summary>
    /// Calculate the seperation distances squared to avoid repeating this calculation every loop
    /// </summary>
    public override void Initialize(GameObject gameObject)
    {
        _seperationRadiusSquared = Mathf.Pow(CohesionRadius * SeparationMultiplier, 2);
    }

    public override Vector3 Calculate(GameObject gameObject)
    {
        var neighbours = Physics.OverlapSphere(
            gameObject.transform.position,
            CohesionRadius,
            FlockingLayerMask
        );

        // We can't divide by zero so we must check if we have any neighbours
        if (neighbours.Length < 2)
        {
            return Vector3.zero;
        }

        var cohesionForce = Vector3.zero;
        var alignmentForce = Vector3.zero;
        var separationForce = Vector3.zero;

        var seperationCount = 0;
        var neighbourCount = Math.Min(MaxNeighbours, neighbours.Length - 1);

        for (int i = 0; i < neighbours.Length; i++)
        {
            var neighbour = neighbours[i];

            // Stop aggregating more forces to minimize performance impact
            if (i > MaxNeighbours)
            {
                break;
            }

            if (neighbour.transform == gameObject.transform)
            {
                continue;
            }

            // Calculate the direction to the neightbour
            var toNeighbour = neighbour.transform.position - gameObject.transform.position;

            // Sum neighbour positions to calculate the cohesion target
            cohesionForce += neighbour.transform.position;

            // Sum the normalize direction (heading) for the alignment behaviour
            alignmentForce += Vector3.Normalize(toNeighbour);

            // Check whether current neighbour should be considered for the separation force
            if (
                toNeighbour.sqrMagnitude <= _seperationRadiusSquared
            )
            {
                // Scale the force inversely proportional to the objects distance from its neighbor
                separationForce += Vector3.Normalize(-toNeighbour) / toNeighbour.magnitude;
                seperationCount++;
            }
        }

        // Calculate direction towards average position for the cohesion force
        cohesionForce = gameObject.transform.position - cohesionForce;

        // Calculate average heading
        alignmentForce /= neighbourCount;

        // Calculate average distance from neighbours that are considered for separation behaviour
        separationForce /= seperationCount > 0 ? seperationCount : 1;

        return WeightedTruncatedRunningSumWithPrioritization.Calculate(
            new Tuple<Vector3, float>[] {
                new Tuple<Vector3, float>(separationForce, SeparationForce),
                new Tuple<Vector3, float>(alignmentForce, AlignmentForce),
                new Tuple<Vector3, float>(cohesionForce, CohesionWeight),
            },
            MaxSpeed
        );
    }

    #region debugging

    public override void DrawGizmos(GameObject gameObject)
    {
        Gizmos.color = new Color(0, 0, 1, 0.1f);

        Gizmos.DrawSphere(gameObject.transform.position, CohesionRadius);
        Gizmos.DrawSphere(gameObject.transform.position, CohesionRadius * SeparationMultiplier);
    }

    #endregion
}