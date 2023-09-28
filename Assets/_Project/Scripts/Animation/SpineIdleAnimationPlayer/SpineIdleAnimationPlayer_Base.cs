using Spine.Unity;
using System;
using UnityEngine;

public abstract class SpineIdleAnimationPlayer_Base : MonoBehaviour
{
    //Variables
    [Header("Base Variables")]
    [SerializeField] protected AnimationReferenceAsset idleAnimation;
    [SerializeField] protected AnimationReferenceAsset[] secondaryIdleAnimations;

    [Space(10)]

    [SerializeField] protected float delayBetweenSecondaryIdleMin = 4;
    [SerializeField] protected float delayBetweenSecondaryIdleMax = 8;

    protected float timeToPlaySecondaryIdleMax;
    protected float timeToPlaySecondaryIdle;
    protected bool canUpdateTimeToPlaySecondaryIdle = false;

    protected void Awake()
    {
        PlayIdle();
    }

    protected void Update()
    {
        if (canUpdateTimeToPlaySecondaryIdle == true)
        {
            UpdateTimeToPlaySecondaryIdle();
        }
    }

    protected abstract void SetAnimation(AnimationReferenceAsset animation, bool loop, Action onAnimationEnd = null);

    protected void PlayIdle()
    {
        SetAnimation(idleAnimation, true, CheckIfCanPlaySecondaryIdle);

        if(HasSecondaryIdle() == true)
        {
            SetDelayToSecondaryIdle();
        }
        else
        {
            timeToPlaySecondaryIdle = 0;
            canUpdateTimeToPlaySecondaryIdle = false;
        }
    }

    protected void PlaySecondaryIdle()
    {
        AnimationReferenceAsset secondaryIdleAnimation = secondaryIdleAnimations[UnityEngine.Random.Range(0, secondaryIdleAnimations.Length)];

        canUpdateTimeToPlaySecondaryIdle = false;

        SetAnimation(secondaryIdleAnimation, false, PlayIdle);
    }

    protected void SetDelayToSecondaryIdle()
    {
        timeToPlaySecondaryIdleMax = UnityEngine.Random.Range(delayBetweenSecondaryIdleMin, delayBetweenSecondaryIdleMax);
        timeToPlaySecondaryIdle = 0;
        canUpdateTimeToPlaySecondaryIdle = true;
    }

    protected void UpdateTimeToPlaySecondaryIdle()
    {
        timeToPlaySecondaryIdle += Time.deltaTime;
    }

    protected void CheckIfCanPlaySecondaryIdle()
    {
        if (timeToPlaySecondaryIdle >= timeToPlaySecondaryIdleMax)
        {
            PlaySecondaryIdle();
        }
    }

    protected bool HasSecondaryIdle()
    {
        return secondaryIdleAnimations != null && secondaryIdleAnimations.Length > 0;
    }
}
