using System;
using Interface;
using UnityEngine;

namespace Enemy
{
    public class EnemyManager : MonoBehaviour, IDamageable
    {
        [SerializeField] private GameObject fractureObject;

        private float health = 100f;
        private float maxHealth = 100f;

        public static event Action<EnemyManager> OnEnemyKilled;
        public static event Action<EnemyManager> OnEnemySpawned;

        private void Awake()
        {
            health = maxHealth;
        }

        private void Start()
        {
            OnEnemySpawned?.Invoke(this);
        }

        public void Damage(float damage)
        {
            if (health == 0)
                return;

            health = Mathf.Max(health - damage, 0);

            if (health == 0)
            {
                OnEnemyKilled?.Invoke(this);
                Instantiate(fractureObject, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
        
    }
}