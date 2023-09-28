public class GoldCurrencyGiver : StageRewardGiver
{
    public override CurrencyType GetCurrencyType()
    {
        return CurrencyType.Gold;
    }

    public override IRewardPackage GetReward(StageGoalProgress stageGoalProgress)
    {
        return new GoldRewardPackage(stageGoalProgress.RewardedValue);
    }
}
