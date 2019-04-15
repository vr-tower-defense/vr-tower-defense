using UnityEngine;

namespace Assets
{
    public class ShootBullet : MonoBehaviour
    {
        public Rigidbody Projectile;
        public Transform ProjectileSpawn;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                var newProjectile = (Rigidbody)Instantiate(Projectile, ProjectileSpawn.position, Projectile.rotation);

                newProjectile.velocity = ProjectileSpawn.TransformDirection(Vector3.forward * 20);
            }
        }
    }
}
