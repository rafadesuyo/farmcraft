using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class TrialCategoryItem : CategoryUIBaseItem, IDateTimeFetcher
{
    [SerializeField] TrialModeScoresDataSO trialModeScoresData;
    [SerializeField] private TextMeshProUGUI playTimesText;
    [SerializeField] private GameObject cooldownWarning;
    [SerializeField] private GameObject dataRetrieveWarning;

    private int playTimeCheckIntervalSeconds = 5;
    private DateTime nextRecharge;
    private Button button;

    public static event Action<QuizCategory> OnCategorySelected;
    public static event Action<QuizCategory, string> OnPlayTimeSaved;

    private void OnEnable()
    {
        InvokeRepeating(nameof(CheckForRecharge), 0.1f, playTimeCheckIntervalSeconds);
        cooldownWarning.SetActive(trialModeScoresData.GetCurrentCardChargesByCategory(quizCategory) == 0);
        UpdatePlayTimesText();

        CardInfosUI.OnPressedStart += SaveCurrentPlayTime;
        TrialStageManager.OnDataLoaded += DisableDataRetrieveWarning;
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(CheckForRecharge));
        CardInfosUI.OnPressedStart -= SaveCurrentPlayTime;
        TrialStageManager.OnDataLoaded -= DisableDataRetrieveWarning;
    }

    private void Start()
    {
        if (quizCategory != QuizCategory.Random)
        {
            SetupItem();
        }

        CalculateCardChargesOnLoad();

        SetButtonListener();
        dataRetrieveWarning.SetActive(true);
    }

    //Checks the play time condition for the quiz category.If the last played time for the category is not empty, it calculates the time elapsed since the last play and
    //increases the current card charges if the cooldown time has passed and the maximum card charges have not been reached.
    private void CalculateCardChargesOnLoad()
    {
        int currentCharges = trialModeScoresData.GetCurrentCardChargesByCategory(quizCategory);
        int maxCharges = trialModeScoresData.GetMaxCardChargesByCategory(quizCategory);
        string lastPlayedTime = trialModeScoresData.GetLastPlayedTimeByCategory(quizCategory);

        if (currentCharges >= maxCharges || string.IsNullOrEmpty(lastPlayedTime))
        {
            return;
        }

        TimeSpan timeElapsed = GetCurrentDateTime() - DateTime.FromBinary(Convert.ToInt64(lastPlayedTime));

        int numberOfChargesToAdd = Mathf.FloorToInt((float)timeElapsed.TotalMinutes / trialModeScoresData.CooldownMinutes);

        if (numberOfChargesToAdd > 0)
        {
            trialModeScoresData.IncreaseCardCurrentCharges(quizCategory, numberOfChargesToAdd);
        }
    }

    private void CheckForRecharge()
    {
        string lastPlayedTime = trialModeScoresData.GetLastPlayedTimeByCategory(quizCategory);
        if (!string.IsNullOrEmpty(lastPlayedTime))
        {
            DateTime lastTimePlayed = DateTime.FromBinary(Convert.ToInt64(lastPlayedTime));
            DateTime currentDateTime = GetCurrentDateTime();

            if (currentDateTime >= lastTimePlayed.AddMinutes(trialModeScoresData.CooldownMinutes) &&
                currentDateTime >= nextRecharge)
            {
                trialModeScoresData.IncreaseCardCurrentCharges(quizCategory, 1);
                nextRecharge = currentDateTime.AddMinutes(trialModeScoresData.CooldownMinutes);
            }
        }

        bool isCooldownWarningActive = trialModeScoresData.GetCurrentCardChargesByCategory(quizCategory) == 0;
        cooldownWarning.SetActive(isCooldownWarningActive);

        UpdatePlayTimesText();
    }

    private void UpdatePlayTimesText()
    {
        playTimesText.text = $"Remaining: {trialModeScoresData.GetCurrentCardChargesByCategory(quizCategory)}/{trialModeScoresData.GetMaxCardChargesByCategory(quizCategory)}";
    }

    private void SetButtonListener()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(SelectCategory);
        }
    }

    private void SelectCategory()
    {
        OnCategorySelected?.Invoke(quizCategory);
    }

    private void SaveCurrentPlayTime(QuizCategory quizCategory)
    {
        if (this.quizCategory != quizCategory)
            return;

        OnPlayTimeSaved?.Invoke(quizCategory, GetCurrentDateTime().ToBinary().ToString());
    }

    public DateTime GetCurrentDateTime()
    {
        return DateTime.Now;
    }

    private void DisableDataRetrieveWarning()
    {
        dataRetrieveWarning.SetActive(false);
    }
}
