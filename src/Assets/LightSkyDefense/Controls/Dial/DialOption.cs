using UnityEngine;
using Valve.VR;

public abstract class DialOption : MonoBehaviour
{
    public bool IsSelected;

    /// <summary>
    /// Render program that is used to render the dial preview
    /// </summary>
    private Renderer _renderer;

    private void Start()
    {
        _renderer = gameObject.GetComponent<Renderer>();
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
        _renderer.material.color = new Color(
            _renderer.material.color.r,
            _renderer.material.color.g,
            _renderer.material.color.b,
            IsSelected ? 1f : .7f
        );

        if (!IsSelected)
        {
            return;
        }

        Debug.Log($"Update {this}");
    }
}
