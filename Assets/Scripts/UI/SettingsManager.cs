using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsManager : MonoBehaviour
    {
        [SerializeField] private Slider sensitivitySlider;
        [SerializeField] private GameObject pauseMenu;

        private void Start()
        {
            sensitivitySlider.onValueChanged.AddListener(ChangeSensitivity);
            sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EnablePauseMenu(!pauseMenu.activeSelf);
            }
        }

        private void ChangeSensitivity(float value)
        {
            if (PlayerManager.Instance == null)
                return;

            PlayerManager.Instance.InputManager.sensitivity = sensitivitySlider.value;
            PlayerPrefs.SetFloat("Sensitivity", sensitivitySlider.value);
            PlayerPrefs.Save();
        }

        private void EnablePauseMenu(bool active)
        {
            if (active)
            {
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                pauseMenu.SetActive(true);
                PlayerManager.Instance.InputManager.PlayerInput.DeactivateInput();
                PlayerManager.Instance.InputManager.ResetMouseInput();
            }
            else
            {
                Time.timeScale = 1;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                pauseMenu.SetActive(false);
                PlayerManager.Instance.InputManager.PlayerInput.ActivateInput();

            }
        }
    }
}