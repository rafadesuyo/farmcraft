using UnityEngine;

[System.Serializable]
public class StageGoal
{
    public enum StageGoalRequisite
    {
        ReachEndNode = 0,
        SheepSaved,
        CorrectAnswers,
        SecondsToFinishTheStage,
        WolvesBeaten,
        SleepingTime,
        TotalGold
    }

    [SerializeField] private StageGoalRequisite requisite;
    [SerializeField] private int targetValue;
    [SerializeField] private bool mainGoal;
    [SerializeField] GoalReward goalReward;

    public StageGoalRequisite Requisite => requisite;
    public int TargetValue => targetValue;
    public bool MainGoal => mainGoal;
    public GoalReward GoalReward => goalReward;

    public StageGoal(StageGoalRequisite requisite, int targetValue, bool mainGoal, GoalReward goalReward)
    {
        this.requisite = requisite;
        this.targetValue = targetValue;
        this.mainGoal = mainGoal;
        this.goalReward = goalReward;
    }
}
