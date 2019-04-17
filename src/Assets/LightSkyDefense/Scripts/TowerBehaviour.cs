using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets
{
    public class TowerBehaviour : MonoBehaviour
    {
        public Rigidbody Projectile;
        public Transform ProjectileSpawn;
        public Transform Target = null;

        public float ReloadTime = 3;
        public float BulletSpeed = 5;

        private IEnumerator _coroutine;
        private AudioSource _source;

        private readonly HashSet<Collider> _enemySet = new HashSet<Collider>();
        private Collider _focusEnemy;

        //Called whenever a colliding gameobject enters the tower's detection radius
        void OnTriggerEnter(Collider target)
        {
            var enemyScript = target.gameObject.GetComponent<Enemy>();

            //If it's not an enemy, then there's no reason to keep going
            if (enemyScript == null) return;

            _enemySet.Add(target);

            //If the tower is already shooting at a Target, there's no need to change his Target
            if (Target != null) return;

            Target = target.transform;
            _focusEnemy = target;
        }

        //Called when a colliding gameobject leaves the tower's detection radius
        void OnTriggerExit(Collider target)
        {
            var enemyScript = target.gameObject.GetComponent<Enemy>();

            //If it's not an enemy, then there's no reason to keep going
            if (enemyScript == null) return;

            _enemySet.Remove(target);

            //Checks for the enemy that came in closest after his last Target.
            foreach (Collider c in _enemySet)
            {
                if (c != null)
                {
                    Target = c.transform;
                    _focusEnemy = c;
                    return;
                }
            }

            //If there are no enemies left in the tower's radius, reset his target and clear his list of enemies.
            Target = null;
            _focusEnemy = null;
            _enemySet.Clear();
        }

        //Called to check if the tower's current target is dead, and if so, pick the next target.
        void PickNextAfterTargetDies()
        {
            if (_focusEnemy) return;

            //Checks for the enemy that came in closest after his last Target.
            foreach (Collider c in _enemySet)
            {
                if (c != null)
                {
                    Target = c.transform;
                    _focusEnemy = c;
                    return;
                }
            }

            //If there are no enemies left in the tower's radius, reset his target and clear his list of enemies.
            Target = null;
            _focusEnemy = null;
            _enemySet.Clear();
        }

        //Called if the tower gets destroyed.
        void OnDisable()
        {
            StopCoroutine(_coroutine);
        }


        // Use this for initialization
        void Start()
        {
            _source = GetComponent<AudioSource>();
            _coroutine = Reload(ReloadTime);

            StartCoroutine(_coroutine);
        }

        //While the tower is alive and there is enemies nearby, the tower will aim, shoot and then wait for a small period of time.
        IEnumerator Reload(float waitTime)
        {
            while (true)
            {
                AimAndShoot();
                yield return new WaitForSeconds(waitTime);
            }
        }

        //Used to rotate towards an enemy on the Y-axis before shooting.
        void RotateToEnemy()
        {
            var speed = 8;
            Vector3 direction = Target.position - transform.position;
            direction.y = 0;
            Quaternion toRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, speed * Time.deltaTime);
        }

        //If there's no target, don't shoot, else, aim and shoot at the target (and play the sound effect).
        void AimAndShoot()
        {
            if (Target == null) return;

            var newProjectile = (Rigidbody) Instantiate(Projectile, ProjectileSpawn.position, Projectile.rotation);
            newProjectile.velocity = (Target.transform.position - transform.position).normalized * BulletSpeed;

            _source.Play();
        }

        void Update()
        {
            PickNextAfterTargetDies();
        }

        void FixedUpdate()
        {
            if (Target == null) return;

            RotateToEnemy();
        }
    }
}