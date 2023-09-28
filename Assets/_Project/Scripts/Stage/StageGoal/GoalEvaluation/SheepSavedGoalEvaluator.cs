public class SheepSavedGoalEvaluator : StageGoalEvaluator
{
    public override StageGoal.StageGoalRequisite GetGoalRequisite()
    {
        return StageGoal.StageGoalRequisite.SheepSaved;
    }

    public override void OnInitialize()
    {
        playerStageData.OnSheepsCollectedChanged += PlayerStageData_OnSheepsCollectedChanged;
    }

    public override void OnDispose()
    {
        playerStageData.OnSheepsCollectedChanged -= PlayerStageData_OnSheepsCollectedChanged;
    }

    private void PlayerStageData_OnSheepsCollectedChanged(int value)
    {
        stageGoalProgress.UpdateValue(value);

        if (IsComplete())
        {
            stageGoalProgress.CompleteProgress();
        }
    }

    public override bool IsComplete()
    {
        return playerStageData.SheepsCollected >= stageGoalProgress.StageGoal.TargetValue;
    }
}
