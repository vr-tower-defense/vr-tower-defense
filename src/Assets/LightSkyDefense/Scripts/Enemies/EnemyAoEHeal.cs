using UnityEngine;

public class EnemyAoEHeal : MonoBehaviour
{
    public float HealAmount = 0.1f;
    public float HealRange = 0.2f;
    public float HealInterval = 3;

    // Start is called before the first frame update
    private void OnEnable()
    {
        InvokeRepeating("HealEnemies", 0, HealInterval);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void HealEnemies()
    {
        var enemiesInRange = Physics.OverlapSphere(transform.position, HealRange, (int)Layers.Enemies);

        foreach (Collider enemy in enemiesInRange)
        {
            enemy.GetComponent<Damageable>()?.SendMessage("UpdateHealth", HealAmount);
        }
    }
}