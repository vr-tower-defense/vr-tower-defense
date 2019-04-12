using UnityEngine;

namespace Assets
{
    public class BulletShootScript : MonoBehaviour
    {
        public Transform BulletSpawn;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {

                clone.velocity = BulletSpawn.TransformDirection(Vector3.forward * 20);
            }
        }
    }
}
