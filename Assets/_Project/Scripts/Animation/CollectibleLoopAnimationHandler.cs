using Spine;
using Spine.Unity;
using System;
using UnityEngine;

public class CollectibleLoopAnimationHandler : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private SkeletonGraphic collectibleSkeletonGraphic;

    private RectTransform rectTransform;

    //Enums
    public enum PivotType { CollectionView, Centered }

    //Variables
    [Header("Variables")]
    [SerializeField] private PivotType pivotType;

    [Space(10)]

    [SerializeField] private bool playSecondaryIdle = true;
    [SerializeField] private float delayBetweenSecondaryIdleMin = 4;
    [SerializeField] private float delayBetweenSecondaryIdleMax = 8;

    private CollectibleSO currentCollectible;

    private float timeToPlaySecondaryIdleMax;
    private float timeToPlaySecondaryIdle;
    private bool canUpdateTimeToPlaySecondaryIdle = false;

    public void SetAnimationSpeed(float speed)
    {
        collectibleSkeletonGraphic.timeScale = speed;
    }

    public void UpdateCollectible(CollectibleSO collectible)
    {
        if(collectible.SpineSkeletonData == null)
        {
            Debug.LogError("The Spine Skeleton Data for this Collectible is null!");
            return;
        }

        currentCollectible = collectible;

        Vector2 pivot;

        switch(pivotType)
        {
            case PivotType.CollectionView:
                pivot = collectible.ImagePivot_CollectionView;
                break;

            case PivotType.Centered:
                pivot = collectible.ImagePivot_Centered;
                break;

            default:
                throw new Exception($"The Pivot Type \"{pivotType}\" is invalid!");
        }

        UpdateSkeletonGraphic(collectible.SpineSkeletonData, pivot, collectible.ImageScale);
    }

    private void Awake()
    {
        rectTransform = collectibleSkeletonGraphic.gameObject.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if(canUpdateTimeToPlaySecondaryIdle == true)
        {
            UpdateTimeToPlaySecondaryIdle();
        }
    }

    private void UpdateSkeletonGraphic(SkeletonDataAsset skeletonData, Vector2 pivot, float scale)
    {
        collectibleSkeletonGraphic.skeletonDataAsset = skeletonData;
        collectibleSkeletonGraphic.Initialize(true);

        rectTransform.pivot = pivot;
        rectTransform.localScale = new Vector2(scale, scale);

        PlayIdle();
    }

    public void PlayIdle()
    {
        TrackEntry animationTrack = collectibleSkeletonGraphic.AnimationState.SetAnimation(0, currentCollectible.IdleAnimation, true);
        animationTrack.Complete += (_) => CheckIfCanPlaySecondaryIdle();

        SetDelayToSecondaryIdle();
    }

    private void PlaySecondaryIdle()
    {
        AnimationReferenceAsset secondaryIdleAnimation = currentCollectible.SecondaryIdleAnimations[UnityEngine.Random.Range(0, currentCollectible.SecondaryIdleAnimations.Length)];

        canUpdateTimeToPlaySecondaryIdle = false;

        TrackEntry animationTrack = collectibleSkeletonGraphic.AnimationState.SetAnimation(0, secondaryIdleAnimation, false);
        animationTrack.Complete += (_) => PlayIdle();
    }

    public void PlayUpgradeAnimation(Action onAnimationEnd = null)
    {
        if (currentCollectible == null)
        {
            Debug.LogError("The current Collectible is null!");
            return;
        }

        AnimationReferenceAsset upgradeAnimation = currentCollectible.UpgradeAnimation;

        if(upgradeAnimation == null)
        {
            Debug.LogError($"The upgrade animation of the collectible \"{currentCollectible}\" is null!");
            return;
        }

        canUpdateTimeToPlaySecondaryIdle = false;

        TrackEntry animationTrack = collectibleSkeletonGraphic.AnimationState.SetAnimation(0, upgradeAnimation, false);

        animationTrack.Complete += (_) =>
        {
            PlayIdle();
            onAnimationEnd?.Invoke();
        };
    }

    public void PlayAbilityAnimation(Action onAnimationEnd = null)
    {
        if (currentCollectible == null)
        {
            Debug.LogError("The current Collectible is null!");
            return;
        }

        AnimationReferenceAsset abilityAnimation = currentCollectible.AbilityAnimation;

        if (abilityAnimation == null)
        {
            Debug.LogError($"The ability animation of the collectible \"{currentCollectible}\" is null!");
            return;
        }

        canUpdateTimeToPlaySecondaryIdle = false;

        TrackEntry animationTrack = collectibleSkeletonGraphic.AnimationState.SetAnimation(0, abilityAnimation, false);
        animationTrack.MixDuration = 0;
        animationTrack.Complete += (_) => onAnimationEnd?.Invoke();
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
        if(playSecondaryIdle == false)
        {
            return;
        }

        if (timeToPlaySecondaryIdle >= timeToPlaySecondaryIdleMax)
        {
            PlaySecondaryIdle();
        }
    }
}
