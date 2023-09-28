using DreamQuiz.Player;
using UnityEngine;

namespace DreamQuiz
{
    public class SleepingTimePawn : PawnAnimated
    {
        //Variables
        [Header("Variables")]
        [SerializeField] int sleepingTimeToAdd = 20;

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
                playerPawn.PlayerStageData.SleepingTime.Add(sleepingTimeToAdd);
                CurrentNode.OnPawnAdded -= NodeBase_OnPawnAdded;

                PlayDisappearingAnimation();
            }
        }

        public override string GetDescription()
        {
            return $"Earn {sleepingTimeToAdd} sleeping time.";
        }
    }
}