using Spine;
using Spine.Unity;
using System;
using UnityEngine;

namespace DreamQuiz
{
    public class StagePawnModel : MonoBehaviour
    {
        //Components
        [Header("Components")]
        [SerializeField] private SkeletonAnimation pawnModelSkeletonAnimation;

        //Enums
        public enum PawnAnimation { Idle, Spawn, Disappearing }

        //Variables
        [Header("Variables")]
        [SerializeField] private AnimationReferenceAsset idleAnimation;
        [SerializeField] private AnimationReferenceAsset[] secondaryIdleAnimations;

        [Space(10)]

        [SerializeField] private AnimationReferenceAsset spawnAnimation;
        [SerializeField] private AnimationReferenceAsset disappearingAnimation;

        [Space(10)]

        [SerializeField] private float delayBetweenSecondaryIdleMin = 4;
        [SerializeField] private float delayBetweenSecondaryIdleMax = 8;

        private float timeToPlaySecondaryIdleMax;
        private float timeToPlaySecondaryIdle;
        private bool canUpdateTimeToPlaySecondaryIdle = false;

        private void Update()
        {
            if (canUpdateTimeToPlaySecondaryIdle == true)
            {
                UpdateTimeToPlaySecondaryIdle();
            }
        }

        public void PlayAnimation(PawnAnimation pawnAnimation, Action onAnimationEnd = null)
        {
            AnimationReferenceAsset animation;
            bool loop = pawnAnimation == PawnAnimation.Idle;

            switch (pawnAnimation)
            {
                case PawnAnimation.Idle:
                    animation = idleAnimation;
                    break;

                case PawnAnimation.Spawn:
                    animation = spawnAnimation;
                    break;

                case PawnAnimation.Disappearing:
                    animation = disappearingAnimation;
                    break;

                default:
                    throw new Exception($"The pawn animation \"{pawnAnimation}\" is invalid!");
            }

            TrackEntry animationTrack = pawnModelSkeletonAnimation.AnimationState.SetAnimation(0, animation, loop);
            animationTrack.Complete += (_) => onAnimationEnd?.Invoke();

            if(pawnAnimation == PawnAnimation.Idle && HasSecondaryIdle() == true)
            {
                animationTrack.Complete += (_) => CheckIfCanPlaySecondaryIdle();
                SetDelayToSecondaryIdle();
            }
            else
            {
                timeToPlaySecondaryIdle = 0;
                canUpdateTimeToPlaySecondaryIdle = false;
            }
        }

        private void PlaySecondaryIdle()
        {
            AnimationReferenceAsset secondaryIdleAnimation = secondaryIdleAnimations[UnityEngine.Random.Range(0, secondaryIdleAnimations.Length)];

            canUpdateTimeToPlaySecondaryIdle = false;

            TrackEntry animationTrack = pawnModelSkeletonAnimation.AnimationState.SetAnimation(0, secondaryIdleAnimation, false);
            animationTrack.Complete += (_) => PlayAnimation(PawnAnimation.Idle);
        }

        private void SetDelayToSecondaryIdle()
        {
            timeToPlaySecondaryIdleMax = UnityEngine.Random.Range(delayBetweenSecondaryIdleMin, delayBetweenSecondaryIdleMax);
            timeToPlaySecondaryIdle = 0;
            canUpdateTimeToPlaySecondaryIdle = true;
        }

        private void UpdateTimeToPlaySecondaryIdle()
        {
            timeToPlaySecondaryIdle += Time.deltaTime;
        }

        private void CheckIfCanPlaySecondaryIdle()
        {
            if (timeToPlaySecondaryIdle >= timeToPlaySecondaryIdleMax)
            {
                PlaySecondaryIdle();
            }
        }

        private bool HasSecondaryIdle()
        {
            return secondaryIdleAnimations != null && secondaryIdleAnimations.Length > 0;
        }
    }
}
