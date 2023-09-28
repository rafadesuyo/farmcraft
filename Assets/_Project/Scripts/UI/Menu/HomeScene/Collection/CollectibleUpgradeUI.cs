using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectibleUpgradeUI : UIControllerAnimated
{
    //Components
    [Header("Components")]
    [SerializeField] private CollectibleLoopAnimationHandler collectibleLoopAnimationHandler;
    [SerializeField] private SkeletonGraphic upgradeEffect;
    [SerializeField] private SkeletonGraphic upgradeEffect_Background;

    [Space(10)]

    [SerializeField] private AnimationReferenceAsset upgradeEffectAnimation;
    [SerializeField] private AnimationReferenceAsset upgradeEffectBackgroundAnimation;
    [SerializeField] private AnimationReferenceAsset upgradeEffectOffAnimation;

    [Space(10)]

    [SerializeField] private TextMeshProUGUI[] collectibleNameTexts;
    [SerializeField] private CollectibleCategoryIconHandler[] collectibleCategoryIconHandlers;
    [SerializeField] private CollectibleShardsProgressHandler[] collectibleShardsProgressHandlers;
    [SerializeField] private FindOrUpgradeButtonHandler[] findOrUpgradeButtonHandlers;

    [Header("Upgrade Collectible Sub Menu")]
    [SerializeField] private CollectibleLevelProgressHandler collectibleLevelProgressHandler_UpgradeCollectible;
    [SerializeField] private RectTransform abilityInfoItem_MinisContainer;

    [Header("Collectible Upgraded Sub Menu")]
    [SerializeField] private CollectibleLevelProgressHandler collectibleLevelProgressHandler_CollectibleUpgraded;
    [SerializeField] private TextMeshProUGUI newLevelText;

    [Header("Sub Menus")]
    [SerializeField] private RectTransform upgradeCollectible;
    [SerializeField] private RectTransform collectibleUpgraded;

    //Enums
    public enum SubMenu { UpgradeCollectible, CollectibleUpgraded }

    //Events
    public event Action OnCollectibleIsUpgraded;
    public event Action OnFindShardsButtonPressed;

    //Variables
    [Header("Default Variables")]
    [SerializeField] private string textToReplaceWithValue = "{value}";
    [SerializeField] private string textNewLevel = "Star Level {value}";

    private Collectible currentCollectible;

    public void UpgradeCollectible()
    {
        int goldToUpgrade = 200; //TODO: get the gold value to upgrade a collectible. Link: https://ocarinastudios.atlassian.net/browse/DQG-1795?atlOrigin=eyJpIjoiYTUzMzU1YTk2NWMxNDg4ZmE2MWQzNTlkNDVlYTZhNmMiLCJwIjoiaiJ9

        if (PlayerProgress.TotalGold >= goldToUpgrade)
        {
            PlayerProgress.TotalGold -= goldToUpgrade;

            currentCollectible.LevelUp();
            OnCollectibleIsUpgraded?.Invoke();

            PlayUpgradeAnimation();

            UpdateVariables_General();
            SetSubMenu(SubMenu.CollectibleUpgraded);

            GameManager.Instance.SaveGame();
        }
        else
        {
            //TODO: add a feedback to represent this. Link: https://ocarinastudios.atlassian.net/browse/DQG-1807?atlOrigin=eyJpIjoiNmZhY2RlNzBhZTdlNGRmZDhjMmNkNDJmMzAwYTVjYWYiLCJwIjoiaiJ9
            Debug.Log("You don't have enough gold to upgrade this Collectible!");
        }
    }

    public void FindShardsButtonPressed()
    {
        OnFindShardsButtonPressed?.Invoke();
    }

    public void SetSubMenuToUpgradeCollectible()
    {
        SetSubMenu(SubMenu.UpgradeCollectible);
    }

    public void Setup(CollectibleType collectibleType)
    {
        OpenUI();

        currentCollectible = CollectibleManager.Instance.GetCollectibleByType(collectibleType);

        UpdateCollectibleAnimation();
        UpdateVariables_General();

        SetSubMenu(SubMenu.UpgradeCollectible);
    }

    protected override void OnClose()
    {
        ResetVariables();
    }

    private void ResetVariables()
    {
        currentCollectible = null;

        foreach (TextMeshProUGUI textMeshPro in collectibleNameTexts)
        {
            textMeshPro.text = string.Empty;
        }

        collectibleLevelProgressHandler_UpgradeCollectible.StopUpgradeAnimation();

        ReleaseAbilityInfo_Minis();
    }

    private void SetSubMenu(SubMenu subMenu)
    {
        upgradeCollectible.gameObject.SetActive(subMenu == SubMenu.UpgradeCollectible);
        collectibleUpgraded.gameObject.SetActive(subMenu == SubMenu.CollectibleUpgraded);

        switch(subMenu)
        {
            case SubMenu.UpgradeCollectible:
                UpdateVariables_UpgradeCollectible();
                break;

            case SubMenu.CollectibleUpgraded:
                UpdateVariables_CollectibleUpgraded();
                break;
        }
    }

    private void UpdateCollectibleAnimation()
    {
        collectibleLoopAnimationHandler.UpdateCollectible(currentCollectible.Data);

        StopUpgradeEffect(upgradeEffect);
        StopUpgradeEffect(upgradeEffect_Background);
    }

    private void PlayUpgradeAnimation()
    {
        collectibleLoopAnimationHandler.PlayUpgradeAnimation();

        PlayUpgradeEffect(upgradeEffect, upgradeEffectAnimation);
        PlayUpgradeEffect(upgradeEffect_Background, upgradeEffectBackgroundAnimation);
    }

    private void PlayUpgradeEffect(SkeletonGraphic skeletonGraphic, AnimationReferenceAsset animation)
    {
        TrackEntry animationTrack = skeletonGraphic.AnimationState.SetAnimation(0, animation, false);
        animationTrack.Complete += (_) => StopUpgradeEffect(skeletonGraphic);

    }

    private void StopUpgradeEffect(SkeletonGraphic skeletonGraphic)
    {
        skeletonGraphic.AnimationState.SetAnimation(0, upgradeEffectOffAnimation, false);
    }

    private void UpdateVariables_General()
    {
        foreach (TextMeshProUGUI textMeshPro in collectibleNameTexts)
        {
            textMeshPro.text = currentCollectible.Data.Name;
        }

        foreach (CollectibleCategoryIconHandler collectibleCategoryIconHandler in collectibleCategoryIconHandlers)
        {
            collectibleCategoryIconHandler.SetupCategory(currentCollectible.Data.Category);
        }

        foreach (CollectibleShardsProgressHandler collectibleShardsProgressHandler in collectibleShardsProgressHandlers)
        {
            collectibleShardsProgressHandler.SetupShardsProgress(currentCollectible);
        }

        foreach (FindOrUpgradeButtonHandler findOrUpgradeButtonHandler in findOrUpgradeButtonHandlers)
        {
            findOrUpgradeButtonHandler.Setup(currentCollectible);
        }
    }

    private void UpdateVariables_UpgradeCollectible()
    {
        int levelToShowProgress = Mathf.Clamp(currentCollectible.CurrentLevel + 1, 0, currentCollectible.MaxLevel);

        collectibleLevelProgressHandler_UpgradeCollectible.SetupLevel(levelToShowProgress);
        collectibleLevelProgressHandler_UpgradeCollectible.PlayUpgradeAnimation(levelToShowProgress);

        SetupAbilityInfo_Minis(currentCollectible.CollectibleAbilities, levelToShowProgress);
    }

    private void UpdateVariables_CollectibleUpgraded()
    {
        collectibleLevelProgressHandler_CollectibleUpgraded.SetupLevel(currentCollectible.CurrentLevel);

        newLevelText.text = textNewLevel.Replace(textToReplaceWithValue, currentCollectible.CurrentLevel.ToString());
    }

    private void SetupAbilityInfo_Minis(List<CollectibleAbility> abilities, int levelToShowProgress)
    {
        ReleaseAbilityInfo_Minis();

        if(currentCollectible.CurrentLevel <= 0)
        {
            Debug.LogError("SetupAbilityInfo_Minis was called with Collectible in Level 0, this is not supported.");
            return;
        }

        foreach (CollectibleAbility ability in abilities)
        {
            if (IsAbilityDifferentFromPreviousLevel(ability, levelToShowProgress) == false)
            {
                continue;
            }

            AbilityInfoItem_Mini abilityInfoItem_Mini = GenericPool.GetItem<AbilityInfoItem_Mini>();
            abilityInfoItem_Mini.transform.SetParent(abilityInfoItem_MinisContainer); //Set parent to this container because the pool is shared by another menu
            abilityInfoItem_Mini.SetupAbilityInSpecificLevel(ability, levelToShowProgress - 1);
        }
    }

    private void ReleaseAbilityInfo_Minis()
    {
        AbilityInfoItem_Mini[] abilityInfoItem_Minis = abilityInfoItem_MinisContainer.GetComponentsInChildren<AbilityInfoItem_Mini>();

        foreach (AbilityInfoItem_Mini abilityInfoItem_Mini in abilityInfoItem_Minis)
        {
            abilityInfoItem_Mini.Release();
        }
    }

    private bool IsAbilityDifferentFromPreviousLevel(CollectibleAbility ability, int currentLevel)
    {
        if(ability.AbilityDataSO.UnlockLevel == currentLevel)
        {
            return true;
        }

        if (ability.AbilityDataSO.AbilityEnhancements[currentLevel - 2] != ability.AbilityDataSO.AbilityEnhancements[currentLevel - 1])
        {
            return true;
        }

        return false;
    }
}
