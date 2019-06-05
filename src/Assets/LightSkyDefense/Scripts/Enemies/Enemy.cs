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

    [Header("Heal effect")]
    public GameObject HealEffect;

    [Tooltip("Earth layer mask")]
    public LayerMask CollisionLayerMask = (int)Layers.EndGoal;

    void OnCollisionEnter(Collision collision)
    {
        if (CollisionLayerMask >> collision.collider.gameObject.layer == 1)
        {
            var playerStatistics = Player.instance.GetComponent<PlayerStatistics>();

            // Reduce player lives
            playerStatistics.UpdateLives(-1);

            // Destroy enemy
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// This kills the enemy and starts the explosion particle system and sound effect
    /// </summary>
    public void OnDie()
    {
        // If not in the world, instantiate
        Instantiate(
            ExplodeEffect,
            transform.position,
            transform.rotation
        );

        // Play sound effect
        SoundUtil.PlayClipAtPointWithRandomPitch(
            ExplodeSound,
            transform.position,
            ExplodePitch - ExplodePitchVariation,
            ExplodePitch + ExplodePitchVariation
        );
    }

    public void OnUpdateHealth(float amount)
    {
        if (amount > 0)
        {
            var animationPrefab = Instantiate(
                HealEffect,
                transform
            );

            var animation = animationPrefab.GetComponent<Animation>();

            Destroy(animationPrefab, animation.clip.averageDuration);
        }
    }
}