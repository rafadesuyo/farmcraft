public class WolvesBeatenGoalEvaluator : StageGoalEvaluator
{
    public override StageGoal.StageGoalRequisite GetGoalRequisite()
    {
        return StageGoal.StageGoalRequisite.WolvesBeaten;
    }

    public override void OnInitialize()
    {
        playerStageData.OnWolvesCollectedChanged += PlayerStageData_OnWolvesCollectedChanged;
    }

    public override void OnDispose()
    {
        playerStageData.OnWolvesCollectedChanged -= PlayerStageData_OnWolvesCollectedChanged;
    }

    private void PlayerStageData_OnWolvesCollectedChanged(int value)
    {
        stageGoalProgress.UpdateValue(value);

        if (IsComplete())
        {
            stageGoalProgress.CompleteProgress();
        }
    }

    public override bool IsComplete()
    {
        return playerStageData.WolvesCollected >= stageGoalProgress.StageGoal.TargetValue;
    }
}
