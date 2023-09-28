using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrialModeCardInfo
{
    public QuizCategory quizCategory;
    public int previousScore;
    public int highScore;
    public string lastTimePlayed;
    public int currentCardCharges;
    public int maxCardCharges;
}

[CreateAssetMenu(fileName = "New TrialModeScoresData", menuName = "TrialModeScoresData")]
public class TrialModeScoresDataSO : ScriptableObject
{
    [Header("General Trial Card Rules")]
    [SerializeField] private int cooldownMinutes = 30;
    public int CooldownMinutes => cooldownMinutes;

    public List<TrialModeCardInfo> trialModeCards = new List<TrialModeCardInfo>();
    //public static event Action<QuizCategory, int, int> OnScoreUpdated;
    ITrialModeDataHandler trialModeDataHandler = null;
    public ITrialModeDataHandler TrialModeDataHandler => trialModeDataHandler;

    public void InitializeDataHandler()
    {
        if (trialModeDataHandler == null)
        {
            trialModeDataHandler = new MockTrialModeDataHandler();
        }
    }

    public int GetHighScoreByCategory(QuizCategory category)
    {
        InitializeDataHandler();

        foreach (var item in trialModeCards)
        {
            if (item.quizCategory == category)
            {
                return item.highScore;
            }
        }

        return 0;
    }

    public int GetPreviousScoreByCategory(QuizCategory category)
    {
        InitializeDataHandler();

        foreach (var item in trialModeCards)
        {
            if (item.quizCategory == category)
            {
                return item.previousScore;
            }
        }

        return 0;
    }

    public string GetLastPlayedTimeByCategory(QuizCategory category)
    {
        InitializeDataHandler();

        foreach (var item in trialModeCards)
        {
            if (item.quizCategory == category)
            {
                return item.lastTimePlayed;
            }
        }

        return null;
    }

    public int GetCurrentCardChargesByCategory(QuizCategory category)
    {
        InitializeDataHandler();

        foreach (var item in trialModeCards)
        {
            if (item.quizCategory == category)
            {
                return item.currentCardCharges;
            }
        }

        return 0;
    }

    public int GetMaxCardChargesByCategory(QuizCategory category)
    {
        InitializeDataHandler();

        foreach (var item in trialModeCards)
        {
            if (item.quizCategory == category)
            {
                return item.maxCardCharges;
            }
        }

        return 0;
    }

    public TrialModeCardInfo GetCardInfo(QuizCategory category)
    {
        foreach (var item in trialModeCards)
        {
            if (item.quizCategory == category)
            {
                return item;
            }
        }

        return null;
    }

    public void IncreaseCardCurrentCharges(QuizCategory category, int amount)
    {
        foreach (var item in trialModeCards)
        {
            if (item.quizCategory == category)
            {
                item.currentCardCharges += amount;
                item.currentCardCharges = Mathf.Clamp(item.currentCardCharges, 0, item.maxCardCharges);
            }
        }
    }
}
