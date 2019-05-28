using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Waves/Wave", order = 1)]
public class Wave : ScriptableObject
{
    public WaveStep[] Steps;

    public IEnumerator Start()
    {
        foreach (var gameObject in FindObjectsOfType<GameObject>())
        {
            gameObject.BroadcastMessage(
                "OnWaveStart",
                SendMessageOptions.DontRequireReceiver
            );
        }

        foreach (var step in Steps)
        {
            yield return step.Run();
        }
    }
}
