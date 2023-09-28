public class TotalGoldGoalEvaluator : StageGoalEvaluator
{
    public override StageGoal.StageGoalRequisite GetGoalRequisite()
    {
        return StageGoal.StageGoalRequisite.TotalGold;
    }

    public override void OnInitialize()
    {
        playerStageData.OnGoldCountChanged += PlayerStageData_OnGoldCountChanged;
    }

    public override void OnDispose()
    {
        playerStageData.OnGoldCountChanged -= PlayerStageData_OnGoldCountChanged;
    }

    private void PlayerStageData_OnGoldCountChanged(int value)
    {
        stageGoalProgress.UpdateValue(value);

        if (IsComplete())
        {
            stageGoalProgress.CompleteProgress();
        }
    }

    public override bool IsComplete()
    {
        return playerStageData.GoldCount >= stageGoalProgress.StageGoal.TargetValue;
    }
}
