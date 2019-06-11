using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(BoxCollider))]
public abstract class MenuButton : MonoBehaviour
{
    [Header("Action")]
    public SteamVR_Action_Boolean ButtonClickAction = SteamVR_Input.GetBooleanAction("InteractWithUI");

    [Header("Button appearance")]
    public TextMesh TextElement;
    public Color HoverColor = Color.red;
    public Color ActiveColor = Color.green;

    private Color _initialColor;

    private void Start()
    {
        _initialColor = TextElement.color;
    }

    private void OnHandHoverBegin(Hand hand)
    {
        TextElement.color = HoverColor;
    }

    private void OnHandHoverEnd(Hand hand)
    {
        TextElement.color = _initialColor;
    }

    private void HandHoverUpdate(Hand hand)
    {
        var pressedDown = ButtonClickAction.GetStateDown(SteamVR_Input_Sources.Any);

        // Ignore this function if button is not pressed down
        if (!pressedDown)
        {
            TextElement.color = HoverColor;
            return;
        }

        hand.TriggerHapticPulse(1000);
        TextElement.color = ActiveColor;

        OnClick(hand);
    }

    public abstract void OnClick(Hand hand);
}
