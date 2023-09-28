using DreamQuiz;
using UnityEngine;

public abstract class StageState
{
    protected const bool debug = true;

    public enum StateName
    {
        Start = 0,
        Turn,
        Tutorial,
        End
    }

    protected StageManager stageManager;

    public void Initialize(StageManager stageManager)
    {
        this.stageManager = stageManager;
        OnInitialize();
    }

    public abstract StateName GetStateName();

    public virtual void OnInitialize()
    {
        if (debug)
        {
            Debug.Log("[StageState] " + GetStateName() + " initialized");
        }
    }

    public virtual void OnEnter()
    {
        if (debug)
        {
            Debug.Log("[StageState] " + GetStateName() + "state entered");
        }
    }

    public virtual void OnLeave()
    {
        if (debug)
        {
            Debug.Log("[StageState] " + GetStateName() + "state left");
        }
    }
}
