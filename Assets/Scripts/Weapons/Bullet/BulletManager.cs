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
        public TrailRenderer trail;
        
        
        public void InitializeBulletAttributes(BulletData bulletData)
        {
            _damage = bulletData.damage;

            if (Vector3.Dot(transform.forward, PlayerManager.Instance.CharacterController.velocity) > 0.5)
                _speed = bulletData.bulletSpeed + PlayerManager.Instance.MovementManager.HorizontalVelocity * 1.5f;
            else
            {
                _speed = bulletData.bulletSpeed;
                Debug.Log("Slow down");
            }

            trail.startColor = bulletData.trailColor;
        }

        private void FixedUpdate()
        {
            rb.MovePosition(transform.position + (_speed * Time.fixedDeltaTime * transform.forward));
        }
    }
}