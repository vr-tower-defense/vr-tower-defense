using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class Credit : MonoBehaviour
{
    public const int value = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnHandHoverBegin(Hand hand)
    {
        var player = Player.instance.gameObject;
        var creditOwner = GetComponent<CreditOwner>();
        creditOwner?.AddCredits(value);
    }
}
