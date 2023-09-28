using System;

[Serializable]
public class DailyRewardsListSOSave
{
    public string key;

    public DailyRewardsProgress dailyRewardsProgress;
    public DailyRewardProgress[] dailyRewards;

    public DailyRewardsListSOSave() { } //Necessary for the JSON Deserializer to work

    public DailyRewardsListSOSave(DailyRewardsListSO dailyRewardsListSO)
    {
        key = dailyRewardsListSO.name;

        dailyRewardsProgress = dailyRewardsListSO.DailyRewardsProgress;

        dailyRewards = new DailyRewardProgress[dailyRewardsListSO.DailyRewards.Length];

        for (int i = 0; i < dailyRewardsListSO.DailyRewards.Length; i++)
        {
            dailyRewards[i] = new DailyRewardProgress(dailyRewardsListSO.DailyRewards[i]);
        }
    }
}
