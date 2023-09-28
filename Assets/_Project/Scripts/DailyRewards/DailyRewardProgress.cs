using System;

[Serializable]
public class DailyRewardProgress
{
    public DailyReward.RewardState state;

    public DailyRewardProgress() { } //Necessary for the JSON Deserializer to work

    public DailyRewardProgress(DailyReward dailyReward)
    {
        state = dailyReward.State;
    }
}
