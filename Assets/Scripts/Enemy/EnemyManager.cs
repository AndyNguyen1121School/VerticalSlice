using System;
using Interface;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    [IncludeInSettings(true)]
    public class EnemyManager : MonoBehaviour, IDamageable
    {
        [SerializeField] private GameObject fractureObject;
        [SerializeField] public Vector3 retreatLocation;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Animator animator;
        [SerializeField] private float rotationSpeed = 5f;
        
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
            retreatLocation = transform.position;
            OnEnemySpawned?.Invoke(this);
            agent.updateRotation = false;
        }

        private void Update()
        {
  
            animator.SetBool("IsWalking", (agent.velocity.sqrMagnitude > 0.04f));
            HandleRotations();

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

        public float DistanceFromPlayer()
        {
            return ((transform.position - PlayerManager.Instance.transform.position).magnitude);
        }

        public float DistanceFromRetreatLocation()
        { 
            return (transform.position - retreatLocation).magnitude;
        }

        public void HandleIdleState()
        {
            
        }

        public void HandleChaseState()
        {
            agent.SetDestination(PlayerManager.Instance.transform.position);
        }
        
        public void HandleRetreatState()
        {
            agent.SetDestination(retreatLocation);
        }

        private void HandleRotations()
        {
            if (agent.desiredVelocity.magnitude <= 0.001f)
                return;
            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(agent.desiredVelocity.normalized), rotationSpeed * Time.deltaTime);
        }

    }
}