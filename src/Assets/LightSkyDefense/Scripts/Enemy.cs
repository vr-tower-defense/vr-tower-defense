using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float MaxHealth = 100f;

    private float _health = 100f;
    private float _energy = 100f;

    public ParticleSystem ExplodeEffect;
    private ParticleSystem _explodeEffectInstance = null;

    void Start()
    {

    }

    void FixedUpdate()
    {
        if (_energy < 0)
        {
            Damage(30);
        }

        //todo: intergrate  behaviour

        //todo: update engery

        //todo: if end reached then ethernal pleasure and happiness
    }

    public float GetHealth()
    {
        return _health;
    }

    /// <summary>
    /// This function will remove the specified dmgAmount from the enemy's health
    /// </summary>
    /// <param name="damageAmount"></param>
    public void Damage(float damageAmount)
    {
        _health -= damageAmount;
        if (_health <= 0)
        {
            Explode();
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// This function will add the specified healtAmount to the enemy's health
    /// </summary>
    /// <param name="healAmount"></param>
    public void Heal(float healAmount)
    {
        _health = Mathf.Clamp(_health + healAmount, 0, MaxHealth);
    }

    /// <summary>
    /// This function kills the enemy and starts the explosion particle system
    /// </summary>
    public void Explode()
    {
        //if not in the world, instantiate
        if (_explodeEffectInstance == null)
        {
            _explodeEffectInstance = Instantiate(ExplodeEffect, transform.position, new Quaternion());
        }
        //play effect
        _explodeEffectInstance.Play();
        //Destroy after particle (emit) duration + maximum particle lifetime
        Destroy(_explodeEffectInstance.gameObject, (_explodeEffectInstance.main.duration + _explodeEffectInstance.main.startLifetime.constantMax));

        //Kill enemy (if Explode() called when the enemy was still alive)
        Destroy(gameObject);
    }
}
