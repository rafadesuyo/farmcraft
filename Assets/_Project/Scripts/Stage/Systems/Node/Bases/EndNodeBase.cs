using DreamQuiz.Player;

namespace DreamQuiz
{
    public class EndNodeBase : NodeBase
    {
        public override void AddPawnToNode(Pawn pawn)
        {
            base.AddPawnToNode(pawn);
            PawnArrived(pawn);
        }

        protected override string GetNodeName()
        {
            return $"NodeBase_EndOfStage_{transform.GetSiblingIndex()}";
        }

        private void PawnArrived(Pawn pawn)
        {
            var playerPawn = pawn as PlayerPawn;

            if (playerPawn != null)
            {
                playerPawn.PlayerStageData.SetEndNodeReached(true);
            }
        }

        public override NodeBaseModelType GetNodeBaseModelSkinType()
        {
            return NodeBaseModelType.EndOfStage;
        }

        private void PlayIdleWithFlowersAnimation()
        {
            nodeBaseModel.PlayAnimation(NodeBaseModel.NodeAnimation.Spawn, () => nodeBaseModel.PlayAnimation(NodeBaseModel.NodeAnimation.Win));
        }
    }
}