using UnityEngine;

public class FollowPathPoint
{
    private static float _distance = 0.01f;

    public static Vector3 Calculate(Transform followerTransform, float pathOffsetDistance, int currentPathPoinIndex, Path.PathPoint nexPathPoint, float maxSpeed = 0.002f)
    {
        // Move our position a step closer to the target.
        //float step = speed * Time.deltaTime; // calculate distance to move
        return Vector3.MoveTowards(followerTransform.position, nexPathPoint.Position, maxSpeed);

        //Vector3 tmp = nexPathPoint.Position + new Vector3(nexPathPoint.Position.x, 0, 0);
        //Vector3 aNormal = Vector3.Cross(tmp, nexPathPoint.Position + nexPathPoint.DirectionVector).normalized*pathOffsetDistance;

        //if (Vector3.Distance(rigidbody.position, nexPathPoint.Position) > _distance)
        ////if (rigidbody.position != nexPathPoint.Position)
        //{
        //    return rigidbody.mass * (maxSpeed * (nexPathPoint.DirectionVector).normalized);
        //}
        //else
        //{
        //    return Vector3.zero;
        //}
    }
}
