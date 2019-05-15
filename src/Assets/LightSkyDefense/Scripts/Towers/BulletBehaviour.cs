using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [Tooltip("The amount of damage that is applied to a target that collides with this gameObject")]
    public float BulletDamage;

    [Tooltip("The time before a bullet is removed from the scene")]
    public float TimeAlive = 3;

    [Tooltip("A higher mass will increase the knockback amount")]
    public float Mass = 1;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, TimeAlive);
    }

    void OnCollisionEnter(Collision collision)
    {
        var rigidbody = collision
            .gameObject
            .GetComponent<Rigidbody>();

        var damagable = collision
            .gameObject
            .GetComponent<Damagable>();

        // Decrease target health
        damagable?.UpdateHealth(-BulletDamage);

        // Apply knockback effect to enemy
        rigidbody?.AddForce(
            -collision.GetContact(0).normal * Mass, 
            ForceMode.Impulse
        );

        Destroy(gameObject);
    }
}