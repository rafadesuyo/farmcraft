using DreamQuiz;
using System;
using UnityEngine;

public enum TargetState
{
    None = 0,
    Searching,
    Invalid,
    Acquired
}

public class TargetSystem : BaseStageSystem
{
    [SerializeReference] private ClickManager clickManager;

    protected override void RegisterSystem()
    {
        StageSystemLocator.RegisterSystem(this);
        IsReady = true;
    }

    protected override void UnregisterSystem()
    {
        StageSystemLocator.UnregisterSystem<TargetSystem>();
    }

    public void ListenForTarget<T>(Action<T, TargetState> onTargetAcquired) where T : ITargetable
    {
        clickManager.RedirectClick((gameObject) =>
        {
            TargetState targetState = TargetState.None;
            ITargetable target = null;

            if (gameObject != null)
            {
                target = gameObject.GetComponent<ITargetable>();

                if (target == null || target is not T)
                {
                    targetState = TargetState.Invalid;
                }
                else
                {
                    targetState = TargetState.Acquired;
                }
            }

            onTargetAcquired((T)target, targetState);
        });
    }
}
