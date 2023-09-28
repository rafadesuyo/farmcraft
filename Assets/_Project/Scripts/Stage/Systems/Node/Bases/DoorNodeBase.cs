using DreamQuiz.Player;
using UnityEngine;

namespace DreamQuiz
{
    public class DoorNodeBase : NodeBase
    {
        //Components
        [Header("Components")]
        [SerializeField] private DoorNodeBaseModel doorModel;

        protected override void Awake()
        {
            base.Awake();

            isBlocked = true;

            doorModel.PlayClosedAnimation();
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

                if (playerPawn.PlayerStageData.KeyCount > 0)
                {
                    return true;
                }
            }

            message = $"You need a KEY to open the DOOR";
            return false;
        }

        public override void AddPawnToNode(Pawn pawn)
        {
            base.AddPawnToNode(pawn);
            PawnArrived(pawn);
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
                if (playerPawn.PlayerStageData.KeyCount > 0)
                {
                    playerPawn.PlayerStageData.RemoveKeyCount(1);

                    OpenDoor();
                }
                else
                {
                    playerPawn.ReturnToPreviousNode();
                }
            }
        }

        protected override string GetNodeName()
        {
            return $"NodeBase_Door_{transform.GetSiblingIndex()}";
        }

        public override string GetNodeDescription()
        {
            return "Needs a key to be opened.";
        }

        public void OpenDoor()
        {
            isBlocked = false;
            doorModel.PlayOpeningAnimation();
        }
    }
}