using DreamQuiz.Player;
using UnityEngine;

namespace DreamQuiz
{
    public class WallNodeBase : NodeBase
    {
        //Components
        [Header("Components")]
        [SerializeField] private WallNodeBaseModel wallModel;

        protected override void Awake()
        {
            base.Awake();

            isBlocked = true;

            wallModel.PlayIdleAnimation();
        }

        public override bool CanInteractWithPawn(Pawn pawn, out string message)
        {
            message = string.Empty;
            var playerPawn = pawn as PlayerPawn;

            if (playerPawn != null)
            {
                if (IsBlocked == false)
                {
                    return true;
                }

                if(playerPawn.PlayerStageData.PowerCount > 0)
                {
                    return true;
                }
            }

            message = $"You need POWER to destroy the WALL";
            return false;
        }

        private void PawnArrived(Pawn pawn)
        {
            if (IsBlocked == false)
            {
                return;
            }

            var playerPawn = pawn as PlayerPawn;

            if (playerPawn != null)
            {
                if (playerPawn.PlayerStageData.PowerCount > 0)
                {
                    playerPawn.PlayerStageData.RemovePowerCount(1);

                    DestroyWall();
                }
                else
                {
                    playerPawn.ReturnToPreviousNode();
                }
            }
        }

        public void DestroyWall()
        {
            isBlocked = false;
            wallModel.PlayDestroyedAnimation();
        }

        protected override string GetNodeName()
        {
            return $"NodeBase_Wall_{transform.GetSiblingIndex()}";
        }

        public override string GetNodeDescription()
        {
            return "Needs power to break it.";
        }

        public override void AddPawnToNode(Pawn pawn)
        {
            base.AddPawnToNode(pawn);
            PawnArrived(pawn);
        }
    }
}