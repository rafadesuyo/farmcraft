public class SecondsToFinishTheStageGoalEvaluator : StageGoalEvaluator
{
    TimeTrackingSystem timeSystem;
    int timeTracked;

    public override StageGoal.StageGoalRequisite GetGoalRequisite()
    {
        return StageGoal.StageGoalRequisite.SecondsToFinishTheStage;
    }

    public override void OnInitialize()
    {
        TimeTrackingSystem timeSystem = StageSystemLocator.GetSystem<TimeTrackingSystem>();
        timeSystem.OnTimeCount += TimeSystem_OnTimeCount;
    }

    private void TimeSystem_OnTimeCount(int timeCount)
    {
        timeTracked = timeCount;
    }

    public override void OnDispose()
    {
        timeSystem.OnTimeCount -= TimeSystem_OnTimeCount;
    }

    public override bool IsComplete()
    {
        return timeTracked < stageGoalProgress.StageGoal.TargetValue;
    }
}