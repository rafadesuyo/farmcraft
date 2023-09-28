using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStageState : StageState
{
    public override StateName GetStateName()
    {
        return StateName.Tutorial;
    }
}
