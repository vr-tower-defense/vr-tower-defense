using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Behaviour
{
    private GameObject gameObject;

    public abstract Vector3 Calculate();
}
