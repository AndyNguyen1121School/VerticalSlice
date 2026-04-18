using System;
using ScriptableObjects;
using UnityEngine;

namespace Weapons.Bullet
{
    public class BulletManager : MonoBehaviour
    {
        private float _damage;
        [SerializeField] private float _speed;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private TrailRenderer trail;
        
        
        public void InitializeBulletAttributes(BulletData bulletData)
        {
            _damage = bulletData.damage;
            _speed = bulletData.bulletSpeed;
            trail.startColor = bulletData.trailColor;
        }

        private void FixedUpdate()
        {
            rb.MovePosition(transform.position + (_speed * Time.fixedDeltaTime * transform.forward));
        }
    }
}