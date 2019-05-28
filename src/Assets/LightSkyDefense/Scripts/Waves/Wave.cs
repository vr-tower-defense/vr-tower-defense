using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

[CreateAssetMenu(fileName = "Wave", menuName = "Waves/Wave", order = 1)]
public class Wave : ScriptableObject
{
    public UnityEngine.Object[] Steps;

    public IEnumerator Start()
    {
        FindObjectsOfType<GameObject>()
                .ForEach(obj => obj.BroadcastMessage(
                    "OnWaveStarted",
                    SendMessageOptions.DontRequireReceiver
                ));

        foreach (var step in Steps)
        {
            if (step.GetType() == typeof(WaveCooldown))
            {
                yield return ((WaveCooldown)step).Wait();
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
