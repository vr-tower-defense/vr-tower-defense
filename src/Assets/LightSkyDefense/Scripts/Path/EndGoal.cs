using UnityEngine;
using Valve.VR.InteractionSystem;

public class EndGoal : MonoBehaviour
{
    [Tooltip("Speed at which the Goal rotates")]
    public float RotationSpeed = 10f;

    private PlayerStatistics _playerStatistics;

    private void Start()
    {
        _playerStatistics = Player.instance.GetComponent<PlayerStatistics>();
    }

    void FixedUpdate()
    {
        transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime, Space.World);
    }

    void OnCollisionEnter(Collision collision)
    {
        var enemy = collision.gameObject.GetComponent<Enemy>();

        // Reduce player lives
        _playerStatistics.UpdateLives(-enemy.Damage);

        // Call OnFinish on Enemy
        enemy.BroadcastMessage(
            "OnFinish",
            null,
            SendMessageOptions.DontRequireReceiver
        );
    }
}