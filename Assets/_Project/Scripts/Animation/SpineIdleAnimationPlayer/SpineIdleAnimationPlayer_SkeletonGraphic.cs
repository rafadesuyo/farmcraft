using Spine;
using Spine.Unity;
using System;
using UnityEngine;

public class SpineIdleAnimationPlayer_SkeletonGraphic : SpineIdleAnimationPlayer_Base
{
    //Components
    [Header("Components")]
    [SerializeField] private SkeletonGraphic skeletonGraphic;

    protected override void SetAnimation(AnimationReferenceAsset animation, bool loop, Action onAnimationEnd = null)
    {
        TrackEntry animationTrack = skeletonGraphic.AnimationState.SetAnimation(0, animation, loop);
        animationTrack.Complete += (_) => onAnimationEnd?.Invoke();
    }
}
