using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SpawnDialOption : DialOption
{
    public Buildable Preview;

    public GameObject PriceText;

    private PlayerStatistics _playerStatistics;

    private void Awake()
    {
        _playerStatistics = Player.instance.GetComponent<PlayerStatistics>();

        SetPrice();
    }

    /// <summary>
    /// Create new instance of `prefab` and attach it to player hand
    /// </summary>
    public override void OnPressStart(SteamVR_Action_Vector2 action)
    {
        if (_playerStatistics.Funds < Preview.Price)
        {
            return;
        }

        var hand = Player.instance.GetHand(action.activeDevice);

        hand.AttachObject(
            Instantiate(Preview.gameObject),
            GrabTypes.None
        );

    }

    /// <summary>
    /// Detach object from hand
    /// </summary>
    public override void OnRelease(SteamVR_Action_Vector2 action)
    {
        var hand = Player.instance.GetHand(action.activeDevice);
        var preview = hand.currentAttachedObject;

        if (preview == null)
        {
            return;
        }

        // Destroy preview and replace with "real" instance
        hand.DetachObject(preview);
        Destroy(preview);

        // Handle buildable logic when component exist
        var buildable = preview.GetComponent<Buildable>();

        if (buildable == null || !buildable.IsPositionValid)
        {
            return;
        }

        // Create final instance when position is valid
        buildable.SendMessage(
            "OnBuild",
            transform,
            SendMessageOptions.RequireReceiver
        );
    }

    /// <summary>
    /// Update price in PriceText
    /// </summary>
    private void SetPrice()
    {
        var textMesh = PriceText.GetComponent<TextMesh>();

        if (textMesh == null)
        {
            return;
        }

        textMesh.text = Preview.Price.ToString();
    }
}
