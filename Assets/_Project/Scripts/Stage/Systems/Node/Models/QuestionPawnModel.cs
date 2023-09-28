using DG.Tweening;
using Spine;
using Spine.Unity;
using TMPro;
using UnityEngine;

namespace DreamQuiz
{
    public class QuestionPawnModel : MonoBehaviour
    {
        //Components
        [Header("Components")]
        [SerializeField] QuestionPawn questionPawn;
        [SerializeField] StagePawnModel stagePawnModel;

        [Space(10)]

        [SerializeField] TextMeshPro questionCountText;
        [SerializeField] private SkeletonAnimation signUnclearedSkeletonAnimation;
        [SerializeField] private SkeletonAnimation signClearedSkeletonAnimation;
        [SerializeField] private Transform signUnclearedContainer;
        [SerializeField] private Transform signClearedContainer;

        //Variables
        [Header("Variables")]
        [SerializeField] private bool showSignClearedSprite;

        [SerializeField] private AnimationReferenceAsset signUnclearedIdleAnimation;
        [SerializeField] private AnimationReferenceAsset signUnclearedDisappearingAnimation;

        [Space(10)]

        [SerializeField] private AnimationReferenceAsset signClearedSpawnAnimation;

        [Space(10)]

        [SerializeField] private float questionCountTextFadeDuration = 1;

        private void Awake()
        {
            PlaySignUnclearedIdleAnimation();
            PlayPawnSpawnAnimation();
        }

        private void OnEnable()
        {
            questionCountText.text = questionPawn.QuestionCount.ToString();
            questionPawn.OnSolved += QuestionPawn_OnSolved;
        }

        private void OnDisable()
        {
            questionPawn.OnSolved -= QuestionPawn_OnSolved;
        }

        private void QuestionPawn_OnSolved()
        {
            PlaySignUnclearedDisappearingAnimation();
            PlayPawnDisappearingAnimation();
        }

        private void PlayPawnSpawnAnimation()
        {
            stagePawnModel.gameObject.SetActive(true);
            stagePawnModel.PlayAnimation(StagePawnModel.PawnAnimation.Spawn, () => stagePawnModel.PlayAnimation(StagePawnModel.PawnAnimation.Idle));
        }

        private void PlayPawnDisappearingAnimation()
        {
            stagePawnModel.PlayAnimation(StagePawnModel.PawnAnimation.Disappearing, () => stagePawnModel.gameObject.SetActive(false));
        }

        private void PlaySignUnclearedIdleAnimation()
        {
            signUnclearedContainer.gameObject.SetActive(true);
            signClearedContainer.gameObject.SetActive(false);

            signUnclearedSkeletonAnimation.AnimationState.SetAnimation(0, signUnclearedIdleAnimation, true);

            Color currentQuestionCountTextColor = questionCountText.color;
            questionCountText.color = new Color(currentQuestionCountTextColor.r, currentQuestionCountTextColor.g, currentQuestionCountTextColor.b, 1);
        }

        private void PlaySignUnclearedDisappearingAnimation()
        {
            TrackEntry animationTrack = signUnclearedSkeletonAnimation.AnimationState.SetAnimation(0, signUnclearedDisappearingAnimation, false);
            animationTrack.Complete += (_) => PlaySignClearedSpawnAnimation();

            questionCountText.DOFade(0, questionCountTextFadeDuration);
        }

        private void PlaySignClearedSpawnAnimation()
        {
            signUnclearedContainer.gameObject.SetActive(false);
            signClearedContainer.gameObject.SetActive(showSignClearedSprite);

            if(showSignClearedSprite == true)
            {
                signClearedSkeletonAnimation.AnimationState.SetAnimation(0, signClearedSpawnAnimation, false);
            }
        }
    }
}