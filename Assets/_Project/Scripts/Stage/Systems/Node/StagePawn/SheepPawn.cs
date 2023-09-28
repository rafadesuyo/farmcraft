using DreamQuiz.Player;
using UnityEngine;

namespace DreamQuiz
{
    public class SheepPawn : PawnAnimated
    {
        //Variables
        [Header("Variables")]
        [SerializeField] int sheepAmount = 1;

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
                playerPawn.PlayerStageData.AddSheepsCollected(sheepAmount);
                CurrentNode.OnPawnAdded -= NodeBase_OnPawnAdded;

                PlayDisappearingAnimation();
            }
        }

        public override string GetDescription()
        {
            return "Save a sheep.";
        }
    }
}