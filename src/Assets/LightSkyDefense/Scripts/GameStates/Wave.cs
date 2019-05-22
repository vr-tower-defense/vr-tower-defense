using System.Collections;
using UnityEngine;


public abstract class Wave : ScriptableObject
{
    public abstract IEnumerator Spawn();

    [Tooltip("The Amount of times this wave will repeat")]
    [Min(0)]
    public int Repeat = 1;

    [Tooltip("The time, in seconds, between each spawn of this wave")]
    [Min(0)]
    public float Delay = 1;
}
