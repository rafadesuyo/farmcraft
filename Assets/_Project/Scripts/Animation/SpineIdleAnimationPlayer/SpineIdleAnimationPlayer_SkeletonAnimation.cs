using Spine;
using Spine.Unity;
using System;
using UnityEngine;

public class SpineIdleAnimationPlayer_SkeletonAnimation : SpineIdleAnimationPlayer_Base
{
    //Components
    [Header("Components")]
    [SerializeField] private SkeletonAnimation skeletonAnimation;

    protected override void SetAnimation(AnimationReferenceAsset animation, bool loop, Action onAnimationEnd = null)
    {
        TrackEntry animationTrack = skeletonAnimation.AnimationState.SetAnimation(0, animation, loop);
        animationTrack.Complete += (_) => onAnimationEnd?.Invoke();
    }
}
