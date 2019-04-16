using System.Collections;
using UnityEngine;

namespace Assets
{
    public class TowerAI : MonoBehaviour
    {
        public Rigidbody Projectile;
        public Transform ProjectileSpawn;
        public Transform Target = null;
        private IEnumerator _coroutine;
        public float ReloadTime = 2;
        public float BulletSpeed = 5;

        void OnTriggerEnter(Collider target)
        {
            Enemy enemyScript = target.gameObject.GetComponent<Enemy>();

            if (enemyScript == null) return;

            Target = target.transform;
        }

        void OnTriggerExit(Collider target)
        {
            Enemy enemyScript = target.gameObject.GetComponent<Enemy>();

            if (enemyScript == null) return;

            Target = null;
        }


        // Use this for initialization
        void Start()
        {
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
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
