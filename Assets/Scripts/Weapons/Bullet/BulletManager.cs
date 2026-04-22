using System;
using Interface;
using ScriptableObjects;
using UnityEngine;

namespace Weapons.Bullet
{
    public class BulletManager : MonoBehaviour
    {
        private float _damage;
        [SerializeField] private float _speed;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float rayLength = 1f;
        public TrailRenderer trail;
        private float _cachedTime;
        [SerializeField] private LayerMask whatIsDamageable;
        [SerializeField] private ParticleSystem bulletParticle;
        [SerializeField] private Gradient trailColor;
        [SerializeField] private float velocityScale = 1f;

        private void Awake()
        {
            _cachedTime = trail.time;
        }

        
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

            trailColor = bulletData.trailColor;
        }

        private void Update()
        {
            HandleBulletCollisions();
        }

        private void FixedUpdate()
        {
            rb.MovePosition(transform.position + (_speed * Time.fixedDeltaTime * transform.forward));
        }

        public void HandleBulletCollisions()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength * (rb.velocity.magnitude * velocityScale),
                    whatIsDamageable))
            {
                IDamageable damageScript;
                if (hit.collider.TryGetComponent<IDamageable>(out damageScript))
                {
                    damageScript.Damage(_damage);
                }
                ActivateParticle();
                gameObject.SetActive(false);
            }
        }

        private void ActivateParticle()
        {
            ParticleSystem particle = Instantiate(bulletParticle, transform.position, Quaternion.identity);
            ParticleSystem.MainModule main = particle.main;
            main.startColor = trailColor;
        }

        public void DisableTrailRenderer()
        {
            trail.time = 0;
            trail.Clear();
        }

        public void EnableTrailRenderer()
        {
            trail.time = _cachedTime;
        }
        
        void OnDrawGizmos()
        {
            if (rb == null) return;

            Gizmos.color = Color.red;
            float length = rayLength * (rb.velocity.magnitude * velocityScale);
            Vector3 direction = transform.forward * length;

            Gizmos.DrawLine(transform.position, transform.position + direction);
        }
    }
}