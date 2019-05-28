using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Waves/Wave", order = 1)]
public class Wave : ScriptableObject
{
    public UnityEngine.Object[] Steps;

    public IEnumerator Start()
    {
        foreach (var gameObject in FindObjectsOfType<GameObject>())
        {
            gameObject.BroadcastMessage(
                "OnWaveStarted",
                SendMessageOptions.DontRequireReceiver
            );
        }

        foreach (var step in Steps)
        {
            if (step is CooldownStep)
            {
                yield return ((CooldownStep)step).Wait();
                continue;
            }

            Instantiate(
                step,
                GameManager.Instance.Path.PathPoints[0],
                Quaternion.identity
            );
        }
    }
}
