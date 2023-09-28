using DreamQuiz.Player;
using System;
using UnityEngine;

public abstract class BaseTargetableAbilityBehaviour<T> : BaseAbilityBehaviour where T : ITargetable
{
    protected TargetSystem targetSystem;
    protected TargetState targetState;
    public TargetState TargetState
    {
        get
        {
            return targetState;
        }
    }

    protected abstract bool OnTargetAcquired(T target);

    public override void OnInitialize()
    {
        base.OnInitialize();
        targetSystem = StageSystemLocator.GetSystem<TargetSystem>();
    }

    public override void UseAbility()
    {
        targetState = TargetState.Searching;
        targetSystem.ListenForTarget<T>(TargetAcquiredCallback);
        UpdateAbility();
    }

    private void TargetAcquiredCallback(T target, TargetState targetState)
    {
        this.targetState = targetState;

        if (this.targetState == TargetState.Acquired)
        {
            if (OnTargetAcquired(target))
            {
                ConsumeUsePerStage();
            }
        }
    }
}
