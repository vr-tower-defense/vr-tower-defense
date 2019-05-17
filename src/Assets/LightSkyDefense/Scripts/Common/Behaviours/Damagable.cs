using UnityEngine;

class Damagable : MonoBehaviour
{
    [SerializeField]
    [Min(0)]
    private float MaxHealth = 1;

    [SerializeField]
    [Min(0)]
    private float InitialHealth = 1;

    /// <summary>
    /// Returns the current health
    /// </summary>
    [HideInInspector]
    public float Health { get; private set; }

    /// <summary>
    /// Set initial health to health value
    /// </summary>
    void Start()
    {
        Health = InitialHealth;
    }

    /// <summary>
    /// Decrease or increase health until health is below 0
    /// </summary>
    /// <param name="amount"></param>
    public void UpdateHealth(float amount)
    {
        Health = Mathf.Max(Health + amount, MaxHealth);

        if (Health <= 0)
        {
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
