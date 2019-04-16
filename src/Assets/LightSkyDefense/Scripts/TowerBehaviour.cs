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
        private AudioSource source;

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
            source = GetComponent<AudioSource>();
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

        void AimAndShoot()
        {
            if (Target == null) return;

            var newProjectile = (Rigidbody)Instantiate(Projectile, ProjectileSpawn.position, Projectile.rotation);
            newProjectile.velocity = (Target.transform.position - transform.position).normalized * BulletSpeed;

            source.Play();

        }
    }
}
