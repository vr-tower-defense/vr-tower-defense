using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour, IWaveH
{

    public Object _prefab;
    public int Delay = 500;
    public int Repeat = 1;

    public IEnumerator Spawn()
    {
        for (int i = 0; i < Repeat; i++)
        {
            Instantiate(_prefab);
            yield return new WaitForSeconds(Delay);
        }
    }
}