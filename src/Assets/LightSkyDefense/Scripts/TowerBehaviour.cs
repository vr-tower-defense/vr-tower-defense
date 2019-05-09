using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Assets
{
    public class TowerBehaviour : MonoBehaviour, IOnGameLossTarget, IOnGameWinTarget
    {
        private readonly HashSet<Collider> _enemySet = new HashSet<Collider>();
        
        private IEnumerator _coroutine;
        private AudioSource _source;
        private float _health = 100f;
        
        public int Cost;
        public float ProjectileSpeed = 10;
        public float RotationSpeed = 1;
        public float ShootInterval = 3;
        public float MaxHealth = 100f;

        public AudioClip BuildSound;
        private Rigidbody ActiveTarget;
        public Rigidbody Projectile;
        public Transform ProjectileSpawn;

        /// <summary>
        /// Use this for initialization
        /// </summary>
        private void Start()
        {
            _source = GetComponent<AudioSource>();
            var creditOwner = Player.instance.GetComponent<PlayerStats>();

            if (creditOwner.Credits < Cost)
            {
                Destroy(gameObject);
                return;
            }

            _source?.PlayOneShotWithRandomPitch(BuildSound, 0.5f, 1.5f);

            creditOwner.Credits -= Cost;

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

            //If it's not an enemy, then there's no reason to keep going
            if (enemyScript == null) return;

            _enemySet.Add(target);
        }

        /// <summary>
        /// Called when a colliding gameobject leaves the tower's detection radius
        /// </summary>
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
            if (ActiveTarget == null) return;

            var newProjectile = Instantiate(
                Projectile, 
                ProjectileSpawn.position,
                ProjectileSpawn.rotation
            );

            newProjectile.velocity = transform.forward * ProjectileSpeed;

            _source.PlayWithRandomPitch(0.5f,1.5f);
        }

        /// <summary>
        /// Used to rotate towards an enemy on the Y-axis before shooting.
        /// </summary>
        private void RotateTowardsEnemy()
        {
            var hasTarget = FindTarget(out var targetTransform);

            // Update new active target
            ActiveTarget = targetTransform;

            // Find first target in list
            if (!hasTarget) return;

            var targetDistance = Vector3.Distance(transform.position, ActiveTarget.position);
            var traveltime = targetDistance / ProjectileSpeed;

            var targetDisplacement = ActiveTarget.velocity * traveltime;

            var predictedlookRotation = Quaternion.LookRotation(
                (ActiveTarget.position + targetDisplacement) - transform.position,
                Vector3.forward
            );

            // Rotate our transform a step closer to the target's.
            transform.rotation = Quaternion.RotateTowards(transform.rotation, predictedlookRotation, RotationSpeed);
        }

        public void Damage(float damageAmount)
        {
            _health -= damageAmount;

            if (_health > 0) return;

            Destroy(gameObject);
        }

        public void OnGameLoss()
        {
            StopCoroutine(_coroutine);
            enabled = false;
        }

        public void OnGameWin()
        {
            gameObject.AddComponent<WinCelebrationBehaviour>();
        }
    }
}