using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float health = 100f;
    private float energy = 100f;

    //private EnemyBehaviour behaviour;
        
    void Start()
    {
        
    }
    
    void Update()
    {
        if(energy >0)
        {

        }
        else
        {
            health -= 30;
        }

        if (health<=0)
        {
            Destroy(this.gameObject);
        }

        //todo: intergrate  behaviour

        //todo: update engery

        //todo: if end reached then ethernal pleasure and happiness
    }

    public float GetHealth()
    {
        return health;
    }

    public void SetHealth(float newHealth)
    {
        health = newHealth;
    }

    
    /// <summary>
    /// This function will remove the specified dmgAmount from the enemy's health
    /// </summary>
    /// <param name="dmgAmount"></param>
    public void Damage(float dmgAmount)
    {
        health -= dmgAmount;
    }

    /// <summary>
    /// This function will add the specified healtAmount to the enemy's health
    /// </summary>
    /// <param name="healAmount"></param>
    public void Heal(float healAmount)
    {
        if(health+healAmount>100)
        {
            health = 100;
        }
        else
        {
            health += healAmount;
        }
    }

}
