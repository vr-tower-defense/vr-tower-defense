using UnityEngine;
using Valve.VR;

public abstract class DialOption : MonoBehaviour
{
    [Tooltip("Value between 0 and 1")]
    public float InactiveTransparency = .5f;

    [HideInInspector]
    public bool IsSelected;

    /// <summary>
    /// Render program that is used to render the dial preview
    /// </summary>
    private Renderer[] _renderers;

    private void Start()
    {
        _renderers = gameObject.GetComponentsInChildren<Renderer>();
    }

    /// <summary>
    /// Method that is invoked when the player presses down on the touchpad
    /// </summary>
    public virtual void OnPressDown(SteamVR_Action_Vector2 action)
    { }

    /// <summary>
    /// Method that is invoked once when the player stops pressing on the touchpad
    /// </summary>
    public virtual void OnPressUp(SteamVR_Action_Vector2 action)
    { }

    /// <summary>
    /// Method that is invoked when the player starts pressing down on the touchpad
    /// </summary>
    public virtual void OnPressStart(SteamVR_Action_Vector2 action)
    { }


    private void Update()
    {
        foreach(var renderer in _renderers)
        {
            renderer.material.color = new Color(
                renderer.material.color.r,
                renderer.material.color.g,
                renderer.material.color.b,
                IsSelected ? .5f : InactiveTransparency
            );
        }
    }
}
