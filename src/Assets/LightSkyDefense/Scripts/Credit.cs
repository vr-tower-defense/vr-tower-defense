using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class Credit : MonoBehaviour
{
    public int Value = 5;
    public AudioClip PickupSound;


    /// <summary>
    /// When the credit touches the hand, the credit gets added to the players total credit count
    /// </summary>
    /// <param name="hand"></param>
    private void OnHandHoverBegin(Hand hand)
    {
        var creditOwner = Player.instance.gameObject.GetComponent<PlayerStats>();

        if (creditOwner == null) { return; }

       
        creditOwner.Credits += Value;
        SoundUtil.PlayClipAtPointWithRandomPitch(PickupSound, this.gameObject.transform.position, 0.8f, 1.2f);

        Destroy(gameObject);
    }


}
