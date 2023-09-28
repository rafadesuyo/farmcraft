using Spine.Unity;
using UnityEngine;

public class DoorNodeBaseModel : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private SkeletonAnimation doorModelSkeletonAnimation;

    //Variables
    [Header("Variables")]
    [SerializeField] private AnimationReferenceAsset closedAnimation;
    [SerializeField] private AnimationReferenceAsset openingAnimation;

    public void PlayClosedAnimation()
    {
        doorModelSkeletonAnimation.AnimationState.SetAnimation(0, closedAnimation, true);
    }

    public void PlayOpeningAnimation()
    {
        doorModelSkeletonAnimation.AnimationState.SetAnimation(0, openingAnimation, false);
    }
}
