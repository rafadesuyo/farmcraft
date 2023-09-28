using DreamQuiz.Player;

public abstract class StageGoalEvaluator
{
    protected PlayerStageData playerStageData;
    protected StageGoalProgress stageGoalProgress;

    public abstract StageGoal.StageGoalRequisite GetGoalRequisite();

    public void InitializeEvaluator(PlayerStageData playerStageData, StageGoalProgress stageGoalProgress)
    {
        this.playerStageData = playerStageData;
        this.stageGoalProgress = stageGoalProgress;

        OnInitialize();
    }

    public abstract void OnInitialize();

    public abstract void OnDispose();

    public abstract bool IsComplete();
}
