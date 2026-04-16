using System;
using TMPro;
using Unity.VisualScripting;
using Unity.UI;
using UnityEngine;

namespace UI
{
    public class SpeedCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI speedText;

        private void Start()
        {
            if (PlayerManager.Instance != null)
            {
                PlayerManager.Instance.MovementManager.onSpeedChanged += UpdateSpeedCounter;
            }
        }

        private void UpdateSpeedCounter(float speed)
        {
            speedText.text = $"Speed: {speed:F0}";
        }
    }
}