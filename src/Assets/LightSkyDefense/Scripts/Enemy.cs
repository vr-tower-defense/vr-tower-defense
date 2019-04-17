using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float health = 100f;
    private float energy = 100f;

    public ParticleSystem explodeEffect;
    private ParticleSystem explodeEffectInstance = null;

    void Start()
    {

    }

    void FixedUpdate()
    {
        if (energy > 0)
        {

        }
        else
        {
            Damage(30);
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
        if (health <= 0)
        {
            Explode();
            Destroy(this.gameObject);
        }
    }


    /// <summary>
    /// This function will remove the specified dmgAmount from the enemy's health
    /// </summary>
    /// <param name="dmgAmount"></param>
    public void Damage(float dmgAmount)
    {
        health -= dmgAmount;
        if (health <= 0)
        {
            Explode();
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// This function will add the specified healtAmount to the enemy's health
    /// </summary>
    /// <param name="healAmount"></param>
    public void Heal(float healAmount)
    {
        if (health + healAmount > 100)
        {
            health = 100;
        }
        else
        {
            health += healAmount;
        }
    }

    /// <summary>
    /// This function kills the enemy and starts the explosion particle system
    /// </summary>
    public void Explode()
    {
        //if not in the world, instantiate
        //if(explodeEffect.scene.name == null)
        if (explodeEffectInstance == null)
        {
            explodeEffectInstance = Instantiate(explodeEffect, this.transform.position, new Quaternion());
        }
        //play effect
        explodeEffectInstance.Play();
        //Destroy after particle (emit) duration + maximum particle lifetime
        Destroy(explodeEffectInstance.gameObject, (explodeEffectInstance.main.duration + explodeEffectInstance.main.startLifetime.constantMax));

        //Kill enemy (if Explode() called when the enemy was still alive)
        Destroy(this.gameObject);
    }
}
