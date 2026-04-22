using System;
using GameManagers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class EnemyCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI enemyCounter;

        private void Start()
        {
            GameManager.Instance.OnUpdateEnemyUI += UpdateEnemyCounter;
        }

        private void UpdateEnemyCounter(int enemiesKilled, int totalEnemies)
        {
            enemyCounter.text = "Enemies: " + enemiesKilled + " / " + totalEnemies;
        }
    }
}