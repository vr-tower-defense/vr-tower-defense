using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ISteering : ScriptableObject
{
    public abstract void Initialize(GameObject gameObject);

    public abstract Vector3 Calculate(GameObject gameObject);

    public abstract void DrawGizmos(GameObject gameObject);
}
