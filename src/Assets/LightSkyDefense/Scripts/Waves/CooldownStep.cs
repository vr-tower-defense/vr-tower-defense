using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "CooldownStep", menuName = "Waves/CooldownStep", order = 1)]
public class CooldownStep : ScriptableObject
{
    [Min(0)]
    public float TimeInSeconds = 1;

    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(TimeInSeconds);
    }
}