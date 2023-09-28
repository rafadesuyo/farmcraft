using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class CardInfosUI : MonoBehaviour
{
    [SerializeField] private Transform content;
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI cardTitleText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private TextMeshProUGUI previousScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private GameObject notEnoughPillowsWarning;
    [Header("Buttons")]
    [SerializeField] private Button closeButton;
    [SerializeField] private Button playButton;
    [Space(10)]
    [Header("Scriptable Object reference")]
    [SerializeField] private TrialModeScoresDataSO trialModeScoresData;

    private int rewardValuePerRightAnswer;
    private QuizCategory quizCategory;

    private const string previousScoreTextPrefix = "Previous Score: ";
    private const string highScoreTextPrefix = "High Score: ";

    private const string timerTextSuffix = "s";
    private const string rewardTextSuffix = " each right answer!";

    public static event Action<QuizCategory> OnPressedStart;

    private void OnEnable()
    {
        TrialStageManager.OnTrialModeInitialized += GetRewardValuePerRightAnswer;
        TrialCategoryItem.OnCategorySelected += Show;
    }

    private void OnDisable()
    {
        TrialStageManager.OnTrialModeInitialized -= GetRewardValuePerRightAnswer;
        TrialCategoryItem.OnCategorySelected -= Show;
    }

    private void Start()
    {
        Hide();
        InitButtons();
    }

    private void InitButtons()
    {
        closeButton.onClick.AddListener(Hide);
        playButton.onClick.AddListener(Play);
    }

    public void Show(QuizCategory category)
    {
        SetPlayButtonState();

        var timeCountdownSystem = StageSystemLocator.GetSystem<TimeCountdownSystem>();
        quizCategory = category;

        content.gameObject.SetActive(true);

        if (quizCategory!= QuizCategory.Random)
        {
            cardTitleText.text = ProjectAssetsDatabase.Instance.GetCategoryName(quizCategory);
        }
        else
        {
            cardTitleText.text = Enum.GetName(typeof(QuizCategory), 1);
        }

        timerText.text = timeCountdownSystem.TargetTime + timerTextSuffix;
        rewardText.text = rewardValuePerRightAnswer + rewardTextSuffix;
        previousScoreText.text = previousScoreTextPrefix + trialModeScoresData.GetPreviousScoreByCategory(category);
        highScoreText.text = highScoreTextPrefix + FormatValueText(trialModeScoresData.GetHighScoreByCategory(category));
    }

    private string FormatValueText(int value)
    {
        if (value == 0)
        {
            return "-";
        }

        return value.ToString();
    }

    private void Hide()
    {
        content.gameObject.SetActive(false);
    }

    private void GetRewardValuePerRightAnswer(int value)
    {
        rewardValuePerRightAnswer = value;
    }

    private void Play()
    {
        Hide();
        OnPressedStart?.Invoke(quizCategory);
    }

    private void SetPlayButtonState()
    {
        bool hasPillow = PlayerProgress.SaveState.playerInfo.currentPillowCount > 0;
        notEnoughPillowsWarning.SetActive(!hasPillow);
        playButton.interactable = hasPillow;
    }
}


