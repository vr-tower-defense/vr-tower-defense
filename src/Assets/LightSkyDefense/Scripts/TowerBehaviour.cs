using System.Collections;
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

        void OnTriggerEnter(Collider target)
        {
            var enemyScript = target.gameObject.GetComponent<Enemy>();

            if (enemyScript == null) return;

            Target = target.transform;
        }

        void OnTriggerExit(Collider target)
        {
            var enemyScript = target.gameObject.GetComponent<Enemy>();

            if (enemyScript == null) return;

            Target = null;
        }

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

        IEnumerator Reload(float waitTime)
        {
            while (true)
            {
                AimAndShoot();
                yield return new WaitForSeconds(waitTime);
            }
        }

        void RotateToEnemy()
        {
            transform.LookAt(Target);

            Vector3 eulerAngles = transform.rotation.eulerAngles;
            eulerAngles.x = 0;
            eulerAngles.z = 0;

            transform.rotation = Quaternion.Euler(eulerAngles);
        }

        void AimAndShoot()
        {
            if (Target == null) return;

            RotateToEnemy();

            var newProjectile = (Rigidbody)Instantiate(Projectile, ProjectileSpawn.position, Projectile.rotation);
            newProjectile.velocity = (Target.transform.position - transform.position).normalized * BulletSpeed;

            _source.Play();

        }
    }
}
