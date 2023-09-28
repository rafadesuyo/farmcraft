using System;
using UnityEngine;

public class MockTrialModeDataHandler : ITrialModeDataHandler
{
    private const string previousScoreKeyPrefix = "PreviousScore_";
    private const string highScoreKeyPrefix = "HighScore_";
    private const string lastTimePlayedKey = "LastTimePlayed_";
    private const string currentChargesKey = "CurrentCharges_";

    public void UpdateScoreData(QuizCategory quizCategory, int highScoreValue, int previousScoreValue)
    {
        Debug.LogWarning($"Mock saving trial mode highScores -> {quizCategory}:{highScoreValue}");

        PlayerPrefs.SetInt(highScoreKeyPrefix + quizCategory, highScoreValue);
        PlayerPrefs.SetInt(previousScoreKeyPrefix + quizCategory, previousScoreValue);       
    }

    public TrialModeCardInfo LoadScoreData(QuizCategory quizCategory)
    {
        Debug.LogWarning("Mock loading trial mode highScore/previousScore datas...");

        int highScore = PlayerPrefs.GetInt(highScoreKeyPrefix + quizCategory);
        int previousScore = PlayerPrefs.GetInt(previousScoreKeyPrefix + quizCategory);

        return new TrialModeCardInfo
        {
            quizCategory = quizCategory,
            highScore = highScore,
            previousScore = previousScore
        };
    }

    public void UpdateTimeData(QuizCategory quizCategory, string lastTimePlayed, int currentCharges)
    {
        PlayerPrefs.SetString(lastTimePlayedKey + quizCategory, lastTimePlayed);
        PlayerPrefs.SetInt(currentChargesKey + quizCategory, currentCharges);
    }

    public TrialModeCardInfo LoadTimeData(QuizCategory quizCategory)
    {
        var loadedString = PlayerPrefs.GetString(lastTimePlayedKey + quizCategory);

        var loadedCurrentCharges = 0;

        if (PlayerPrefs.HasKey(currentChargesKey + quizCategory))
        {
            loadedCurrentCharges = PlayerPrefs.GetInt(currentChargesKey + quizCategory);
        }
        else
        {
            loadedCurrentCharges = -1;
        } 

        return new TrialModeCardInfo
        {
            quizCategory = quizCategory,
            lastTimePlayed = loadedString,
            currentCardCharges = loadedCurrentCharges
        };
    }
}
