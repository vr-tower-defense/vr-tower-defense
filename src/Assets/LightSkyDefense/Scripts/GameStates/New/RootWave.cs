using UnityEngine;

public class RootWave : MonoBehaviour
{
    public WaveH Root;
    private Coroutine _spawn;

    public void Start()
    {
        _spawn = StartCoroutine(Root.Spawn());
    }
}
