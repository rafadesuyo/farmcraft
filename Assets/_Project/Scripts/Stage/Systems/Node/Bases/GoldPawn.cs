using DreamQuiz.Player;
using UnityEngine;

namespace DreamQuiz
{
    public class GoldPawn : PawnAnimated
    {
        //Variables
        [Header("Variables")]
        [SerializeField] int goldAmount = 20;

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
                playerPawn.PlayerStageData.AddGoldCount(goldAmount);
                CurrentNode.OnPawnAdded -= NodeBase_OnPawnAdded;

                PlayDisappearingAnimation();
            }
        }

        public override string GetDescription()
        {
            return $"Earn {goldAmount} Gold.";
        }
    }
}
