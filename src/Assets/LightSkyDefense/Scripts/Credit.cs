using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class Credit : MonoBehaviour
{
    public const int value = 5;
        
    /// <summary>
    /// When the credit touches the hand, the credit gets added to the players total credit count
    /// </summary>
    /// <param name="hand"></param>
    private void OnHandHoverBegin(Hand hand)
    {
        var player = Player.instance.gameObject;
        var creditOwner = player.GetComponent<CreditOwner>();
        creditOwner?.AddCredits(value);
        Destroy(gameObject);
    }
}
