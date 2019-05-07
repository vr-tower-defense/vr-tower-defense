using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SpawnDialOption : DialOption
{
    public GameObject prefab;

    /// <summary>
    /// Instance that is currently being placed
    /// </summary>
    private GameObject _clone;

    /// <summary>
    /// Create new instance of `prefab` and attach it to player hand
    /// </summary>
    public override void OnPressStart(SteamVR_Action_Vector2 action)
    {
        _clone = Instantiate(
            prefab,
            Player.instance.rightHand.transform.position,
            Player.instance.rightHand.transform.rotation
        );

        // Attach game object to hand
        Player.instance.rightHand.AttachObject(_clone, GrabTypes.Trigger);

        Debug.Log($"Created new instance of {_clone}");
    }

    /// <summary>
    /// Detach object from hand
    /// </summary>
    public override void OnPressUp(SteamVR_Action_Vector2 action)
    {
        Player.instance.rightHand.DetachObject(_clone);

        // Clear clone property
        _clone = null;
    }
}
