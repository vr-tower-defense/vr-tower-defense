using UnityEngine;

public class EnemyAoEHeal : MonoBehaviour
{
    public float HealAmount = 0.1f;
    public float HealInterval = 3;

    // Start is called before the first frame update
    private void Start()
    {
        AoEHealWithInterval();
    }

    private void AoEHealWithInterval()
    {
        HealEnemies(transform.position, 0.15f, HealAmount);
        Invoke("AoEHealWithInterval", HealInterval);
    }

    private static void HealEnemies(Vector3 center, float radius, float amount)
    {
        var enemiesInRange = Physics.OverlapSphere(center, radius);

        foreach (Collider enemy in enemiesInRange)
        {
            if (enemy.GetComponent<Damageable>() != null)
            {
                enemy.GetComponent<Damageable>().SendMessage("UpdateHealth", amount);
            }
        }
    }
}