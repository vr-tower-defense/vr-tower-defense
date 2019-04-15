using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBullet : MonoBehaviour
{
    public int BulletDamage;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        Enemy enemyScript = collision.gameObject.GetComponent<Enemy>();
        if (enemyScript == null) return;
        enemyScript.Damage(BulletDamage);
        Destroy(gameObject);
    }
}
