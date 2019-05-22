using System.Collections;
using UnityEngine;

public abstract class WaveH : MonoBehaviour
{
    public abstract IEnumerator Spawn();

    [Tooltip("The Amount of times this wave will repeat")]
    public int Repeat = 1;

    [Tooltip("The time, in seconds, between each spawn of this wave")]
    public float Delay = 1;
}
