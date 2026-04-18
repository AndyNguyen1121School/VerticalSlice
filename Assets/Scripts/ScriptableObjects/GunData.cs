using System;
using UnityEngine;

namespace ScriptableObjects
{
    public enum FireMode
    {
        Single,
        Auto,
        Spread
    }

    [Serializable]
    public struct BulletData
    {
        public float damage;
        public float bulletSpeed;
        public Color trailColor;
    }
    
    [CreateAssetMenu(fileName = "GunData", menuName = "New GunData", order = 0)]
    public class GunData : ScriptableObject
    {
        public FireMode fireMode;
        public int ammoCapacity;
        public float fireRate;
        public int bulletCount;
        public Vector3 spreadVariance;
        public BulletData bulletData;
    }
}