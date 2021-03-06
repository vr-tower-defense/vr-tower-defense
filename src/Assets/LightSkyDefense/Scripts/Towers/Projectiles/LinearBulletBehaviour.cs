﻿using UnityEngine;

public class LinearBulletBehaviour : MonoBehaviour
{
    [Tooltip("The amount of damage that is applied to a target that collides with this gameObject")]
    public float BulletDamage = 1;

    [Tooltip("The time before a bullet is removed from the scene")]
    public float TimeAlive = 3;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, TimeAlive);
    }

    void OnCollisionEnter(Collision collision)
    {
        var damagable = collision
            .gameObject
            .GetComponent<Damageable>();

        // Decrease target health
        damagable?.UpdateHealth(-BulletDamage);

        Destroy(gameObject);
    }
}