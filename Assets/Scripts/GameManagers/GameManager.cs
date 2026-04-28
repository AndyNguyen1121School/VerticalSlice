using System;
using Enemy;
using UnityEngine;

namespace GameManagers
{
    public class GameManager : MonoBehaviour
    {
        private int TotalEnemies = 0;
        private int EnemiesKilled = 0;

        public static GameManager Instance;
        public event Action<int, int> OnUpdateEnemyUI;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            
            EnemyManager.OnEnemySpawned += EnemySpawned;
            EnemyManager.OnEnemyKilled += EnemyKilled;
        }

        private void EnemySpawned(EnemyManager enemyManager)
        {
            TotalEnemies++;
            OnUpdateEnemyUI?.Invoke(EnemiesKilled, TotalEnemies);
        }

        private void EnemyKilled(EnemyManager enemyManager)
        {
            EnemiesKilled++;
            PlayerManager.Instance.MovementManager.speedCap = Mathf.Min( PlayerManager.Instance.MovementManager.speedCap + 2, 40);
            OnUpdateEnemyUI?.Invoke(EnemiesKilled, TotalEnemies);
        }

        private void OnDisable()
        {
            EnemyManager.OnEnemySpawned -= EnemySpawned;
            EnemyManager.OnEnemyKilled -= EnemyKilled;
        }
    }
}