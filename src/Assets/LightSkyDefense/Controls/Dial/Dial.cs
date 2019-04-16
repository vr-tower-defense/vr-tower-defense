using System;
using UnityEngine;
using Valve.VR;

public class Dial : MonoBehaviour
{
    public SteamVR_Action_Vector2 DialAction = SteamVR_Input.GetVector2Action("Dial");
    public SteamVR_Action_Boolean DialClickAction = SteamVR_Input.GetBooleanAction("DialClick");

    [Tooltip("Offset in degrees on which the first element in dialOptions starts, ie. make the options horizontal by adding 90 degrees")]
    public int DialRotationOffset = 90;

    [Tooltip("Options that can are placed onto the Dial")]
    public GameObject[] DialOptions;

    // Field used to check whether user is in process of clicking touchpad
    private DialOption _pressedDial;

    /// <summary>
    /// Valdiate properties
    /// </summary>
    private void Start()
    {
        // Validate actions
        if (DialAction == null)
            Debug.LogError("`Dial` action has not been set on this component.");

        if (DialClickAction == null)
            Debug.LogError("`DialClick` action has not been set on this component.");
    }

    // Update is called once per frame
    void Update()
    {
        ResetDialOptions();

        // Ignore the rest of this method if the touchpad is not touched
        if (DialAction.axis == Vector2.zero)
        {
            return;
        }

        // Find dial option script
        var dialOption = FindDialOption(DialAction.axis);

        // Update dial option IsSelected state to true, this property could for 
        // example be used to change the appearance when the dial option is selected
        dialOption.IsSelected = true;

        // Invoke OnPressUp when dial touchpad click ends
        if (_pressedDial != null && DialClickAction.stateUp)
        {
            _pressedDial.OnPressUp(DialAction);
            _pressedDial = null;
        }

        // Invoke OnPressDown when touchpad is pressed down
        if (DialClickAction.stateDown)
        {
            // Invoke OnPressStart handler once
            if (_pressedDial == null)
            {
                _pressedDial = dialOption;
                _pressedDial.OnPressStart(DialAction);
            }

            _pressedDial.OnPressDown(DialAction);
        }
    }

    /// <summary>
    /// Finds DialOption on dial
    /// </summary>
    /// <param name="positionOnTouchpad"></param>
    private DialOption FindDialOption(Vector2 positionOnTouchpad)
    {
        // We can't find a dial option when there are no options :D
        if(DialOptions.Length < 1)
        {
            return null;
        }

        // Find option index by calculating the angle of the touchpad position
        var radians = Mathf.Atan2(positionOnTouchpad.x, positionOnTouchpad.y);
        var degrees = (Mathf.Rad2Deg * radians) - DialRotationOffset;

        if (degrees < 0)
        {
            degrees += 360;
        }

        var optionDegreeSpan = 360 / DialOptions.Length;
        var optionIndex = Mathf.FloorToInt(degrees / optionDegreeSpan);

        // Get dial option
        var dialOption = DialOptions[optionIndex];

        if(dialOption == null)
        {
            Debug.LogWarning($"GameObject on index {optionIndex} is empty!");
            return null;
        }
        
        // Get dial option script
        var dialOptionScript = dialOption?.GetComponent<DialOption>();

        if (dialOptionScript == null)
        {
            Debug.LogWarning($"GameObject on index {optionIndex} does not have dial option script!");
            return null;
        }

        return dialOptionScript;
    }

    /// <summary>
    /// Resets all dial options
    /// </summary>
    private void ResetDialOptions()
    {
        // Reset IsSelected property on all DialOptions
        // We might want to do this only once since this loop will run every update.
        foreach (var dialOption in DialOptions)
        {
            if (dialOption == null)
            {
                continue;
            }

            var dialOptionScript = dialOption.GetComponent<DialOption>();
            if (dialOptionScript != null)
            {
                dialOptionScript.IsSelected = false;
            }
        }
    }
}
