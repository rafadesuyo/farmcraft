using System;
using System.Collections.Generic;

[Serializable]
public class DailyMission
{
    //Enums
    public enum MissionState { Active, CanBeCompleted, Completed }

    //Variables
    private DailyMissionSO dailyMissionSO;

    //The progress is an array so that missions can have multiple requirements. Most missions will probably have just one.
    private DailyMissionProgress[] currentMissionProgress;
    private DailyMissionProgress[] requiredMissionProgress;

    private MissionState state;

    //Getters
    public DailyMissionSO DailyMissionSO => dailyMissionSO;
    public DailyMissionProgress[] CurrentMissionProgress => currentMissionProgress;
    public DailyMissionProgress[] RequiredMissionProgress => requiredMissionProgress;
    public MissionState State => state;

    public string MissionName => dailyMissionSO.MissionName;
    public string Description => dailyMissionSO.GetDescription(this);
    public Reward[] Rewards => dailyMissionSO.Rewards;

    public string GetProgressText()
    {
        return dailyMissionSO.GetProgressText(currentMissionProgress, requiredMissionProgress);
    }

    public DailyMission(DailyMissionSO dailyMissionSO)
    {
        this.dailyMissionSO = dailyMissionSO;

        currentMissionProgress = dailyMissionSO.GetInitialMissionProgress();
        requiredMissionProgress = dailyMissionSO.GetRequiredMissionProgress(currentMissionProgress);

        SetState(MissionState.Active);

        //Check after creating the mission, to check missions with requirements tied to persistent progress
        CheckIfCanBeCompleted();
    }

    public DailyMission(DailyMissionSave dailyMissionSave, List<DailyMissionSO> dailyMissionTypes)
    {
        dailyMissionSO = GetDailyMissionSOFromKey(dailyMissionTypes, dailyMissionSave.dailyMissionSOKey);

        currentMissionProgress = dailyMissionSave.currentMissionProgress;
        requiredMissionProgress = dailyMissionSave.requiredMissionProgress;

        SetState(dailyMissionSave.state);
    }

    public void CollectRewards()
    {
        foreach (Reward reward in dailyMissionSO.Rewards)
        {
            reward.CollectReward();
        }

        CompleteMission();
    }

    private void SetStateToCanBeCompleted()
    {
        SetState(MissionState.CanBeCompleted);

        EventsManager.Publish(EventsManager.onMissionCanBeCompleted, new OnMissionStateChangedEvent(this));

        GameManager.Instance.SaveGame();
    }

    private void CompleteMission()
    {
        SetState(MissionState.Completed);

        EventsManager.Publish(EventsManager.onMissionIsCompleted, new OnMissionStateChangedEvent(this));

        GameManager.Instance.SaveGame();
    }

    public void DisableMission()
    {
        UnsubscribeFromMissionEvent();
    }

    private void SetState(MissionState state)
    {
        this.state = state;

        switch(this.state)
        {
            case MissionState.Active:
                SubscribeToMissionEvent();
                break;

            case MissionState.CanBeCompleted:
                UnsubscribeFromMissionEvent();
                break;

            case MissionState.Completed:
                UnsubscribeFromMissionEvent();
                break;
        }
    }

    private void SubscribeToMissionEvent()
    {
        EventsManager.AddListener(dailyMissionSO.GetMissionEventName(), UpdateMission);
    }

    private void UnsubscribeFromMissionEvent()
    {
        EventsManager.RemoveListener(dailyMissionSO.GetMissionEventName(), UpdateMission);
    }

    private void UpdateMission(IGameEvent gameEvent)
    {
        dailyMissionSO.HandleMissionUpdate(currentMissionProgress, gameEvent);

        CheckIfCanBeCompleted();
    }

    private void CheckIfCanBeCompleted()
    {
        if (CanBeCompleted() == true)
        {
            SetStateToCanBeCompleted();
        }
    }

    private bool CanBeCompleted()
    {
        return dailyMissionSO.CanDailyMissionBeCompleted(currentMissionProgress, requiredMissionProgress);
    }

    public DailyMissionSO GetDailyMissionSOFromKey(List<DailyMissionSO> dailyMissionTypes, string key)
    {
        foreach (DailyMissionSO dailyMissionSO in dailyMissionTypes)
        {
            if (dailyMissionSO.name == key)
            {
                return dailyMissionSO;
            }
        }

        throw new Exception($"There's no DailyMissionSO with the name \"{key}\" in the daily mission types list!");
    }

    [Serializable]
    public class DailyMissionProgress
    {
        public int targetType;
        public int targetValue;

        public DailyMissionProgress(int targetType, int targetValue)
        {
            this.targetType = targetType;
            this.targetValue = targetValue;
        }
    }
}

[Serializable]
public class DailyMissionSave
{
    public string dailyMissionSOKey;

    public DailyMission.DailyMissionProgress[] currentMissionProgress;
    public DailyMission.DailyMissionProgress[] requiredMissionProgress;

    public DailyMission.MissionState state;

    public DailyMissionSave() { } //Necessary for the JSON Deserializer to work

    public DailyMissionSave(DailyMission dailyMission)
    {
        dailyMissionSOKey = dailyMission.DailyMissionSO.name;

        currentMissionProgress = dailyMission.CurrentMissionProgress;
        requiredMissionProgress = dailyMission.RequiredMissionProgress;

        state = dailyMission.State;
    }
}