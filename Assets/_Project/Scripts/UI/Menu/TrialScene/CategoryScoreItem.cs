using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CategoryScoreItem : CategoryUIBaseItem
{
    [SerializeField] private TrialModeScoresDataSO trialModeScoreData;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private void OnEnable()
    {
        TrialStageManager.OnDataLoaded += UpdateHighScoreText;
        TrialStageManager.OnScoreUpdated += UpdateHighScoreText;
    }

    private void OnDisable()
    {
        TrialStageManager.OnDataLoaded -= UpdateHighScoreText;
        TrialStageManager.OnScoreUpdated -= UpdateHighScoreText;
    }

    void Start()
    {
        SetupItem();
    }

    void UpdateHighScoreText()
    {
        highScoreText.text = FormatValueText(trialModeScoreData.GetHighScoreByCategory(quizCategory));
    }

    void UpdateHighScoreText(QuizCategory category, int previousScore, int highScore)
    {
        if (quizCategory == category)
        {
            highScoreText.text = highScore.ToString();
        }
    }

    private string FormatValueText(int value)
    {
        if (value == 0)
        {
            return "-";
        }

        return value.ToString();
    }
}
