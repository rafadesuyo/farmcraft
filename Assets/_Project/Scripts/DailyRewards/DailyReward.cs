using UnityEngine;

[System.Serializable]
public class DailyReward
{
    //Enums
    public enum RewardState { CantBeCollected, CanBeCollected, WasCollected }

    //Variables
    [SerializeField] private Reward[] rewards;

    private RewardState state;

    //Getters
    public Reward[] Rewards => rewards;
    public RewardState State => state;

    public void LoadSave(DailyRewardProgress dailyRewardProgress)
    {
        state = dailyRewardProgress.state;
    }

    public void ResetReward()
    {
        state = RewardState.CantBeCollected;
    }

    public void UnlockReward()
    {
        state = RewardState.CanBeCollected;
    }

    public void CollectRewards()
    {
        foreach(Reward reward in rewards)
        {
            reward.CollectReward();
        }

        state = RewardState.WasCollected;
    }
}
