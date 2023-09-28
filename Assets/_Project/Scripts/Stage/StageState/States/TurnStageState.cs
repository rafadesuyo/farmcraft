using System;
using UnityEngine;

namespace DreamQuiz
{
    public class TurnStageState : StageState
    {
        TurnSystem turnSystem;

        public override StateName GetStateName()
        {
            return StateName.Turn;
        }

        public override void OnInitialize()
        {
            base.OnInitialize();

            if (StageSystemLocator.TryGetSystem(out turnSystem) == false)
            {
                Debug.LogError("[TurnStageState] State needs a TurnSystem on scene");
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();

            turnSystem.BeginTurn();
        }

        public override void OnLeave()
        {
            base.OnLeave();
        }
    }
}