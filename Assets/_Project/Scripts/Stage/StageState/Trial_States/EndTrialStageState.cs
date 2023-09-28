using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrialStageState : TrialStageState
{
    public override StateName GetStateName()
    {
        return StateName.End;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }
}
