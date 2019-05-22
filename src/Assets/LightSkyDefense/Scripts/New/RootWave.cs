using UnityEngine;

public class RootWave : MonoBehaviour
{
    public MonoBehaviour Root;
    private Coroutine _spawn;
    public void Start()
    {
        if (Root is IWaveH Wave) 
            _spawn = StartCoroutine(Wave.Spawn());
        
    }
}

