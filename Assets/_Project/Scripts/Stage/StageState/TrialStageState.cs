using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TrialStageState
{
    protected const bool isDebug = true;

    public enum StateName
    {
        Start = 0,
        Tutorial,
        Play,
        End
    }

    protected TrialStageManager trialStageManager;

    public void Initialize(TrialStageManager trialStageManager)
    {
        this.trialStageManager = trialStageManager;
        OnInitialize();
    }

    public abstract StateName GetStateName();

    public virtual void OnInitialize()
    {
        if (isDebug)
        {
            Debug.Log("[TrialStageState] " + GetStateName() + " initialized");
        }
    }

    public virtual void OnEnter()
    {
        if (isDebug)
        {
            Debug.Log("[TrialStageState] " + GetStateName() + "state entered");
        }
    }

    public virtual void OnLeave()
    {
        if (isDebug)
        {
            Debug.Log("[TrialStageState] " + GetStateName() + "state left");
        }
    }
}
