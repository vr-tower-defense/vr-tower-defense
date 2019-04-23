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
        var creditOwner = Player.instance.gameObject.GetComponent<CreditOwner>();

        if (creditOwner == null) { return; }

       
        creditOwner.Credits += Value;
        AudioSource.PlayClipAtPoint(PickupSound, this.gameObject.transform.position);

        Destroy(gameObject);
    }


}
