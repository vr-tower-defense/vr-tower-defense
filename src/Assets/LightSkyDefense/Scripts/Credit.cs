using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class Credit : MonoBehaviour
{
    public const int value = 5;
        
    private void OnHandHoverBegin(Hand hand)
    {
        var player = Player.instance.gameObject;
        var creditOwner = player.GetComponent<CreditOwner>();
        creditOwner?.AddCredits(value);
    }
}
