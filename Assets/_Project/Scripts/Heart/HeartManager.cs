using UnityEngine;
using System;

public class HeartManager : LocalSingleton<HeartManager>
{
    public readonly int MaxHeartCount = 5;
    private int currentHeartCount = 0;

    private int DelayBetweenHeartsInSeconds
    {
        get
        {
            return GameDifficultyHandler.Instance.GetHeartTime();
        }
    }

    public float TimeToNextHeart
    {
        get;
        private set;
    }

    public bool CanEarnFreeHeart
    {
        get
        {
            return CurrentHeartCount < MaxHeartCount;
        }
    }

    public bool HasHeart
    {
        get
        {
            return CurrentHeartCount > 0;
        }
    }

    public int CurrentHeartCount
    {
        get
        {
            return currentHeartCount;
        }
        private set
        {
            currentHeartCount = value;
            EventsManager.Publish(EventsManager.onHeartChange);
        }
    }

    private void OnEnable()
    {
        SetupHearts();
    }

    public void AddHearts(int heartCount, bool ignoreHeartLimit = false)
    {
        if (ignoreHeartLimit)
        {
            CurrentHeartCount += heartCount;
        }
        else if (CurrentHeartCount + heartCount <= MaxHeartCount)
        {
            CurrentHeartCount += heartCount;
        }
    }

    public void UseHeart()
    {
        CurrentHeartCount--;
    }

    public void SetupHearts()
    {
        if (!PlayerProgress.SaveState.hasSaveData)
        {
            CurrentHeartCount = MaxHeartCount;
        }
        else
        {
            CurrentHeartCount = PlayerProgress.SaveState.playerInfo.currentHeartCount;
            CheckOfflineHearts();
        }

        RestartHeartTimer();
    }

    private void Update()
    {
        if (!CanEarnFreeHeart)
        {
            return;
        }

        TimeToNextHeart -= Time.deltaTime;

        if (TimeToNextHeart <= 0)
        {
            OnGetNewHeart();
        }
    }

    private void CheckOfflineHearts()
    {
        if (!CanEarnFreeHeart)
        {
            return;
        }

        double offlineSeconds = (DateTime.Now - PlayerProgress.LastSaveTime).TotalSeconds;

        while (offlineSeconds >= DelayBetweenHeartsInSeconds)
        {
            if (CanEarnFreeHeart)
            {
                OnGetNewHeart();
                offlineSeconds -= DelayBetweenHeartsInSeconds;
                continue;
            }

            if (offlineSeconds < DelayBetweenHeartsInSeconds)
            {
                TimeToNextHeart = (float)offlineSeconds;
            }

            break;
        }
    }

    private void OnGetNewHeart()
    {
        RestartHeartTimer();
        AddHearts(1);
    }

    private void RestartHeartTimer()
    {
        TimeToNextHeart = DelayBetweenHeartsInSeconds;
    }

    public int TimeLeftInSecondsToFullHearts()
    {
        int countToMaxHearts = MaxHeartCount - CurrentHeartCount;

        int timeLeftToMax = countToMaxHearts * DelayBetweenHeartsInSeconds;

        return timeLeftToMax;
    }
}
