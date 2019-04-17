﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class Credit : MonoBehaviour
{
    public int value = 5;
    public AudioClip pickupSound;


    /// <summary>
    /// When the credit touches the hand, the credit gets added to the players total credit count
    /// </summary>
    /// <param name="hand"></param>
    private void OnHandHoverBegin(Hand hand)
    {
        var creditOwner = Player.instance.gameObject.GetComponent<CreditOwner>();

        if (creditOwner == null) { return; }

       
        creditOwner.AddCredits(value);
        AudioSource.PlayClipAtPoint(pickupSound, this.gameObject.transform.position);

        Destroy(gameObject);
    }


}
