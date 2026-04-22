using System;
using System.Collections;
using System.Collections.Generic;
using GameManagers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
   public GameObject gameoverMenu;

   private void Start()
   {
      GameManager.Instance.OnUpdateEnemyUI += CheckWinCondition;
   }

   void CheckWinCondition(int enemiesKilled, int totalEnemies)
   {
      if (enemiesKilled == totalEnemies)
      {
         gameoverMenu.SetActive(true);
         Cursor.lockState = CursorLockMode.None;
         Cursor.visible = true;
         Time.timeScale = 0;
         PlayerManager.Instance.InputManager.PlayerInput.DeactivateInput(); 
      }
   }

   public void RestartScene()
   {
      Time.timeScale = 1;
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
   }
}
