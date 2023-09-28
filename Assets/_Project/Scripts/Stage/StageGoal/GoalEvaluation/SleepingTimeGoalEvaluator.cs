public class SleepingTimeGoalEvaluator : StageGoalEvaluator
{
    public override StageGoal.StageGoalRequisite GetGoalRequisite()
    {
        return StageGoal.StageGoalRequisite.SleepingTime;
    }

    public override void OnInitialize()
    {
        playerStageData.SleepingTime.OnSleepingTimeChanged += PlayerStageData_OnSleepingTimeChanged;
    }

    public override void OnDispose()
    {
        playerStageData.SleepingTime.OnSleepingTimeChanged -= PlayerStageData_OnSleepingTimeChanged;
    }

    private void PlayerStageData_OnSleepingTimeChanged(int value)
    {
        stageGoalProgress.UpdateValue(value);

        if (IsComplete())
        {
            stageGoalProgress.CompleteProgress();
        }
    }

    public override bool IsComplete()
    {
        return playerStageData.SleepingTime.CurrentValue >= stageGoalProgress.StageGoal.TargetValue;
    }
}
