using DreamQuiz.Player;
using System;
using UnityEngine;

public class StageGoalProgress
{
    public StageGoal StageGoal { get; private set; }
    public StageGoalEvaluator StageGoalEvaluator { get; private set; }
    public int ProgressValue { get; private set; }
    public bool IsComplete { get; private set; }
    public float ProgressRatio
    {
        get
        {
            return Mathf.Clamp((float)ProgressValue / StageGoal.TargetValue, 0, 1);
        }
    }

    public int RewardedValue
    {
        get
        {
            int rewardAmount = 0;

            if (ProgressRatio == 1 || StageGoal.GoalReward.GivePartialReward)
            {
                rewardAmount = Mathf.RoundToInt(StageGoal.GoalReward.TotalReward * ProgressRatio);
            }

            return rewardAmount;
        }
    }

    public event Action<int> OnProgress;
    public event Action OnComplete;

    public StageGoalProgress(PlayerStageData playerStageData, StageGoal stageGoal, StageGoalEvaluator stageGoalEvaluator)
    {
        StageGoal = stageGoal;
        StageGoalEvaluator = stageGoalEvaluator;
        StageGoalEvaluator.InitializeEvaluator(playerStageData, this);
    }

    public void UpdateValue(int value)
    {
        ProgressValue = value;

        OnProgress?.Invoke(value);
    }

    public void CompleteProgress()
    {
        IsComplete = true;

        OnComplete?.Invoke();
    }

    public IRewardPackage GetReward()
    {
        return RewardProcessor.GiveReward(this);
    }
}
