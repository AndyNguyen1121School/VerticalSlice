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

        [Header("Attack Settings")] 
        [SerializeField] private float minimumDistanceToAttack;
        public bool canAttack = true;

        private void Awake()
        {
            health = maxHealth;
        }

        private void Start()
        {
            OnEnemySpawned?.Invoke(this);
            retreatLocation = transform.position;
            agent.updateRotation = false;
        }

        private void Update()
        {
            animator.SetBool("IsWalking", (agent.velocity.sqrMagnitude > 0.04f));
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
            if (canAttack && DistanceFromPlayer() <= minimumDistanceToAttack)
            {
                Attack();
            }
            else if (!canAttack)
            {
                RotateTowardsPlayer();
            }
            else
            {
                RotateTowardsVelocity();
            }
        }
        
        public void HandleRetreatState()
        {
            agent.SetDestination(retreatLocation);
        }

        private void RotateTowardsVelocity()
        {
            if (agent.desiredVelocity.magnitude <= 0.001f)
                return;
            
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(agent.desiredVelocity.normalized), rotationSpeed * Time.deltaTime);
        }

        private void RotateTowardsPlayer()
        {
            Vector3 directionToPlayer = PlayerManager.Instance.transform.position - transform.position;
            directionToPlayer.y = 0;
            directionToPlayer.Normalize();
            
            transform.rotation = Quaternion.Slerp(transform.rotation, 
                Quaternion.LookRotation(directionToPlayer), rotationSpeed * Time.deltaTime);
        }

        private void Attack()
        {
            animator.CrossFade("Attack", 0.1f);
            canAttack = false;
            DeactivateMovement();
            Debug.Log("Attacking");
        }

        public void ActivateMovement()
        {
            agent.isStopped = false;
        }

        public void DeactivateMovement()
        {
            agent.isStopped = true;
        }

    }
}