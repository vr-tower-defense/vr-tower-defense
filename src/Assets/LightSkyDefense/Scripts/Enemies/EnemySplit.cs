using UnityEngine;

public class EnemySplit : MonoBehaviour
{
    public GameObject NewEnemy;
    public int SplitAmount = 2;

    void OnDestroy()
    {
        for (int i = 0; i < SplitAmount; i++)
        {
            var leftSplitEnemy = Instantiate(
                NewEnemy,
                transform.position,
                Random.rotation
            );

            leftSplitEnemy.gameObject.GetComponent<Damageable>().UpdateHealth(-0.6f);
        }
    }
}
