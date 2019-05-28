using UnityEngine;
using Valve.VR.InteractionSystem;

public class UpdateScoreOnDie : MonoBehaviour
{
    public float Score = 0;

    /// <summary>
    /// grant player points on dead
    /// </summary>
    void OnDie()
    {
        var playerStatistics = Player.instance.GetComponent<PlayerStatistics>();

        playerStatistics.UpdateScore(Score);
    }
}
