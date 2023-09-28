using DreamQuiz;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrackingSystem : BaseStageSystem
{
    private float currentDelta = 0;
    private float elapsedTime = 0;
    private int elapsedTimeInt = 0;
    private bool isCountingTime = false;

    public event Action OnStartCount;
    public event Action OnResumeCount;
    public event Action OnPauseCount;
    public event Action OnResetCount;
    public event Action<int> OnTimeCount;

    public int ElapsedTime
    {
        get
        {
            return elapsedTimeInt;
        }
    }

    private void Update()
    {
        currentDelta = Time.deltaTime;
        UpdateTimeCount();
    }

    protected override void RegisterSystem()
    {
        StageSystemLocator.RegisterSystem(this);
        isCountingTime = false;
        IsReady = true;
    }

    protected override void UnregisterSystem()
    {
        StageSystemLocator.UnregisterSystem<TimeTrackingSystem>();
    }

    public void StartCountingTime()
    {
        if (IsReady == false)
        {
            return;
        }

        elapsedTime = 0;
        isCountingTime = true;
        OnStartCount?.Invoke();
    }

    public void ResumeCountingTime()
    {
        isCountingTime = true;
        OnResumeCount?.Invoke();
    }

    public void PauseCountingTime()
    {
        isCountingTime = false;
        OnPauseCount?.Invoke();
    }

    public void ResetCountingTime()
    {
        SetTimeCount(0);
        OnResetCount?.Invoke();
    }

    private void UpdateTimeCount()
    {
        if (isCountingTime == false)
        {
            return;
        }

        SetTimeCount(elapsedTime + currentDelta);
    }

    private void SetTimeCount(float time)
    {
        elapsedTime = time;
        int newStageTimeInt = Mathf.CeilToInt(elapsedTime);

        if (newStageTimeInt != elapsedTimeInt)
        {
            elapsedTimeInt = newStageTimeInt;
            OnTimeCount?.Invoke(elapsedTimeInt);
        }
    }
}