using UnityEngine;

public class EnemyAoEHeal : MonoBehaviour
{
    public float HealAmount = 0.1f;
    public float HealRadius = 0.2f;
    public float HealInterval = 3;

    /// <summary>
    /// Update health of enemies in HealRadius every HealInterval
    /// </summary>
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
        var enemiesInRange = Physics.OverlapSphere(
            transform.position,
            HealRadius,
            (int)Layers.Enemies
        );

        foreach (Collider enemy in enemiesInRange)
        {
            enemy.GetComponent<Damageable>()?.SendMessage("UpdateHealth", HealAmount);
        }
    }

    #region debugging

    /// <summary>
    /// Vizualize the heal radius
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.1f);
        Gizmos.DrawSphere(transform.position, HealRadius);
    }

    #endregion
}