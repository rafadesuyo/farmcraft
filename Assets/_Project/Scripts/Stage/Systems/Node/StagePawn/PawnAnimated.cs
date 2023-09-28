using UnityEngine;

namespace DreamQuiz
{
    public class PawnAnimated : Pawn
    {
        //Components
        [Header("Components")]
        [SerializeField] protected StagePawnModel stagePawnModel;

        protected override void Awake()
        {
            base.Awake();

            PlaySpawnAnimation();
        }

        protected void PlaySpawnAnimation()
        {
            stagePawnModel.PlayAnimation(StagePawnModel.PawnAnimation.Spawn, () => stagePawnModel.PlayAnimation(StagePawnModel.PawnAnimation.Idle));
        }

        protected void PlayDisappearingAnimation()
        {
            stagePawnModel.PlayAnimation(StagePawnModel.PawnAnimation.Disappearing, RemovePawnFromBoard);
        }
    }
}
