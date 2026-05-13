using System;
using System.Collections;
using System.Collections.Generic;
using GameManagers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
   [SerializeField] private GameObject timer;
   [SerializeField] private TextMeshProUGUI gameoverTimerText;
   [SerializeField] private TextMeshProUGUI deathMenuTimerText;
   public GameObject gameoverMenu;
   public GameObject deathMenu;

   public static UIManager instance;

   private void Awake()
   {
      if (instance == null)
      {
         instance = this;
      }
      else
      {
         Destroy(gameObject);
      }
   }

   private void Start()
   {
      GameManager.Instance.OnUpdateEnemyUI += CheckWinCondition;
   }

   void CheckWinCondition(int enemiesKilled, int totalEnemies)
   {
      if (enemiesKilled == totalEnemies)
      {
         gameoverMenu.SetActive(true);
         float finalTime = (float)Variables.Object(timer).Get("Time");
         gameoverTimerText.text = "Time: " + $"{finalTime:F2}";
         Cursor.lockState = CursorLockMode.None;
         Cursor.visible = true;
         Time.timeScale = 0;
         PlayerManager.Instance.InputManager.PlayerInput.DeactivateInput();
         PlayerManager.Instance.MovementManager.enabled = false;
      }
   }

   public void ActivateDeathScreen()
   {
      deathMenu.SetActive(true);
      float finalTime = (float)Variables.Object(timer).Get("Time");
      deathMenuTimerText.text = "Time: " + $"{finalTime:F2}";
      Cursor.lockState = CursorLockMode.None;
      Cursor.visible = true;
      Time.timeScale = 0;
      PlayerManager.Instance.InputManager.PlayerInput.DeactivateInput();
      PlayerManager.Instance.MovementManager.enabled = false;
   }

   public void RestartScene()
   {
      Time.timeScale = 1;
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
   }

   private void OnDisable()
   {
      GameManager.Instance.OnUpdateEnemyUI -= CheckWinCondition;
   }
}
