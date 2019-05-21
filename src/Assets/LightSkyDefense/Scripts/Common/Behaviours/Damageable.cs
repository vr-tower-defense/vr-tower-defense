using UnityEngine;

class Damageable : MonoBehaviour
{
    [Min(0)]
    public float MaxHealth = 1;

    [Min(0)]
    public float InitialHealth = 1;

    /// <summary>
    /// Returns the current health
    /// </summary>
    [HideInInspector]
    public float Health { get; private set; }

    /// <summary>
    /// Set initial health to health value
    /// </summary>
    void Awake()
    {
        Health = InitialHealth;
    }

    /// <summary>
    /// Decrease or increase health until health is below 0
    /// </summary>
    /// <param name="amount"></param>
    public void UpdateHealth(float amount)
    {
        Health = Mathf.Min(Health + amount, MaxHealth);

        if (Health <= 0)
        {
            gameObject.BroadcastMessage(
                "OnDie",
                null,
                SendMessageOptions.DontRequireReceiver
            );

            Destroy(gameObject);
            return;
        }

        gameObject.BroadcastMessage(
            "OnUpdateHealth",
            amount,
            SendMessageOptions.DontRequireReceiver
        );
    }
}
