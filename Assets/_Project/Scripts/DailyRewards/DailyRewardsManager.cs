using System;
using UnityEngine;

public class DailyRewardsManager : LocalSingleton<DailyRewardsManager>
{
    //Variables
    [Header("Variables")]
    [SerializeField] private DailyRewardsListSO[] rewards;

#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField] private int daysToAddToCurrentDateTime;
    [SerializeField] private bool updateDailyRewardsListsWithSpace;
#endif

    //Getters
    public DailyRewardsListSO[] Rewards => rewards;

#if UNITY_EDITOR
    private void Update()
    {
        if(updateDailyRewardsListsWithSpace == true)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                UpdateDailyRewardsLists();
            }
        }
    }
#endif

    private void OnEnable()
    {
        InitializeDailyRewards();
    }

    public void InitializeDailyRewards()
    {
        ResetDailyRewardsLists();

        LoadDailyRewardsManagerSave();

        UpdateDailyRewardsLists();
    }

    private void LoadDailyRewardsManagerSave()
    {
        foreach(DailyRewardsListSO dailyRewardsList in rewards)
        {
            DailyRewardsListSOSave save = GetDailyRewardsListSave(dailyRewardsList);

            dailyRewardsList.LoadSave(save);
        }
    }

    private DailyRewardsListSOSave GetDailyRewardsListSave(DailyRewardsListSO dailyRewardsList)
    {
        DailyRewardsListSOSave[] rewardsSaves = PlayerProgress.DailyRewardsManagerSave.rewards;

        if(rewardsSaves == null)
        {
            return null;
        }

        foreach (DailyRewardsListSOSave dailyRewardsListSOSave in rewardsSaves)
        {
            if (dailyRewardsListSOSave.key == dailyRewardsList.name)
            {
                return dailyRewardsListSOSave;
            }
        }

        return null;
    }

    private void ResetDailyRewardsLists()
    {
        foreach(DailyRewardsListSO dailyRewardsList in rewards)
        {
            dailyRewardsList.ResetRewardsList();
        }
    }

    public void UpdateDailyRewardsLists()
    {
        foreach (DailyRewardsListSO dailyRewardsList in rewards)
        {
            dailyRewardsList.UpdateDailyRewardsList();
        }

        GameManager.Instance.SaveGame();
    }

    public DateTime GetCurrentDateTime()
    {
        //DateTime.Today is used so only the day matters for the "time" comparison

#if UNITY_EDITOR
        return DateTime.Today.AddDays(daysToAddToCurrentDateTime);
#else
        return DateTime.Today;
#endif
    }
}

[System.Serializable]
public class DailyRewardsManagerSave
{
    public DailyRewardsListSOSave[] rewards;

    public DailyRewardsManagerSave() { } //Necessary for the JSON Deserializer to work

    public DailyRewardsManagerSave(DailyRewardsManager dailyRewardsManager)
    {
        rewards = new DailyRewardsListSOSave[dailyRewardsManager.Rewards.Length];

        for(int i = 0; i < dailyRewardsManager.Rewards.Length; i++)
        {
            rewards[i] = new DailyRewardsListSOSave(dailyRewardsManager.Rewards[i]);
        }
    }
}
