using UnityEngine;

public class EnemyAoEHeal : MonoBehaviour
{
    [Tooltip("The amount of heal that is added every HealInterval")]
    public float HealAmount = 0.1f;

    [Tooltip("The range in which an enemy should be to receive health")]
    public float HealRadius = 0.2f;

    [Tooltip("The time between each heal")]
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