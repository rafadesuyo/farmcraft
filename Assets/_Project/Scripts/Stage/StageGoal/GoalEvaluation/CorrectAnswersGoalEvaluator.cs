using DreamQuiz.Player;

public class CorrectAnswersGoalEvaluator : StageGoalEvaluator
{
    public override StageGoal.StageGoalRequisite GetGoalRequisite()
    {
        return StageGoal.StageGoalRequisite.CorrectAnswers;
    }

    public override void OnInitialize()
    {
        playerStageData.OnCorrectAnswer += PlayerStageData_OnCorrectAnswer;
    }

    public override void OnDispose()
    {
        playerStageData.OnCorrectAnswer -= PlayerStageData_OnCorrectAnswer;
    }

    private void PlayerStageData_OnCorrectAnswer(PlayerAnswerEventArgs playerAnswerEventArgs)
    {
        stageGoalProgress.UpdateValue(playerAnswerEventArgs.TotalCorrectAnswer);

        if (IsComplete())
        {
            stageGoalProgress.CompleteProgress();
        }
    }

    public override bool IsComplete()
    {
        return playerStageData.TotalCorrectAnswers >= stageGoalProgress.StageGoal.TargetValue;
    }
}
