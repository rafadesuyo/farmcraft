public class ReachEndNodeGoalEvaluator : StageGoalEvaluator
{
    public override StageGoal.StageGoalRequisite GetGoalRequisite()
    {
        return StageGoal.StageGoalRequisite.ReachEndNode;
    }

    public override bool IsComplete()
    {
        return stageGoalProgress.ProgressValue > 0;
    }

    public override void OnDispose()
    {
        playerStageData.OnReachedEndNode -= PlayerStageData_OnReachedEndNode;
    }

    public override void OnInitialize()
    {
        playerStageData.OnReachedEndNode += PlayerStageData_OnReachedEndNode;
    }

    private void PlayerStageData_OnReachedEndNode()
    {
        stageGoalProgress.UpdateValue(1);

        if (IsComplete())
        {
            stageGoalProgress.CompleteProgress();
        }
    }
}
