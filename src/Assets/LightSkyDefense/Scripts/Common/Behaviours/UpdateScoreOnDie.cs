using UnityEngine;
using Valve.VR.InteractionSystem;

public class UpdateScoreOnDie : MonoBehaviour
{
    public float Score = 0;

    // Update is called once per frame
    void OnDie()
    {
        var playerStatistics = Player.instance.GetComponent<PlayerStatistics>();

        playerStatistics.UpdateScore(Score);
    }
}
