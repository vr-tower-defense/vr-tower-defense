using UnityEngine;

public class EnemySplit : MonoBehaviour
{
    public GameObject NewEnemy;
    public int SplitAmount = 2;

    void OnDie()
    {
        for (int i = 0; i < SplitAmount; i++)
        {
            Instantiate(
                NewEnemy,
                transform.position,
                Random.rotation
            );
        }
    }
}
