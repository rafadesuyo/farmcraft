using DreamQuiz;
using Spine;
using Spine.Unity;
using System;
using UnityEngine;

public class NodeBaseModel : MonoBehaviour
{
    //Constants
    public const string goldenSkinName = "Golden";

    //Components
    [Header("Components")]
    [SerializeField] private SkeletonAnimation nodeBaseSkeletonAnimation;
    [SerializeField, HideInInspector] NodeBaseModelType currentNodeBaseModelType;

    //Enums
    public enum NodeAnimation { Idle, Spawn, Disappearing, Win }

    //Variables
    [Header("Variables")]
    [SerializeField] protected NodeBaseModelSkinDatabaseSO nodeBaseModelSkinDatabaseSO;

    [Space(10)]

    [SerializeField] private AnimationReferenceAsset idleAnimation;
    [SerializeField] private AnimationReferenceAsset spawnAnimation;
    [SerializeField] private AnimationReferenceAsset disappearingAnimation;
    [SerializeField] private AnimationReferenceAsset winAnimation;

    [Space(10)]

    [SerializeField] private AnimationReferenceAsset endNodeIdleWithFlowersAnimation;
    [SerializeField] private AnimationReferenceAsset endNodeIdleWithoutFlowersAnimation;
    [SerializeField] private AnimationReferenceAsset endNodeSpawnAnimation;

    public void UpdateNodeModel(NodeBaseModelType nodeBaseModelType)
    {
        currentNodeBaseModelType = nodeBaseModelType;
        UpdateSkin();
    }

    public void UpdateNodeModelInEditor(NodeBaseModelType nodeBaseModelType)
    {
        currentNodeBaseModelType = nodeBaseModelType;
        UpdateInitialSkin();
    }

    private void UpdateSkin()
    {
        UpdateSkin(nodeBaseModelSkinDatabaseSO.GetSkinByType(currentNodeBaseModelType));
    }

    public void UpdateSkin(string skinName)
    {
        nodeBaseSkeletonAnimation.Skeleton.SetSkin(skinName);
    }

    private void UpdateInitialSkin()
    {
        nodeBaseSkeletonAnimation.initialSkinName = nodeBaseModelSkinDatabaseSO.GetSkinByType(currentNodeBaseModelType);
    }

    public void PlayAnimation(NodeAnimation nodeAnimation, Action onAnimationEnd = null)
    {
        AnimationReferenceAsset animation;
        bool loop = nodeAnimation == NodeAnimation.Idle;

        if(currentNodeBaseModelType == NodeBaseModelType.EndOfStage)
        {
            switch(nodeAnimation)
            {
                case NodeAnimation.Idle:
                    animation = endNodeIdleWithoutFlowersAnimation;
                    break;

                case NodeAnimation.Spawn:
                    animation = endNodeSpawnAnimation;
                    break;

                case NodeAnimation.Win:
                    animation = endNodeIdleWithFlowersAnimation;
                    loop = true;
                    break;

                default:
                    throw new Exception($"The node animation \"{nodeAnimation}\" is invalid for the node type \"{NodeBaseModelType.EndOfStage}\"!");
            }
        }
        else
        {
            switch (nodeAnimation)
            {
                case NodeAnimation.Idle:
                    animation = idleAnimation;
                    break;

                case NodeAnimation.Spawn:
                    animation = spawnAnimation;
                    break;

                case NodeAnimation.Disappearing:
                    animation = disappearingAnimation;
                    break;

                case NodeAnimation.Win:
                    animation = winAnimation;
                    break;

                default:
                    throw new Exception($"The node animation \"{nodeAnimation}\" is invalid!");
            }
        }

        TrackEntry animationTrack = nodeBaseSkeletonAnimation.AnimationState.SetAnimation(0, animation, loop);
        animationTrack.Complete += (_) => onAnimationEnd?.Invoke();
    }
}
