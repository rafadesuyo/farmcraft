using System;

[Serializable]
public class Achievement
{
    private AchievementSO data;
    private AchievementProgress progress;
    private bool unlocked = false;

    public AchievementSO Data => data;
    public bool Unlocked => unlocked;
    public bool CanCollectReward => unlocked && !progress.rewardCollected;
    public AchievementProgress Progress => progress;

    public Achievement(AchievementSO newData, AchievementProgress newProgress)
    {
        data = newData;
        progress = newProgress;

        Setup();
    }

    public void CollectReward()
    {
        if (progress.rewardCollected)
        {
            return;
        }

        progress.rewardCollected = true;
        PlayerProgress.TotalGold += data.GoldRewardValue;
        AudioManager.Instance.Play("Collect");

        switch (data.RewardType)
        {
            case AchievementReward.DreamEnergy:
                {
                    PlayerProgress.SaveState.playerInfo.maxDreamEnergy += data.RewardValue;
                    break;
                }
            case AchievementReward.SleepingTime:
                {
                    PlayerProgress.SaveState.playerInfo.maxSleepingTime += data.RewardValue;
                    break;
                }
            default:
                break;
        }

        GameManager.Instance.SaveGame();
    }

    private void Setup()
    {
        EvaluateProgress(true);
        EventsManager.AddListener(data.EventToListen, OnTriggerProgress);
    }

    private void EvaluateProgress(bool isFromLoad)
    {
        if (progress.value >= data.ProgressNeeded)
        {
            Unlock(isFromLoad);
        }
    }

    private void OnTriggerProgress(IGameEvent gameEvent)
    {
        if (unlocked)
        {
            return;
        }

        progress.value = data.GetProgress();
        EvaluateProgress(false);
    }

    private void Unlock(bool isFromLoad)
    {
        unlocked = true;

        if (!isFromLoad)
        {
            PopupManager.Instance.OpenAchievementPopup(data);
        }
    }
}
