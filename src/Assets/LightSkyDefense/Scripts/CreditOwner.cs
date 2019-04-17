using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditOwner : MonoBehaviour
{
    private int Credits;

    public void AddCredits(int val)
    { 
        Credits += val;
    }
}
