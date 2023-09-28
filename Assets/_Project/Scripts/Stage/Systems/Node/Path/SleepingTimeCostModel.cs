using Spine;
using Spine.Unity;
using TMPro;
using UnityEngine;

namespace DreamQuiz
{
    public class SleepingTimeCostModel : MonoBehaviour
    {
        //Components
        [Header("Components")]
        [SerializeField] private NodePath nodePath;
        [SerializeField] private TextMeshPro costText;
        [SerializeField] private SkeletonAnimation signModelSkeletonAnimation;

        //Variables
        [Header("Variables")]
        [SerializeField] private AnimationReferenceAsset idleAnimation;
        [SerializeField] private AnimationReferenceAsset[] secondaryIdleAnimations;

        [Space(10)]

        [SerializeField] private float delayBetweenSecondaryIdleMin = 4;
        [SerializeField] private float delayBetweenSecondaryIdleMax = 8;

        private float timeToPlaySecondaryIdleMax;
        private float timeToPlaySecondaryIdle;
        private bool canUpdateTimeToPlaySecondaryIdle = false;

        private void Awake()
        {
            PlayIdle();
        }

        private void OnEnable()
        {
            nodePath.OnSleepingTimeToTraverseChanged += NodePath_OnSleepingTimeToTraverseChanged;
            UpdateCostText(nodePath.SleepingTimeToTraverse);
        }

        private void OnDisable()
        {
            nodePath.OnSleepingTimeToTraverseChanged -= NodePath_OnSleepingTimeToTraverseChanged;
        }

        private void NodePath_OnSleepingTimeToTraverseChanged(int value)
        {
            UpdateCostText(value);
        }

        private void Update()
        {
            if (canUpdateTimeToPlaySecondaryIdle == true)
            {
                UpdateTimeToPlaySecondaryIdle();
            }
        }

        private void UpdateCostText(int value)
        {
            costText.text = value.ToString();
        }

        private void PlayIdle()
        {
            TrackEntry animationTrack = signModelSkeletonAnimation.AnimationState.SetAnimation(0, idleAnimation, true);
            animationTrack.Complete += (_) => CheckIfCanPlaySecondaryIdle();

            SetDelayToSecondaryIdle();
        }

        private void PlaySecondaryIdle()
        {
            AnimationReferenceAsset secondaryIdleAnimation = secondaryIdleAnimations[UnityEngine.Random.Range(0, secondaryIdleAnimations.Length)];

            canUpdateTimeToPlaySecondaryIdle = false;

            TrackEntry animationTrack = signModelSkeletonAnimation.AnimationState.SetAnimation(0, secondaryIdleAnimation, false);
            animationTrack.Complete += (_) => PlayIdle();
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
    }
}