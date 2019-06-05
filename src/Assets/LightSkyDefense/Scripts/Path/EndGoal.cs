using UnityEngine;
using Valve.VR.InteractionSystem;

public class EndGoal : MonoBehaviour
{
    [Tooltip("Speed at which the Goal rotates")]
    public float RotationSpeed = 10f;

    void FixedUpdate()
    {
        transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime, Space.World);
    }

    void OnCollisionEnter(Collision collision)
    {
        var playerStatistics = Player.instance.GetComponent<PlayerStatistics>();

        // Reduce player lives
        playerStatistics.UpdateLives(-1);

        // Destroy enemy
        Destroy(collision.gameObject);
    }
}