using UnityEngine;
using Valve.VR;

public class Dial : MonoBehaviour
{
    public SteamVR_Action_Vector2 DialAction = SteamVR_Input.GetVector2Action("Dial");
    public SteamVR_Action_Boolean DialClickAction = SteamVR_Input.GetBooleanAction("DialClick");

    [Tooltip("Offset in degrees on which the first element in dialOptions starts, ie. make the options horizontal by adding 90 degrees")]
    public int dialRotationOffset = 90;

    [Tooltip("Options that can are placed onto the Dial")]
    public GameObject[] dialOptions;

    /// <summary>
    /// Valdiate properties
    /// </summary>
    private void Start()
    {
        // Validate actions
        if (DialAction == null)
            Debug.LogError("`Dial` action has been set on this component.");

        if (DialClickAction == null)
            Debug.LogError("`DialClick` action has been set on this component.");
    }

    // Update is called once per frame
    void Update()
    {
        // Ignore dial options if there are none
        if(dialOptions.Length < 1)
        {
            return;
        }

        // Get touchpad position
        var positionOnTouchpad = DialAction.axis;

        if (positionOnTouchpad == Vector2.zero)
        {
            // Reset IsSelected property on all DialOptions
            // We might want to do this only once since this loop will run every update.
            foreach (var dialOption in dialOptions)
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

            return;
        }

        // Find option index on dial by calculating the angle of the touchpad position
        var radians = Mathf.Atan2(positionOnTouchpad.x, positionOnTouchpad.y);
        var degrees = (Mathf.Rad2Deg * radians) + -dialRotationOffset;

        if(Mathf.Sign(degrees) == -1)
        {
            degrees += 360;
        }

        var optionDegreeSpan = 360 / dialOptions.Length;
        var optionIndex = Mathf.FloorToInt(degrees / optionDegreeSpan);

        // Find DialOption GameObject's script
        var gameObjectDialOptionScript = dialOptions[optionIndex]
            .GetComponent<DialOption>();

        if(gameObjectDialOptionScript == null)
        {
            Debug.LogWarning("Game object does not have dial option script!");
            return;
        }

        gameObjectDialOptionScript.IsSelected = true;

        // Invoke DialOption.OnClick when touchpad is clicked
        if (DialClickAction.stateDown)
        {
            gameObjectDialOptionScript.OnClick();
        }
    }
}
