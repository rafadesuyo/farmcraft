using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTrialStageState : TrialStageState
{
    public override StateName GetStateName()
    {
        return StateName.Start;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        var tutorialSystem = StageSystemLocator.GetSystem<TutorialSystem>();

        if (tutorialSystem != null)
        {
            trialStageManager.SetTrialStageState(StateName.Tutorial);
        }

        trialStageManager.SetTrialStageState(StateName.Play);
    }

    public override void OnLeave()
    {
        base.OnLeave();
    }
}
