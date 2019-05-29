using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SpawnDialOption : DialOption
{
    public GameObject Preview;

    /// <summary>
    /// Instance that is currently being placed
    /// </summary>
    private GameObject _preview;

    /// <summary>
    /// Create new instance of `prefab` and attach it to player hand
    /// </summary>
    public override void OnPressStart(SteamVR_Action_Vector2 action)
    {
        var handTransform = Player.instance.rightHand.transform;

        _preview = Instantiate(
            Preview,
            handTransform.position,
            handTransform.rotation,
            Player.instance.rightHand.transform
        );
    }

    /// <summary>
    /// Detach object from hand
    /// </summary>
    public override void OnPressUp(SteamVR_Action_Vector2 action)
    {
        var buildable = _preview.GetComponent<Buildable>();

        // Destroy clone and replace with "real" instance
        Destroy(_preview);

        // Create final instance when position is valid
        if (!buildable.IsPositionValid)
        {
            return;
        }

        buildable.SendMessage(
            "OnBuild",
            Player.instance.rightHand.transform,
            SendMessageOptions.RequireReceiver
        );
    }
}
