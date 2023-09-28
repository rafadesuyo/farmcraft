using System;

[Serializable]
public class DailyRewardsProgress
{
    public DailyRewardsListSO.DailyRewardsListState state = DailyRewardsListSO.DailyRewardsListState.Disabled;
    public int rewardsUnlocked;
    public DateTime startDate;
    public DateTime lastRewardUnlockedDate;

    public void ResetDailyRewardsProgress()
    {
        state = DailyRewardsListSO.DailyRewardsListState.Disabled;
        rewardsUnlocked = 0;
        startDate = default;
        lastRewardUnlockedDate = default;
    }
}
