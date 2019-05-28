using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "CooldownStep", menuName = "Waves/CooldownStep", order = 1)]
public class CooldownStep : WaveStep
{
    [Min(0)]
    public float TimeInSeconds = 1;

    public override IEnumerator Run()
    {
        yield return new WaitForSeconds(TimeInSeconds);
    }
}