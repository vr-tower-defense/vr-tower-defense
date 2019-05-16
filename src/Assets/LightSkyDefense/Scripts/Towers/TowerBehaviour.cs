using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Assets
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(AudioSource))]
    public class TowerBehaviour : MonoBehaviour
    {
        public float ProjectileSpeed = 10;
        public float RotationSpeed = 1;
        public float ShootInterval = 3;
        public float MaxHealth = 100f;

        public AudioSource AudioSource;
        public AudioClip BuildSound;
        public AudioClip ShootSound;
        public Rigidbody Projectile;
        public Transform ProjectileSpawn;

        private IEnumerator _coroutine;
        private float _health;

        private readonly HashSet<Collider> _enemySet = new HashSet<Collider>();
        private Rigidbody _activeTarget;

        /// <summary>
        /// Use this for initialization
        /// </summary>
        private void Start()
        {
            AudioSource?.PlayOneShotWithRandomPitch(BuildSound, 0.5f, 1.5f);

            // Start new coroutine to shoot projectiles
            _coroutine = ShootWithInterval(ShootInterval);
            StartCoroutine(_coroutine);
        }

        /// <summary>
        /// Called whenever a colliding gameobject enters the tower's detection radius
        /// </summary>
        private void OnTriggerEnter(Collider target)
        {
            var enemyScript = target.gameObject.GetComponent<Enemy>();

            // If it's not an enemy, then there's no reason to keep going
            if (enemyScript == null) return;

            _enemySet.Add(target);
        }

        private void OnTriggerExit(Collider target)
        {
            var enemyScript = target.gameObject.GetComponent<Enemy>();

            // If it's not an enemy, then there's no reason to keep going
            if (enemyScript == null) return;

            _enemySet.Remove(target);
        }


        /// <summary>
        /// Called every 16 ms.
        /// </summary>
        private void FixedUpdate()
        {
            RotateTowardsEnemy();
        }

        /// <summary>
        /// While the tower is alive and there are enemies nearby, the tower will aim, shoot and then wait for a small period of time.
        /// </summary>
        private IEnumerator ShootWithInterval(float waitTime)
        {
            while (true)
            {
                ShootProjectile();
                yield return new WaitForSeconds(waitTime);
            }
        }

        /// <summary>
        /// Finds first target in list
        /// </summary>
        /// <returns></returns>
        private bool FindTarget(out Rigidbody targetTransform)
        {
            // Checks for the enemy that came in closest after his last Target.
            foreach (var targetCollider in _enemySet)
            {
                if (targetCollider == null) continue;

                targetTransform = targetCollider.GetComponent<Rigidbody>();
                return true;
            }

            targetTransform = null;
            return false;
        }

        /// <summary>
        /// If there's no target, don't shoot, else, aim and shoot at the target (and play the sound effect).
        /// </summary>
        private void ShootProjectile()
        {
            if (_activeTarget == null) return;

            var newProjectile = Instantiate(
                Projectile,
                ProjectileSpawn.position,
                ProjectileSpawn.rotation
            );

            newProjectile.velocity = transform.forward * ProjectileSpeed;

            AudioSource?.PlayOneShotWithRandomPitch(ShootSound, 0.5f, 1.5f);
        }

        /// <summary>
        /// Used to rotate towards an enemy on the Y-axis before shooting.
        /// </summary>
        private void RotateTowardsEnemy()
        {
            var hasTarget = FindTarget(out var targetTransform);

            // Update new active target
            _activeTarget = targetTransform;
            
            // Don't rotate when target does not exist
            if (!hasTarget) return;

            var targetDistance = Vector3.Distance(
                transform.position,
                _activeTarget.position
            );

            var travelTime = targetDistance / ProjectileSpeed;
            var targetDisplacement = _activeTarget.velocity * travelTime;

            var predictedlookRotation = Quaternion.LookRotation(
                (_activeTarget.position + targetDisplacement) - transform.position,
                Vector3.forward
            );

            // Rotate our transform a step closer to the target's.
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                predictedlookRotation, 
                RotationSpeed
            );
        }
    }
}