using Spine.Unity;
using UnityEngine;

public class WallNodeBaseModel : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private SkeletonAnimation wallModelSkeletonAnimation;

    //Variables
    [Header("Variables")]
    [SerializeField] private AnimationReferenceAsset idleAnimation;
    [SerializeField] private AnimationReferenceAsset destroyedAnimation;

    public void PlayIdleAnimation()
    {
        wallModelSkeletonAnimation.AnimationState.SetAnimation(0, idleAnimation, true);
    }

    public void PlayDestroyedAnimation()
    {
        wallModelSkeletonAnimation.AnimationState.SetAnimation(0, destroyedAnimation, false);
    }
}
