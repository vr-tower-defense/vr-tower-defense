using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SpawnDialOption : DialOption
{
    public GameObject Preview;
    public GameObject Final;

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
        var handTransform = Player.instance.rightHand.transform;

        // Destroy clone and replace with "real" instance
        Destroy(_preview);
        Instantiate(Final, handTransform.position, handTransform.rotation);
    }

    // Applies an upwards force to all rigidbodies that enter the trigger.
    void OnTriggerStay(Collider collider)
    {
        Debug.Log("bAammm collision" + collider.name);
    }

    // Applies an upwards force to all rigidbodies that enter the trigger.
    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("bAammm collision enter" + collider.name);
    }
}
