﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditOwner : MonoBehaviour
{
    private int credits;

    public void AddCredits(int val)
    { 
        credits += val;
    }
}
