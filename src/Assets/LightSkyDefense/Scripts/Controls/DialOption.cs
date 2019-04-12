using System;
using UnityEngine;

public class DialOption : MonoBehaviour
{
    public bool IsSelected;

    /// <summary>
    /// Method that is invoked when the player clicks on the touchpad
    /// </summary>
    public virtual void OnClick()
    {
        throw new NotImplementedException();
    }
}
