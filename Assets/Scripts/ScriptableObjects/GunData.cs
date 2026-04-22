using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    public enum FireMode
    {
        Hitscan,
        Projectile
    }

    [Serializable]
    public struct BulletData
    {
        public float damage;
        public float bulletSpeed;
        public Gradient trailColor;
    }
    
    [CreateAssetMenu(fileName = "GunData", menuName = "New GunData", order = 0)]
    public class GunData : ScriptableObject
    {
        public FireMode fireMode;
        public int ammoCapacity;
        public float fireRate;
        public int bulletCount;
        public Vector3 spreadVariance;
        public float pushForce;
        public float verticalForce;
        public bool impulseY = false;
        public BulletData bulletData;
        public float screenShakeForce = 1f;

    }
}