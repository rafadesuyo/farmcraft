using DreamQuiz;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCountdownSystem : BaseStageSystem
{
    private bool isCountingTime = false;

    [SerializeField] private int targetTime = 120;
    public int TargetTime => targetTime;

    private float elapsedTime;
    public float ElapsedTime => elapsedTime;

    private void Start()
    {
        SetTimeCount(targetTime);
    }

    private void Update()
    {
        if (isCountingTime)
        {
            elapsedTime -= Time.deltaTime;
            elapsedTime = Mathf.Clamp(elapsedTime, 0, targetTime);
        }

        if (HasTimerExpired())
        {
            isCountingTime = false;
        }
    }

    protected override void RegisterSystem()
    {
        StageSystemLocator.RegisterSystem(this);
        isCountingTime = false;
        IsReady = true;
    }

    protected override void UnregisterSystem()
    {
        StageSystemLocator.UnregisterSystem<TimeCountdownSystem>();
    }

    public void ResumeCountingTime()
    {
        if (IsReady == false)
        {
            return;
        }

        isCountingTime = true;
    }

    public void PauseCountingTime()
    {
        isCountingTime = false;
    }

    public void ResetCountingTime()
    {
        SetTimeCount(targetTime);
        isCountingTime = false;
    }

    private void SetTimeCount(float time)
    {
        elapsedTime = targetTime;
    }

    public bool HasTimerExpired()
    {
        return elapsedTime <= 0 && isCountingTime;
    }

    public float CalculateTimeProportion()
    {
        return elapsedTime / targetTime;
    }
}
