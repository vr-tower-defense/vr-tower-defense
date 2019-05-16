using UnityEngine;
using Valve.VR.InteractionSystem;

class PlacementCost : MonoBehaviour
{
    [Min(0)]
    public float Cost = 0;

    private void Start()
    {
        var playerStatistics = Player.instance.GetComponent<PlayerStatistics>();

        var outOfFunds = !playerStatistics.UpdateFunds(-Cost);

        if (outOfFunds)
        {
            // Funds are not updated, this means that the player doesn't have enough credits.
            Destroy(gameObject);
        }
    }
}
