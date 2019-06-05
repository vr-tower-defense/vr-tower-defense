using System.Collections;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SpawnDialOption : DialOption
{
    [Header("Haptic feedback")]
    [Tooltip("Duration in seconds")]
    public float HapticPulseDuration = .1f;

    public float HapticPulseFrequency = 15f;

    [Header("Other stuff")]
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

        StartCoroutine(OnBuild(hand));
    }

    /// <summary>
    /// Detach object from hand
    /// </summary>
    public override void OnRelease(SteamVR_Action_Vector2 action)
    {
        StopAllCoroutines();

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
            hand.objectAttachmentPoint,
            SendMessageOptions.RequireReceiver
        );
    }

    /// <summary>
    /// Apply haptic feedback when user is building a tower
    /// </summary>
    private IEnumerator OnBuild(Hand hand)
    {
        hand.TriggerHapticPulse(HapticPulseDuration, HapticPulseFrequency, 1);

        yield return new WaitForSeconds(HapticPulseDuration);
        yield return OnBuild(hand);
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
