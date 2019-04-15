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
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemyScript = GameObject.FindObjectOfType(typeof(Enemy)) as Enemy;
            enemyScript.Damage(BulletDamage);
            Destroy(gameObject);
        }
    }
}
