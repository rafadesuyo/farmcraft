public abstract class StageRewardGiver
{
    public abstract CurrencyType GetCurrencyType();
    public abstract IRewardPackage GetReward(StageGoalProgress stageGoalProgress);
}
