using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public abstract class DialOption : MonoBehaviour
{
    public Material InactiveMaterial;
    public Material ActiveMaterial;

    public MeshRenderer TargetMesh;

    [HideInInspector]
    public bool IsSelected;

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
        TargetMesh.material = IsSelected
            ? ActiveMaterial
            : InactiveMaterial;
    }
}
