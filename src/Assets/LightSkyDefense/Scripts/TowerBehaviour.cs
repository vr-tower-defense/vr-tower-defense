using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class TowerBehaviour : MonoBehaviour
    {
        private readonly HashSet<Collider> _enemySet = new HashSet<Collider>();

        private IEnumerator _coroutine;
        private AudioSource _source;

        public float ProjectileSpeed = 10;
        public float RotationSpeed = 1;
        public float ShootInterval = 3;

        public Transform ActiveTargetTransform;

        public Rigidbody Projectile;
        public Transform ProjectileSpawn;

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
        /// Use this for initialization
        /// </summary>
        private void Start()
        {
            _source = GetComponent<AudioSource>();

            // Start new coroutine to shoot projectiles
            _coroutine = ShootWithInterval(ShootInterval);
            StartCoroutine(_coroutine);
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
        private bool FindTarget(out Transform targetTransform)
        {
            // Checks for the enemy that came in closest after his last Target.
            foreach (var targetCollider in _enemySet)
            {
                if (targetCollider == null)
                {
                    continue;
                }

                targetTransform = targetCollider.transform;
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
            if(ActiveTargetTransform == null)
            {
                return;
            }

            var newProjectile = Instantiate(
                Projectile, 
                ProjectileSpawn.position, 
                Projectile.rotation
            );

            newProjectile.velocity = (ActiveTargetTransform.position - transform.position).normalized * ProjectileSpeed;

            _source.Play();
        }

        /// <summary>
        /// Used to rotate towards an enemy on the Y-axis before shooting.
        /// </summary>
        private void RotateTowardsEnemy()
        {
            var hasTarget = FindTarget(out var targetTransform);

            // Update new active target
            ActiveTargetTransform = targetTransform;

            // Find first target in list
            if (!hasTarget)
            {
                return;
            }


            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(targetTransform.position - transform.position),
                RotationSpeed * Time.deltaTime
            );
        }
    }
}