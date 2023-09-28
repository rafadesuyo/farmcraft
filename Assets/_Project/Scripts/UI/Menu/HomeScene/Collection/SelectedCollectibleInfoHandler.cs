using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class SelectedCollectibleInfoHandler : MonoBehaviour
{
    //Components
    [Header("Default Info")]
    [SerializeField] private TextMeshProUGUI nameText;

    [Space(10)]

    [SerializeField] private CollectibleCategoryIconHandler collectibleCategoryHandler;
    [SerializeField] private CollectibleLevelProgressHandler collectibleLevelHandler;

    [Space(10)]

    [SerializeField] private CollectibleShardsProgressHandler collectibleShardsProgressHandler;

    [Space(10)]

    [SerializeField] private GameObject abilityInfoItem_MiniPrefab;
    [SerializeField] private RectTransform abilityInfoItem_MinisContainer;

    [Space(10)]

    [SerializeField] private FindOrUpgradeButtonHandler[] findOrUpgradeButtonHandlers;

    [Space(10)]

    [SerializeField] private CollectibleLoopAnimationHandler collectibleLoopAnimationHandler;

    [Space(10)]

    [SerializeField] private RectTransform infoPanel;
    [SerializeField] private RectTransform infoPanelSize_Normal;
    [SerializeField] private RectTransform infoPanelSize_SubMenu_FindShards;

    [Space(10)]

    [SerializeField] private RectTransform infoButton;
    [SerializeField] private RectTransform abilityInfo_Minis;
    [SerializeField] private RectTransform findOrUpgradeButton;
    [SerializeField] private RectTransform biography;

    [Header("Profile")]
    [SerializeField] private TextMeshProUGUI biographyText;

    [Space(10)]

    [SerializeField] private GameObject abilityInfoItemPrefab;
    [SerializeField] private RectTransform abilityInfoItemsContainer;

    [Space(10)]

    [SerializeField] private RectTransform collectibleProfileLayout;

    //Events
    public event AbilityInfoItem.AbilityEvent OnAbilityInfoButtonPressed;

    public void Initialize(CollectionView collectionView)
    {
        GenericPool.CreatePool<AbilityInfoItem_Mini>(abilityInfoItem_MiniPrefab, abilityInfoItem_MinisContainer);
        GenericPool.CreatePool<AbilityInfoItem>(abilityInfoItemPrefab, abilityInfoItemsContainer);

        collectionView.OnSubMenuChanged += UpdateVisibleInfo;
    }

    public void UpdateCollectibleInfos(CollectibleType collectibleType)
    {
        Collectible collectible = CollectibleManager.Instance.GetCollectibleByType(collectibleType);

        nameText.text = collectible.Data.Name;

        collectibleCategoryHandler.SetupCategory(collectible.Data.Category);
        collectibleLevelHandler.SetupLevel(collectible.CurrentLevel);

        collectibleShardsProgressHandler.SetupShardsProgress(collectible);

        foreach(FindOrUpgradeButtonHandler findOrUpgradeButtonHandler in findOrUpgradeButtonHandlers)
        {
            findOrUpgradeButtonHandler.Setup(collectible);
        }

        collectibleLoopAnimationHandler.UpdateCollectible(collectible.Data);

        SetupAbilityInfo_Minis(collectible.CollectibleAbilities);
    }

    public void SetupProfile(CollectibleType collectibleType)
    {
        Collectible collectible = CollectibleManager.Instance.GetCollectibleByType(collectibleType);

        biographyText.text = collectible.Data.Description;

        collectibleProfileLayout.anchoredPosition = Vector2.zero;

        SetupAbilityInfos(collectible.CollectibleAbilities);

        //TODO: setup the Bonus info. Link: https://ocarinastudios.atlassian.net/browse/DQG-1796?atlOrigin=eyJpIjoiMWFiMjI5OTAwZjlkNDcwMWI1MTBiNDdkNTkyMTIwZWYiLCJwIjoiaiJ9

        AudioManager.Instance.Play("Button");
    }

    private void UpdateVisibleInfo(CollectionView.SubMenu subMenu)
    {
        if(subMenu == CollectionView.SubMenu.FindShards)
        {
            infoPanel.sizeDelta = infoPanelSize_SubMenu_FindShards.sizeDelta;
            infoPanel.anchoredPosition = infoPanelSize_SubMenu_FindShards.anchoredPosition;
            infoPanel.anchorMin = infoPanelSize_SubMenu_FindShards.anchorMin;
        }
        else
        {
            infoPanel.sizeDelta = infoPanelSize_Normal.sizeDelta;
            infoPanel.anchoredPosition = infoPanelSize_Normal.anchoredPosition;
            infoPanel.anchorMin = infoPanelSize_Normal.anchorMin;
        }

        infoButton.gameObject.SetActive(subMenu == CollectionView.SubMenu.CollectibleList);
        abilityInfo_Minis.gameObject.SetActive(subMenu == CollectionView.SubMenu.CollectibleList);
        findOrUpgradeButton.gameObject.SetActive(subMenu == CollectionView.SubMenu.CollectibleList);
        biography.gameObject.SetActive(subMenu == CollectionView.SubMenu.CollectibleProfile);
    }

    private void SetupAbilityInfo_Minis(List<CollectibleAbility> abilities)
    {
        ReleaseAbilityInfo_Minis();

        foreach(CollectibleAbility ability in abilities)
        {
            AbilityInfoItem_Mini abilityInfoItem_Mini = GenericPool.GetItem<AbilityInfoItem_Mini>();
            abilityInfoItem_Mini.transform.SetParent(abilityInfoItem_MinisContainer); //Set parent to this container because the pool is shared by another menu
            abilityInfoItem_Mini.Setup(ability);
        }
    }

    private void ReleaseAbilityInfo_Minis()
    {
        AbilityInfoItem_Mini[] abilityInfoItem_Minis = abilityInfoItem_MinisContainer.GetComponentsInChildren<AbilityInfoItem_Mini>();

        foreach(AbilityInfoItem_Mini abilityInfoItem_Mini in abilityInfoItem_Minis)
        {
            abilityInfoItem_Mini.Release();
        }
    }

    private void SetupAbilityInfos(List<CollectibleAbility> abilities)
    {
        ReleaseAbilityInfos();

        foreach (CollectibleAbility ability in abilities)
        {
            AbilityInfoItem abilityInfoItem = GenericPool.GetItem<AbilityInfoItem>();
            abilityInfoItem.Setup(ability);
            abilityInfoItem.OnInfoButtonPressed = AbilityInfoButtonPressed;
        }
    }

    private void ReleaseAbilityInfos()
    {
        AbilityInfoItem[] abilityInfoItems = abilityInfoItemsContainer.GetComponentsInChildren<AbilityInfoItem>();

        foreach (AbilityInfoItem abilityInfoItem in abilityInfoItems)
        {
            abilityInfoItem.Release();
        }
    }

    private void AbilityInfoButtonPressed(CollectibleAbility ability)
    {
        OnAbilityInfoButtonPressed?.Invoke(ability);
    }
}
