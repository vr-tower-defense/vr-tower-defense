using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class TowerBehaviour : MonoBehaviour
    {
        private readonly HashSet<Collider> _enemySet = new HashSet<Collider>();

        private IEnumerator _coroutine;
        private Collider _focusEnemy;
        private AudioSource _source;

        public float BulletSpeed = 5;
        public Rigidbody Projectile;
        public Transform ProjectileSpawn;

        public float ReloadTime = 3;
        public Transform Target;

        /// <summary>
        ///     Called whenever a colliding gameobject enters the tower's detection radius
        /// </summary>
        private void OnTriggerEnter(Collider target)
        {
            var enemyScript = target.gameObject.GetComponent<Enemy>();

            ///If it's not an enemy, then there's no reason to keep going
            if (enemyScript == null) return;

            _enemySet.Add(target);

            ///If the tower is already shooting at a Target, there's no need to change his Target
            if (Target != null) return;

            Target = target.transform;
            _focusEnemy = target;
        }

        /// <summary>
        /// Called when a colliding gameobject leaves the tower's detection radius
        /// </summary>
        private void OnTriggerExit(Collider target)
        {
            var enemyScript = target.gameObject.GetComponent<Enemy>();

            ///If it's not an enemy, then there's no reason to keep going
            if (enemyScript == null) return;

            _enemySet.Remove(target);

            ///Checks for the enemy that came in closest after his last Target.
            foreach (var c in _enemySet)
                if (c != null)
                {
                    Target = c.transform;
                    _focusEnemy = c;
                    return;
                }

            ///If there are no enemies left in the tower's radius, reset his target and clear his list of enemies.
            Target = null;
            _focusEnemy = null;
            _enemySet.Clear();
        }

        /// <summary>
        /// Called to check if the tower's current target is dead, and if so, pick the next target.
        /// </summary>
        private void PickNextAfterTargetDies()
        {
            if (_focusEnemy) return;

            ///Checks for the enemy that came in closest after his last Target.
            foreach (var c in _enemySet)
                if (c != null)
                {
                    Target = c.transform;
                    _focusEnemy = c;
                    return;
                }

            ///If there are no enemies left in the tower's radius, reset his target and clear his list of enemies.
            Target = null;
            _focusEnemy = null;
            _enemySet.Clear();
        }

        /// <summary>
        /// Called if the tower gets destroyed.
        /// </summary>
        private void OnDisable()
        {
            StopCoroutine(_coroutine);
        }


        /// <summary>
        /// Use this for initialization
        /// </summary>
        private void Start()
        {
            _source = GetComponent<AudioSource>();
            _coroutine = Reload(ReloadTime);

            StartCoroutine(_coroutine);
        }

        /// <summary>
        /// While the tower is alive and there is enemies nearby, the tower will aim, shoot and then wait for a small period of time.
        /// </summary>
        private IEnumerator Reload(float waitTime)
        {
            while (true)
            {
                AimAndShoot();
                yield return new WaitForSeconds(waitTime);
            }
        }

        /// <summary>
        /// Used to rotate towards an enemy on the Y-axis before shooting.
        /// </summary>
        private void RotateToEnemy()
        {
            var speed = 8;
            var direction = Target.position - transform.position;
            direction.y = 0;
            var toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, speed * Time.deltaTime);
        }

        /// <summary>
        ///     If there's no target, don't shoot, else, aim and shoot at the target (and play the sound effect).
        /// </summary>
        private void AimAndShoot()
        {
            if (Target == null) return;

            var newProjectile = Instantiate(Projectile, ProjectileSpawn.position, Projectile.rotation);
            newProjectile.velocity = (Target.transform.position - transform.position).normalized * BulletSpeed;

            _source.Play();
        }

        private void Update()
        {
            PickNextAfterTargetDies();
        }

        private void FixedUpdate()
        {
            if (Target == null) return;

            RotateToEnemy();
        }
    }
}