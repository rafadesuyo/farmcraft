using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbilityData", menuName = "Ability/Ability Data")]
public class AbilityDataSO : ScriptableObject
{
    //Constants
    public const string textToReplaceWithModifierText = "{modifier}";

    [Header("Info")]
    [SerializeField] private AbilityId abilityId;
    [SerializeField] private new string name;
    [SerializeField][TextArea] private string description;
    [SerializeField] private string miniDescription;

    [Space(10)]

    [SerializeField] private Sprite icon;
    [SerializeField] private SkeletonDataAsset iconSkeletonData;
    [SerializeField] private AnimationReferenceAsset iconAnimation;

    [Space(10)]

    [SerializeField] private int unlockLevel = 1;

    [Header("Gameplay")]
    [SerializeField] private List<AbilityEnhancementSO> abilityEnhancements;
    [SerializeField] private int maxLevel = 5;
    [SerializeField] private bool isPassiveUse = false;

    public AbilityId AbilityId
    {
        get
        {
            return abilityId;
        }
    }

    public string Name
    {
        get
        {
            return name;
        }
    }

    public Sprite Icon
    {
        get
        {
            return icon;
        }
    }

    public SkeletonDataAsset IconSkeletonData => iconSkeletonData;

    public AnimationReferenceAsset IconAnimation => iconAnimation;

    public int UnlockLevel
    {
        get
        {
            return unlockLevel;
        }
    }

    public IReadOnlyList<AbilityEnhancementSO> AbilityEnhancements
    {
        get
        {
            return abilityEnhancements.AsReadOnly();
        }
    }

    public int MaxLevel
    {
        get
        {
            return maxLevel;
        }
    }

    public bool IsPassiveUse
    {
        get
        {
            return isPassiveUse;
        }
    }

    public int GetUsePerStage(int level)
    {
        if (level >= MaxLevel)
        {
            level = MaxLevel - 1;
        }

        var enhancement = abilityEnhancements[level] as ActiveAbilityEnhancementSO;

        if (enhancement != null)
        {
            return enhancement.UsePerStage;
        }

        return 0;
    }

    public string GetDescription(int level, Color modifierTextColor)
    {
        return description.Replace(textToReplaceWithModifierText, GetModifierTextWithColor(level, modifierTextColor));
    }

    public string GetMiniDescription(int level)
    {
        return miniDescription.Replace(textToReplaceWithModifierText, GetModifierText(level));
    }

    public string GetModifierTextWithColor(int level, Color color)
    {
        return TextFormatter.InlineColor(GetModifierText(level), color);
    }

    public string GetModifierText(int level)
    {
        level = Mathf.Clamp(level, 0, maxLevel - 1);

        return abilityEnhancements[level].GetModifierText();
    }
}
