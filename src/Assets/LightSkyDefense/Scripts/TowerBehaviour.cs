using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Assets
{
    public class TowerBehaviour : MonoBehaviour
    {
        private readonly HashSet<Collider> _enemySet = new HashSet<Collider>();
        
        private IEnumerator _shootWithIntervalCoroutine;
        private AudioSource _source;
        private Rigidbody _activeTarget;
        private Quaternion _idleRotation;
        private float _health = 100f;

        public int Cost;
        public float ProjectileSpeed = 10;
        public float RotationSpeed = 1;
        public float IdleRotationSpeed = .1f;
        public float ShootInterval = 3;
        public float MaxHealth = 100f;

        public AudioClip BuildSound;
        public Rigidbody Projectile;
        public Transform ProjectileSpawn;

        #region lifecycle methods

        /// <summary>
        /// Use this for initialization
        /// </summary>
        private void Start()
        {
            _source = GetComponent<AudioSource>();
            var creditOwner = Player.instance.GetComponent<CreditOwner>();

            if (creditOwner.Credits < Cost)
            {
                Destroy(gameObject);
                return;
            }

            _source?.PlayOneShot(BuildSound);
            creditOwner.Credits -= Cost;

            StartCoroutine(ShootCoroutine(ShootInterval));
        }

        /// <summary>
        /// Used to rotate towards an enemy on the Y-axis before shooting.
        /// </summary>
        private void FixedUpdate()
        {
            var hasTarget = FindTarget(out var targetTransform);

            // Update new active target
            _activeTarget = targetTransform;

            // Find first target in list
            if (!hasTarget)
            {
                if (
                    _idleRotation.x == default ||
                    Mathf.Abs(transform.rotation.eulerAngles.sqrMagnitude - _idleRotation.eulerAngles.sqrMagnitude) < 1
                )
                {
                    var randomRotation = Random.rotation;

                    _idleRotation = new Quaternion(
                        randomRotation.x, 
                        randomRotation.y, 
                        randomRotation.z, 
                        randomRotation.w
                    );
                }

                // Rotate turret when idle
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _idleRotation, IdleRotationSpeed);

                return;
            }

            // Calculate predicted target position
            var targetDistance = Vector3.Distance(transform.position, _activeTarget.position);
            var traveltime = targetDistance / ProjectileSpeed;

            var targetDisplacement = _activeTarget.velocity * traveltime;

            var predictedlookRotation = Quaternion.LookRotation(
                (_activeTarget.position + targetDisplacement) - transform.position,
                Vector3.forward
            );

            // Rotate our transform a step closer to the target's.
            transform.rotation = Quaternion.RotateTowards(transform.rotation, predictedlookRotation, RotationSpeed);
        }

        #endregion

        #region  event handlers

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

        #endregion

        #region coroutines

        /// <summary>
        /// While the tower is alive and there are enemies nearby, the tower will aim, shoot and then wait for a small period of time.
        /// </summary>
        private IEnumerator ShootCoroutine(float waitTime)
        {
            while (true)
            {
                ShootProjectile();
                yield return new WaitForSeconds(waitTime);
            }
        }

        #endregion

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

            _source.Play();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="damageAmount"></param>
        public void ReceiveDamage(float damageAmount)
        {
            _health -= damageAmount;

            if (_health > 0) return;

            Destroy(gameObject);
        }
    }
}