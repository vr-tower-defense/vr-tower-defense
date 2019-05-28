using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Cooldown", menuName = "Waves/Cooldown", order = 1)]
public class WaveCooldown : ScriptableObject
{
    [Min(0)]
    public float Cooldown = 1;

    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(Cooldown);
    }
}