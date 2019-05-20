using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [Tooltip("The amount of damage that is applied to a target that collides with this gameObject")]
    public float BulletDamage;

    [Tooltip("The time before a bullet is removed from the scene")]
    public float TimeAlive = 3;

    // The bullets own rigidbody
    public Rigidbody _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

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

        Destroy(gameObject);
    }
}