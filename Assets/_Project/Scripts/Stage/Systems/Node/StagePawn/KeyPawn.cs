using DreamQuiz.Player;
using UnityEngine;

namespace DreamQuiz
{
    public class KeyPawn : PawnAnimated
    {
        //Variables
        [Header("Variables")]
        [SerializeField] int keyAmount = 1;

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
                playerPawn.PlayerStageData.AddKeyCount(keyAmount);
                CurrentNode.OnPawnAdded -= NodeBase_OnPawnAdded;

                PlayDisappearingAnimation();
            }
        }

        public override string GetDescription()
        {
            return "Earn a key that open doors.";
        }
    }
}