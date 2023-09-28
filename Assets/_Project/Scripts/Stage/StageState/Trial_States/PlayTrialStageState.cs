using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTrialStageState : TrialStageState
{
    public override StateName GetStateName()
    {
        return StateName.Play;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnLeave()
    {
        base.OnLeave();
    }
}
