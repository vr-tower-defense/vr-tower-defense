using UnityEngine;

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

    public int Damage = 1;

    /// <summary>
    /// This kills the enemy and starts the explosion particle system and sound effect
    /// </summary>
    public void OnDie()
    {
        InstantiateExplode();

        Destroy(gameObject);
    }

    public void OnFinish()
    {
        InstantiateExplode();

        Destroy(gameObject);
    }

    private void InstantiateExplode()
    {
        Instantiate(
            ExplodeEffect,
            transform.position,
            transform.rotation
        );

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