using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Collectible", menuName = "Collectibles/Collectible")]
public class CollectibleSO : ScriptableObject
{
    [Header("Infos")]
    [SerializeField] private CollectibleType type;

    [Space(5)]

    [SerializeField] private Sprite icon;
    [SerializeField] private Sprite portraitColored;
    [SerializeField] private Sprite portraitBlackWhite;
    [SerializeField] private Sprite fullPortrait;

    [Space(5)]

    [SerializeField] private new string name;
    [SerializeField] [TextArea] private string description;

    [Space(5)]

    [SerializeField] private QuizCategory category;
    [SerializeField] private int[] shardsRequiredPerLevel = null;
    [SerializeField] private int maxLevel = 5;

    [Space(5)]
    [SerializeField] private List<AbilityDataSO> abilityDataList;

    [Header("Spine Data")]
    [SerializeField] private SkeletonDataAsset spineSkeletonData;

    [Space(10)]

    [SerializeField] private AnimationReferenceAsset idleAnimation;
    [SerializeField] private AnimationReferenceAsset[] secondaryIdleAnimations;

    [Space(10)]

    [SerializeField] private AnimationReferenceAsset upgradeAnimation;

    [Space(10)]

    [SerializeField] private AnimationReferenceAsset abilityAnimation;
    [SerializeField] private AnimationReferenceAsset abilityBackgroundEffectAnimation;

    [Header("Collection View Options")]
    [SerializeField] private Vector2 imagePivot_CollectionView = new Vector2(0.5f, 0);
    [SerializeField] private Vector2 imagePivot_Centered = new Vector2(0.5f, 0);
    [SerializeField] private float imageScale = 1;

    public CollectibleType Type => type;
    public Sprite Icon => icon;
    public Sprite PortraitColored => portraitColored;
    public Sprite PortraitBlackWhite => portraitBlackWhite;
    public Sprite FullPortrait => fullPortrait;
    public string Name => name;
    public string Description => description;
    public QuizCategory Category => category;
    public int MaxLevel => maxLevel;
    public int[] ShardsRequiredPerLevel => shardsRequiredPerLevel;
    public List<AbilityDataSO> AbilityDataList => abilityDataList;
    public SkeletonDataAsset SpineSkeletonData => spineSkeletonData;
    public AnimationReferenceAsset IdleAnimation => idleAnimation;
    public AnimationReferenceAsset[] SecondaryIdleAnimations => secondaryIdleAnimations;
    public AnimationReferenceAsset UpgradeAnimation => upgradeAnimation;
    public AnimationReferenceAsset AbilityAnimation => abilityAnimation;
    public AnimationReferenceAsset AbilityBackgroundEffectAnimation => abilityBackgroundEffectAnimation;
    public Vector2 ImagePivot_CollectionView => imagePivot_CollectionView;
    public Vector2 ImagePivot_Centered => imagePivot_Centered;
    public float ImageScale => imageScale;
}