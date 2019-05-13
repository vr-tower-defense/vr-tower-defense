﻿using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float BulletDamage;
    public float TimeBeforeDestroy = 3;
    public float KnockBackAmount = 1;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, TimeBeforeDestroy);
    }

    void OnCollisionEnter(Collision collision)
    {
        Enemy enemyScript = collision.gameObject.GetComponent<Enemy>();

        if (enemyScript != null)
        {
            enemyScript.Damage(BulletDamage);
            enemyScript.Rigidbody.AddForce(-collision.GetContact(0).normal * KnockBackAmount, ForceMode.Impulse);
        }
        Destroy(gameObject);
    }
}