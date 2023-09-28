using DreamQuiz.Player;
using UnityEngine;

namespace DreamQuiz
{
    public class PenaltyPawn : PawnAnimated
    {
        //Variables
        [Header("Variables")]
        [SerializeField] int sleepingTimeToRemove = 20;

        public override void SetPawnToNode(NodeBase nodeBase)
        {
            base.SetPawnToNode(nodeBase);
            nodeBase.OnPawnAdded += NodeBase_OnPawnAdded;
        }

        private void NodeBase_OnPawnAdded(Pawn pawn)
        {
            var playerPawn = pawn as PlayerPawn;

            if (playerPawn != null)
            {
                playerPawn.PlayerStageData.SleepingTime.Use(sleepingTimeToRemove);
                CurrentNode.OnPawnAdded -= NodeBase_OnPawnAdded;

                PlayDisappearingAnimation();
            }
        }

        public override string GetDescription()
        {
            return $"Lose {sleepingTimeToRemove} sleeping time.";
        }
    }
}