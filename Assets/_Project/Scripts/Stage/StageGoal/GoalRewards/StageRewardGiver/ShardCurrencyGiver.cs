public class ShardCurrencyGiver : StageRewardGiver
{
    public override CurrencyType GetCurrencyType()
    {
        return CurrencyType.Shard;
    }

    public override IRewardPackage GetReward(StageGoalProgress stageGoalProgress)
    {
        return new ShardRewardPackage(stageGoalProgress.RewardedValue, stageGoalProgress.StageGoal.GoalReward.CollectibleType);
    }
}
