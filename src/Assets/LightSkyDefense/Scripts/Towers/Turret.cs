using System.Collections;
using UnityEngine;

namespace Assets
{
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(AudioSource))]
    public class Turret : BaseTower
    {
        public float ProjectileSpeed = 10;
        public float RotationSpeed = 1;
        public float ShootInterval = 3;
        public float MaxHealth = 100f;

        public AudioSource AudioSource;
    }
}