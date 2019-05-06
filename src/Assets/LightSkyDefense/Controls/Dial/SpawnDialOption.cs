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
        _clone = Instantiate(prefab);
    }

    public override void OnPressDown(SteamVR_Action_Vector2 action)
    {
        if (_clone == null) return;

        _clone.transform.position = Player.instance.rightHand.transform.position;
    }

    /// <summary>
    /// Detach object from hand
    /// </summary>
    public override void OnPressUp(SteamVR_Action_Vector2 action)
    {
        // Clear clone property
        _clone = null;
    }
}
