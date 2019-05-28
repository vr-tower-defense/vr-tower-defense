using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class Deletable : MonoBehaviour
{
    public AudioClip DestroySound;

    [Tooltip("The time in seconds that the button should be pressed down before the game object is deleted")]
    public float DeleteCooldown = .7f;

    [Header("Action properties")]
    public SteamVR_Action_Boolean TriggerClickAction = SteamVR_Input.GetBooleanAction("TriggerClick");

    public SteamVR_Input_Sources InputSource = SteamVR_Input_Sources.RightHand;

    [Header("Haptic properties")]
    [Tooltip("Duration in seconds")]
    [Range(0, 1)]
    public float Duration = .1f;

    public AnimationCurve AmplitudeOverCooldown;

    [Tooltip("The vibration frequency over time, should max out at DeleteCooldown")]
    public AnimationCurve FrequencyOverCooldown;

    // The time the button is pressed down
    private float _pressDownStartTime;

    /// <summary>
    /// Emit OnDelete message when button is pressed for given cooldown.
    /// Provide haptic feedback when button is pressed.
    /// </summary>
    private void HandHoverUpdate(Hand hand)
    {
        var pressedDown = TriggerClickAction.GetState(InputSource);

        // Ignore this function if button is not pressed down
        if (!pressedDown)
        {
            // Reset timer when trigger is not pressed down
            _pressDownStartTime = 0;

            return;
        }

        if (_pressDownStartTime == 0)
        {
            _pressDownStartTime = Time.time;
        }

        // Calculate the press down duration
        var pressDownDuration = Time.time - _pressDownStartTime;

        hand.TriggerHapticPulse(
            Duration,
            FrequencyOverCooldown.Evaluate(pressDownDuration),
            AmplitudeOverCooldown.Evaluate(pressDownDuration)
        );

        // Delete the gameObject after the cooldown
        if (pressDownDuration > DeleteCooldown)
        {
            gameObject.BroadcastMessage("OnDelete", SendMessageOptions.RequireReceiver);

            // Reset timer
            _pressDownStartTime = 0;
        }
    }

    /// <summary>
    /// Reset timer on hover end
    /// </summary>
    private void OnHandHoverEnd()
    {
        _pressDownStartTime = 0;
    }
}
