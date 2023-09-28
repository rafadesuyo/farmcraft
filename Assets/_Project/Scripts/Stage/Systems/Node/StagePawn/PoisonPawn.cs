using DreamQuiz.Player;
using UnityEngine;

namespace DreamQuiz
{
    public class PoisonPawn : PawnAnimated
    {
        //Variables
        [Header("Variables")]
        [SerializeField] private int poisonCount = 1;

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
                playerPawn.PlayerStageData.AddPoison(poisonCount);
                CurrentNode.OnPawnAdded -= NodeBase_OnPawnAdded;

                PlayDisappearingAnimation();
            }
        }

        public override string GetDescription()
        {
            return $"Debuff: Spend 50% more sleeping time when walking or answering wrong questions.";
        }
    }
}