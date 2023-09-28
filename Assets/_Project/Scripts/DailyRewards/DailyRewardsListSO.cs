using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Daily Rewards/Daily Rewards List")]
public class DailyRewardsListSO : ScriptableObject
{
    //Constants
    private const int daysBetweenRewards = 1;

    //Enums
    public enum DailyRewardsListState { Disabled, Enabled, Expired }

    //Variables
    [Header("Variables")]
    [SerializeField] private int maxDaysToGetRewards = 10;

    [Tooltip("If true, the daily rewards list will reset itself when expiring (after the \"Max Days To Get Rewards\" have passed).")]
    [SerializeField] private bool shouldResetListWhenExpiring;

    private DailyRewardsProgress dailyRewardsProgress = new DailyRewardsProgress();

    [Header("Daily Rewards")]
    [Tooltip("The daily rewards.\nThey will start being represented with the small container up.\nAfter the \"MediumRewardsStartIndex\" they will be represented with the medium container.\nAfter the \"BigRewardsStartIndex\" they will be represented with the big container.")]
    [SerializeField] private DailyReward[] dailyRewards;

    [Space(10)]

    [Tooltip("The index to start representing the daily rewards with the medium container.")]
    [SerializeField] private int mediumRewardsStartIndex = 10;

    [Tooltip("The index to start representing the daily rewards with the big container.")]
    [SerializeField] private int bigRewardsStartIndex = 19;

    //Getters
    public DailyRewardsProgress DailyRewardsProgress => dailyRewardsProgress;
    public DailyRewardsListState State => dailyRewardsProgress.state;
    public int RewardsUnlocked => dailyRewardsProgress.rewardsUnlocked;
    public DateTime StartDate => dailyRewardsProgress.startDate;
    public DateTime LastRewardUnlockedDate => dailyRewardsProgress.lastRewardUnlockedDate;
    public DailyReward[] DailyRewards => dailyRewards;
    public int MediumRewardsStartIndex => mediumRewardsStartIndex;
    public int BigRewardsStartIndex => bigRewardsStartIndex;

    private void OnValidate()
    {
        if(mediumRewardsStartIndex < 0)
        {
            mediumRewardsStartIndex = 0;
        }

        if(bigRewardsStartIndex <= mediumRewardsStartIndex)
        {
            bigRewardsStartIndex = mediumRewardsStartIndex + 1;
        }
    }

    public void LoadSave(DailyRewardsListSOSave dailyRewardsListSOSave)
    {
        if (dailyRewardsListSOSave == null)
        {
            return;
        }

        dailyRewardsProgress = dailyRewardsListSOSave.dailyRewardsProgress;

        for (int i = 0; i < dailyRewards.Length; i++)
        {
            if (i >= dailyRewardsListSOSave.dailyRewards.Length)
            {
                Debug.LogWarning($"The list from the Daily Rewards List SO Save is smaller than the one in \"{this.name}\"!");
                break;
            }

            dailyRewards[i].LoadSave(dailyRewardsListSOSave.dailyRewards[i]);
        }
    }

    public void ResetRewardsList()
    {
        dailyRewardsProgress.ResetDailyRewardsProgress();

        ResetRewards();
    }

    public void UpdateDailyRewardsList()
    {
        EnableRewardsList();
        CheckRewards();
        CheckIfCanResetAfterExpiring();
    }

    private void EnableRewardsList()
    {
        if(dailyRewardsProgress.state != DailyRewardsListState.Disabled)
        {
            return;
        }

        dailyRewardsProgress.state = DailyRewardsListState.Enabled;
        dailyRewardsProgress.rewardsUnlocked = 0;

        dailyRewardsProgress.startDate = DailyRewardsManager.Instance.GetCurrentDateTime();
        dailyRewardsProgress.lastRewardUnlockedDate = DailyRewardsManager.Instance.GetCurrentDateTime();

        ResetRewards();

        UnlockReward();
    }

    private void CheckRewards()
    {
        if(dailyRewardsProgress.state != DailyRewardsListState.Enabled)
        {
            return;
        }

        if(CheckIfRewardsHaveExpired() == true)
        {
            return;
        }

        CheckIfCanUnlockReward();
    }

    private void CheckIfCanResetAfterExpiring()
    {
        if (dailyRewardsProgress.state != DailyRewardsListState.Expired)
        {
            return;
        }

        if (shouldResetListWhenExpiring == true)
        {
            ResetRewardsList();
            UpdateDailyRewardsList();
        }
    }

    public bool CanRewardsBeCollected()
    {
        foreach(DailyReward dailyReward in dailyRewards)
        {
            if(dailyReward.State == DailyReward.RewardState.CanBeCollected)
            {
                return true;
            }
        }

        return false;
    }

    public int GetDaysLeftToCollectRewards()
    {
        DateTime currentDate = DailyRewardsManager.Instance.GetCurrentDateTime();
        double days = maxDaysToGetRewards - (currentDate - dailyRewardsProgress.startDate).TotalDays;

        if(days >= 0)
        {
            return (int)days;
        }
        else
        {
            return 0;
        }
    }

    private double GetDaysSinceLastRewardUnlocked()
    {
        DateTime currentDate = DailyRewardsManager.Instance.GetCurrentDateTime();
        return (currentDate - dailyRewardsProgress.lastRewardUnlockedDate).TotalDays;
    }

    private bool CheckIfRewardsHaveExpired()
    {
        if(GetDaysLeftToCollectRewards() <= 0)
        {
            dailyRewardsProgress.state = DailyRewardsListState.Expired;
            return true;
        }

        return false;
    }

    private void CheckIfCanUnlockReward()
    {
        if (GetDaysSinceLastRewardUnlocked() >= daysBetweenRewards)
        {
            UnlockReward();
        }
    }

    private void ResetRewards()
    {
        foreach(DailyReward dailyReward in dailyRewards)
        {
            dailyReward.ResetReward();
        }
    }

    private void UnlockReward()
    {
        if(dailyRewardsProgress.rewardsUnlocked >= dailyRewards.Length)
        {
            return;
        }

        dailyRewards[dailyRewardsProgress.rewardsUnlocked].UnlockReward();

        dailyRewardsProgress.rewardsUnlocked++;

        dailyRewardsProgress.lastRewardUnlockedDate = DailyRewardsManager.Instance.GetCurrentDateTime();
    }
}
