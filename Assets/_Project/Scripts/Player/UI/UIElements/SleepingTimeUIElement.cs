using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DreamQuiz.Player
{
    public class SleepingTimeUIElement : PlayerUIElement
    {
        [SerializeField] private Image sleepingTimeBar = null;
        [SerializeField] private TextMeshProUGUI sleepingTimeText = null;

        private int maxSleepingTime = 0;
        private int currentSleepingTime = 0;

        private void OnEnable()
        {
            if (!initialized)
            {
                return;
            }

            playerStageInstance.PlayerStageData.SleepingTime.OnSleepingTimeChanged += PlayerStageData_OnSleepingTimeChanged;
        }

        private void OnDisable()
        {
            playerStageInstance.PlayerStageData.SleepingTime.OnSleepingTimeChanged -= PlayerStageData_OnSleepingTimeChanged;
        }

        protected override void InitializeUI()
        {
            playerStageInstance.PlayerStageData.SleepingTime.OnSleepingTimeChanged += PlayerStageData_OnSleepingTimeChanged;

            maxSleepingTime = playerStageInstance.PlayerStageData.SleepingTime.MaxValue;
            currentSleepingTime = playerStageInstance.PlayerStageData.SleepingTime.CurrentValue;

            UpdateUI();
        }

        private void PlayerStageData_OnSleepingTimeChanged(int value)
        {
            currentSleepingTime = value;
            UpdateUI();
        }

        public void UpdateUI()
        {
            sleepingTimeText.text = $"{currentSleepingTime}/{maxSleepingTime}";
            sleepingTimeBar.fillAmount = Mathf.InverseLerp(0, maxSleepingTime, currentSleepingTime);
        }
    }
}