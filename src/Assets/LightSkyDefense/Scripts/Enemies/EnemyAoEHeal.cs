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

    private void HealEnemies(Vector3 center, float radius, float amount)
    {
        var enemiesInRange = Physics.OverlapSphere(center, radius);

        if (enemiesInRange.Length < 1) return;

        for (var i = 0; i < enemiesInRange.Length; i++)
        {
            if (enemiesInRange[i].gameObject.GetComponent<Damageable>() != null)
            {
                enemiesInRange[i].gameObject.GetComponent<Damageable>().SendMessage("UpdateHealth", amount);
            }
        }
    }
}