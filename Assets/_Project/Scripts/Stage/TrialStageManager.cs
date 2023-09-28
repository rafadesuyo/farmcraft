using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TrialStageManager : MonoBehaviour
{
    [SerializeField] private StageInfoSO stageInfo;
    [SerializeField] private TrialModeScoresDataSO trialModeScoreData;

    private int correctAnswersTarget = 999;
    private Dictionary<TrialStageState.StateName, TrialStageState> trialStageStates = new Dictionary<TrialStageState.StateName, TrialStageState>();
    private bool initialized = false;
    private TrialStageState currentTrialStageState;
    private QuizSystem quizSystem;
    private QuizCategory currentQuizCategory;

    public StageInfoSO TrialStageInfo { get => stageInfo; }

    public int TrialStageID
    {
        get
        {
            return TrialStageInfo.Id;
        }
    }

    public event Action<TrialStageState.StateName> OnTrialStageEnter;
    public event Action<TrialStageState.StateName> OnTrialStateLeave;
    public static event Action<int> OnTrialModeInitialized;
    public static event Action<int> OnTrialRewardHandled;
    public static event Action OnDataLoaded;
    public static event Action<QuizCategory, int, int> OnScoreUpdated;

    TimeCountdownSystem timeCountdownSystem;

    private void Awake()
    {
        StartCoroutine(MockLoadScoresWithDelayCoroutine());
    }

    private void Start()
    {
        SetupTrialStage();
        Mock_LoadGame();
        SetTrialStageState(TrialStageState.StateName.Start);
        timeCountdownSystem = StageSystemLocator.GetSystem<TimeCountdownSystem>();
        SetupQuiz();
    }

    private void Update()
    {
        if (timeCountdownSystem.HasTimerExpired())
        {
            quizSystem.EndQuiz();
            HandleReward();
            UpdateScore(currentQuizCategory, quizSystem.CurrentQuestionCount);
        }
    }

    private void OnEnable()
    {
        CardInfosUI.OnPressedStart += StartInfiniteQuiz;
        TrialCategoryItem.OnPlayTimeSaved += UpdateTimeDate;
    }

    private void OnDisable()
    {
        quizSystem.OnQuizStateChange -= ChangeCountdownTimerState;
        quizSystem.OnQuizStateChange -= ResetTrial;
        CardInfosUI.OnPressedStart -= StartInfiniteQuiz;
        TrialCategoryItem.OnPlayTimeSaved -= UpdateTimeDate;
    }

    public void SetupTrialStage()
    {
        InitializeStates();
        initialized = true;

        int rewardPerRightAnswer = TrialStageInfo.Goals[0].GoalReward.TotalReward;
        OnTrialModeInitialized?.Invoke(rewardPerRightAnswer);
    }

    private void SetupQuiz()
    {
        quizSystem = StageSystemLocator.GetSystem<QuizSystem>();
        quizSystem.OnQuizStateChange += ChangeCountdownTimerState;
        quizSystem.OnQuizStateChange += ResetTrial;
    }

    private void StartInfiniteQuiz(QuizCategory category)
    {
        currentQuizCategory = category;
        quizSystem.StartQuiz(correctAnswersTarget, currentQuizCategory, QuizDifficulty.Level.Easy);
        timeCountdownSystem?.ResumeCountingTime();
    }

    private void InitializeStates()
    {
        trialStageStates.Clear();

        var allStates = Assembly.GetAssembly(typeof(TrialStageState))
            .GetTypes()
            .Where(t => typeof(TrialStageState).IsAssignableFrom(t) && !t.IsAbstract);

        foreach (var state in allStates)
        {
            TrialStageState stageState = Activator.CreateInstance(state) as TrialStageState;
            var trialStageName = stageState.GetStateName();

            if (trialStageStates.ContainsKey(trialStageName))
            {
                Debug.LogError("[TrialStageManager] Duplicate StageState: " + trialStageName);
                continue;
            }

            stageState.Initialize(this);
            trialStageStates.Add(trialStageName, stageState);
        }
    }

    public void SetTrialStageState(TrialStageState.StateName trialStateName)
    {
        if (!initialized)
        {
            Debug.LogError("[StageManager] Cannot change to state:" + trialStateName + ". StageManager is not initialized");
            return;
        }

        if (!trialStageStates.TryGetValue(trialStateName, out var state))
        {
            Debug.LogError("[StageManager] State not found: " + trialStateName);
            return;
        }

        SetTrialStageState(state);
    }

    private void SetTrialStageState(TrialStageState trialStageState)
    {
        if (currentTrialStageState != null)
        {
            currentTrialStageState.OnLeave();
            OnTrialStateLeave?.Invoke(currentTrialStageState.GetStateName());
        }

        currentTrialStageState = trialStageState;

        currentTrialStageState.OnEnter();
        OnTrialStageEnter?.Invoke(currentTrialStageState.GetStateName());
    }

    //MOCK
    private IEnumerator MockLoadScoresWithDelayCoroutine()
    {
        LoadAllScores();
        LoadAllTimeData();
        yield return new WaitForSeconds(3);
        OnDataLoaded?.Invoke();
    }

    //MOCK
    public void Mock_LoadGame()
    {
        GameData gameData = SaveManager.LoadData<GameData>(SaveManager.playerData_SaveInfo);
        PlayerProgress.EvaluateLoad(gameData);
    }

    public void ChangeCountdownTimerState(QuizState questionState)
    {
        if (questionState == QuizState.WaitingForAnswer)
        {
            timeCountdownSystem?.ResumeCountingTime();
        }
        else if (questionState == QuizState.CorrectAnswer || 
                 questionState == QuizState.WrongAnswer)
        {
            timeCountdownSystem?.PauseCountingTime();
        }
    }

    public void ResetCountdownTimer()
    {
        timeCountdownSystem = StageSystemLocator.GetSystem<TimeCountdownSystem>();
        timeCountdownSystem?.ResetCountingTime();
    }

    public void ResetTrial(QuizState questionState)
    {
        if (questionState == QuizState.None)
        {
            ResetCountdownTimer();

        }
    }

    private void HandleReward()
    {
        int amountToReward = TrialStageInfo.Goals[0].GoalReward.TotalReward * quizSystem.CurrentQuestionCount;
        GoldRewardPackage goldReward = new GoldRewardPackage(amountToReward);
        goldReward.Unpack();
        OnTrialRewardHandled?.Invoke(amountToReward);
    }

    #region Save/Load
    public void UpdateScore(QuizCategory category, int value)
    {
        trialModeScoreData.InitializeDataHandler();

        foreach (TrialModeCardInfo item in trialModeScoreData.trialModeCards)
        {
            if (item.quizCategory == category)
            {
                item.previousScore = value;

                if (item.previousScore > item.highScore)
                {
                    item.highScore = item.previousScore;
                }
                OnScoreUpdated?.Invoke(item.quizCategory, item.previousScore, item.highScore);
                trialModeScoreData.TrialModeDataHandler.UpdateScoreData(item.quizCategory, item.highScore, item.previousScore);
            }
        }
    }

    public void LoadAllScores()
    {
        trialModeScoreData.InitializeDataHandler();

        foreach (TrialModeCardInfo item in trialModeScoreData.trialModeCards)
        {
            TrialModeCardInfo loadedData = trialModeScoreData.TrialModeDataHandler.LoadScoreData(item.quizCategory);
            item.highScore = loadedData.highScore;
            item.previousScore = loadedData.previousScore;
        }
    }

    private void UpdateTimeDate(QuizCategory category, string lastTimePlayed)
    {
        trialModeScoreData.InitializeDataHandler();

        foreach (TrialModeCardInfo item in trialModeScoreData.trialModeCards)
        {
            if (item.quizCategory == category)
            {
                item.lastTimePlayed = lastTimePlayed;
                item.currentCardCharges--;
                if (item.currentCardCharges < 0)
                {
                    item.currentCardCharges = 0;
                }

                trialModeScoreData.TrialModeDataHandler.UpdateTimeData(item.quizCategory, lastTimePlayed, item.currentCardCharges);
            }
        }
    }

    private void LoadAllTimeData()
    {
        trialModeScoreData.InitializeDataHandler();

        foreach (TrialModeCardInfo item in trialModeScoreData.trialModeCards)
        {
            TrialModeCardInfo loadedData = trialModeScoreData.TrialModeDataHandler.LoadTimeData(item.quizCategory);
            item.lastTimePlayed = loadedData.lastTimePlayed;

            if (loadedData.currentCardCharges >= 0)
            {
                item.currentCardCharges = loadedData.currentCardCharges;
            }
            else
            {
                item.currentCardCharges = item.maxCardCharges;
            }
        }
    }
    #endregion
}
