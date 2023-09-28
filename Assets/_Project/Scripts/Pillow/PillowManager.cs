using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillowManager : MonoBehaviour
{
    [SerializeField] private int delayBetweenPillowsInSeconds = 1800;
    public readonly int MaxPillowsCount = 500;
    private int currentPillowsCount = 1;
    public int CurrentPillowsCount
    {
        get => currentPillowsCount;
        private set => currentPillowsCount = value;
    }

    private float timeToNextPillow;

    public float TimeToNextPillow
    {
        get => timeToNextPillow;
        private set => timeToNextPillow = value;
    }
    public bool CanEarnPillow => currentPillowsCount < MaxPillowsCount;
    public bool HasPillow => currentPillowsCount > 0;

    public static event Action<int,int> OnPillowAmountChanged;

    private void OnEnable()
    {
        CardInfosUI.OnPressedStart += UsePillow;
        PillowShopItem.OnPillowItemPurchased += AddPillow;
    }

    private void OnDisable()
    {
        CardInfosUI.OnPressedStart -= UsePillow;
        PillowShopItem.OnPillowItemPurchased -= AddPillow;
    }

    private void Start()
    {
        SetupPillows();
    }

    private void Update()
    {
        if (!CanEarnPillow)
        {
            return;
        }

        TimeToNextPillow -= Time.deltaTime;

        if (TimeToNextPillow <= 0)
        {
            OnGetNewPillow();
        }
    }

    public void AddPillow(int pillowCount)
    {
        CurrentPillowsCount += pillowCount;
        CurrentPillowsCount = Mathf.Clamp(CurrentPillowsCount, 0, MaxPillowsCount);
        OnPillowAmountChanged?.Invoke(CurrentPillowsCount, MaxPillowsCount);
        SavePillowCount();
    }

    public void UsePillow(QuizCategory category)
    {
        CurrentPillowsCount--;
        OnPillowAmountChanged?.Invoke(CurrentPillowsCount, MaxPillowsCount);
        SavePillowCount();
    }

    public void SetupPillows()
    {
        if (!PlayerProgress.SaveState.hasSaveData)
        {
            CurrentPillowsCount = MaxPillowsCount;
            RestartPillowTimer();
        }
        else
        {
            CurrentPillowsCount = PlayerProgress.SaveState.playerInfo.currentPillowCount;
            CheckOfflinePillows();
        }

        RestartPillowTimer();

        OnPillowAmountChanged?.Invoke(currentPillowsCount, MaxPillowsCount);
    }

    private void CheckOfflinePillows()
    {
        if (!CanEarnPillow)
        {
            return;
        }

        double offlineSeconds = (DateTime.Now - PlayerProgress.LastSaveTime).TotalSeconds;

        while (offlineSeconds >= delayBetweenPillowsInSeconds)
        {
            if (CanEarnPillow)
            {
                OnGetNewPillow();
                offlineSeconds -= delayBetweenPillowsInSeconds;
                continue;
            }

            if (offlineSeconds < delayBetweenPillowsInSeconds)
            {
                TimeToNextPillow = (float)offlineSeconds;
            }

            break;
        }
    }

    private void OnGetNewPillow()
    {
        RestartPillowTimer();
        AddPillow(1);
    }

    private void RestartPillowTimer()
    {
        TimeToNextPillow = delayBetweenPillowsInSeconds;
    }

    public void SavePillowCount()
    {
        PlayerProgress.SaveState.playerInfo.currentPillowCount = CurrentPillowsCount;
        GameManager.Instance.SaveGame();
    }
}
