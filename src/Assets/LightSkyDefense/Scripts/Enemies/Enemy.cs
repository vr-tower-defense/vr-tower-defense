using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Damageable))]
[RequireComponent(typeof(SpawnCreditOnDie))]
public class Enemy : MonoBehaviour
{
    [Header("Die effect")]

    public float ExplodePitch;
    public float ExplodePitchVariation;
    public ParticleSystem ExplodeEffect;
    public AudioClip ExplodeSound;

    public void OnReachEndOfPath()
    {
        var playerStatistics = Player.instance.GetComponent<PlayerStatistics>();

        // Reduce player lives
        playerStatistics.UpdateLives(-1);

        // Destroy enemy
        Destroy(gameObject);
    }

    /// <summary>
    /// This kills the enemy and starts the explosion particle system and sound effect
    /// </summary>
    public void OnDie()
    {
        // If not in the world, instantiate
        var explodeEffectInstance = Instantiate(
            ExplodeEffect,
            transform.position,
            transform.rotation
        );

        // Play effect
        explodeEffectInstance.Play();

        // Play sound effect
        SoundUtil.PlayClipAtPointWithRandomPitch(
            ExplodeSound,
            transform.position,
            ExplodePitch - ExplodePitchVariation,
            ExplodePitch + ExplodePitchVariation
        );

        // Destroy after particle (emit) duration + maximum particle lifetime
        Destroy(
            explodeEffectInstance.gameObject,
            explodeEffectInstance.main.duration + explodeEffectInstance.main.startLifetime.constantMax
        );
    }
}