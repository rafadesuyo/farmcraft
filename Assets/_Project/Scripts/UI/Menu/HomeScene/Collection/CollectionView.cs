using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectionView : MenuView
{
    //Components
    [Header("Components")]
    [SerializeField] private GameObject collectionItemPrefab;
    [SerializeField] private GameObject collectionItemCategorySeparatorPrefab;
    [SerializeField] private GameObject collectionItemsGridPrefab;
    [SerializeField] private GameObject shardLocationItemPrefab;

    [Space(10)]

    [SerializeField] private RectTransform collectionItemsContainer;
    [SerializeField] private RectTransform shardLocationItemsContainer;

    [Space(10)]

    [SerializeField] private SelectedCollectibleInfoHandler selectedCollectibleInfoHandler;

    [Header("Sub Menus")]
    [SerializeField] private RectTransform collectibleListContainer;
    [SerializeField] private RectTransform collectibleProfileContainer;
    [SerializeField] private RectTransform findShardsUIContainer;

    [Header("Menus")]
    [SerializeField] private AbilityInfoUI abilityInfoUI;
    [SerializeField] private CollectibleUpgradeUI collectibleUpgradeUI;

    [Space(10)]

    [SerializeField] private RectTransform homeFooter;

    [Header("Database")]
    [SerializeField] private StoreSO storeData; //TODO: check if this will be used, or if the Store Search will be made directly from the StoreView scripts. Link: https://ocarinastudios.atlassian.net/browse/DQG-1850?atlOrigin=eyJpIjoiMmE3MjNhYTFkMTg3NGVlNzgyMmViZDdhZmZjMGNkYWIiLCJwIjoiaiJ9

    //Enums
    public enum SubMenu { CollectibleList, CollectibleProfile, FindShards }
    public enum CollectibleFilter { All, AZ, Star, Category }

    //Events
    public delegate void SubMenuEvent(SubMenu subMenu);

    public event SubMenuEvent OnSubMenuChanged;

    //Variables
    [Header("Variables")]
    [SerializeField] private Color borderColorDefault;
    [SerializeField] private float borderSizeDefault = 0f;

    [Space(10)]

    [SerializeField] private Color borderColorSelected;
    [SerializeField] private float borderSizeSelected = 0f;

    private SubMenu currentSubMenu = SubMenu.CollectibleList;
    
    private CollectionItem selectedCollectible;
    private CollectibleType selectedCollectibleType = CollectibleType.None;
    private CollectibleFilter currentFilter = CollectibleFilter.All;

    //Getters
    public override Menu Type => Menu.Collection;
    public SubMenu CurrentSubMenu => currentSubMenu;
    public CollectibleType SelectedCollectibleType => selectedCollectibleType;

    public void SetSubMenu(SubMenu subMenu)
    {
        currentSubMenu = subMenu;

        UpdateSubMenus();

        OnSubMenuChanged?.Invoke(currentSubMenu);
    }

    #region Sub Menu Setters
    public void SetSubMenuToCollectibleList()
    {
        SetSubMenu(SubMenu.CollectibleList);
    }

    public void SetSubMenuToCollectibleProfile()
    {
        SetSubMenu(SubMenu.CollectibleProfile);
    }

    public void SetSubMenuToFindShards()
    {
        SetSubMenu(SubMenu.FindShards);
    }
    #endregion

    #region Filter Setters
    public void SetFilterToAll()
    {
        SetupCollectibles(CollectibleFilter.All);
    }

    public void SetFilterToAZ()
    {
        SetupCollectibles(CollectibleFilter.AZ);
    }

    public void SetFilterToStar()
    {
        SetupCollectibles(CollectibleFilter.Star);
    }

    public void SetFilterToCategory()
    {
        SetupCollectibles(CollectibleFilter.Category);
    }
    #endregion

    public override void Initialize()
    {
        GenericPool.CreatePool<CollectionItem>(collectionItemPrefab, collectionItemsContainer);
        GenericPool.CreatePool<CollectionItemsCategorySeparator>(collectionItemCategorySeparatorPrefab, collectionItemsContainer);
        GenericPool.CreatePool<CollectionItemsGrid>(collectionItemsGridPrefab, collectionItemsContainer);
        GenericPool.CreatePool<ShardLocationItem>(shardLocationItemPrefab, shardLocationItemsContainer);

        collectibleUpgradeUI.OnCollectibleIsUpgraded += OnCollectibleIsUpgraded;
        collectibleUpgradeUI.OnFindShardsButtonPressed += OnCollectibleUpgradeUIFindShardsButtonPressed;
        selectedCollectibleInfoHandler.OnAbilityInfoButtonPressed += OpenAbilityInfo;

        selectedCollectibleInfoHandler.Initialize(this);
    }

    protected override void Setup(MenuSetupOptions setupOptions)
    {
        currentFilter = CollectibleFilter.All;
        SetSubMenu(SubMenu.CollectibleList);
    }

    protected override void OnClose()
    {
        selectedCollectibleType = CollectibleType.None;
        selectedCollectible = null;
    }

    private void UpdateSubMenus()
    {
        collectibleListContainer.gameObject.SetActive(currentSubMenu == SubMenu.CollectibleList);
        collectibleProfileContainer.gameObject.SetActive(currentSubMenu == SubMenu.CollectibleProfile);
        findShardsUIContainer.gameObject.SetActive(currentSubMenu == SubMenu.FindShards);

        homeFooter.gameObject.SetActive(currentSubMenu == SubMenu.CollectibleList);

        switch (currentSubMenu)
        {
            case SubMenu.CollectibleList:
                SetupCollectibles(currentFilter, true);
                break;

            case SubMenu.CollectibleProfile:
                selectedCollectibleInfoHandler.SetupProfile(selectedCollectibleType);
                break;

            case SubMenu.FindShards:
                SetupShardLocationItems();
                shardLocationItemsContainer.anchoredPosition = Vector3.zero;
                break;
        }
    }

    #region Collectible List
    private void SetupCollectibles(CollectibleFilter collectibleFilter, bool ignoreCurrentFilter = false)
    {
        if(ignoreCurrentFilter == false)
        {
            if(collectibleFilter == currentFilter)
            {
                return;
            }
        }

        currentFilter = collectibleFilter;

        FillCollectibleItems();

        if (selectedCollectibleType != CollectibleType.None)
        {
            SelectCollectibleByType(selectedCollectibleType);
        }
        else
        {
            SelectFirstCollectiblePossible();
        }
    }

    private void FillCollectibleItems()
    {
        List<CollectibleSO> collectibles = CollectibleManager.Instance.CollectiblesData;

        collectibles = OrderCollectiblesBasedOnFilter(collectibles, currentFilter);

        collectionItemsContainer.gameObject.SetActive(false); //Release all objets to Object Pool
        collectionItemsContainer.gameObject.SetActive(true);

        collectionItemsContainer.anchoredPosition = Vector2.zero;

        if (currentFilter == CollectibleFilter.Category)
        {
            FillCollectibleItemsByCategory(collectibles);
        }
        else
        {
            FillCollectibleItems(collectibles);
        }
    }

    private void FillCollectibleItems(List<CollectibleSO> collectibles)
    {
        CollectionItemsGrid collectionItemsGrid = GenericPool.GetItem<CollectionItemsGrid>();

        foreach(CollectibleSO collectible in collectibles)
        {
            CollectionItem collectionItem = GetAndSetupCollectionItem(collectible);

            collectionItemsGrid.AddCollectionItemToGrid(collectionItem);
        }
    }

    private void FillCollectibleItemsByCategory(List<CollectibleSO> collectibles)
    {
        System.Array quizCategories = System.Enum.GetValues(typeof(QuizCategory));

        foreach(QuizCategory quizCategory in quizCategories)
        {
            if(quizCategory == QuizCategory.None || quizCategory == QuizCategory.Random)
            {
                continue;
            }

            CollectionItemsCategorySeparator collectionItemsCategorySeparator = GenericPool.GetItem<CollectionItemsCategorySeparator>();
            collectionItemsCategorySeparator.Setup(quizCategory);

            CollectionItemsGrid collectionItemsGrid = GenericPool.GetItem<CollectionItemsGrid>();

            foreach (CollectibleSO collectible in collectibles)
            {
                if(collectible.Category != quizCategory)
                {
                    continue;
                }

                CollectionItem collectionItem = GetAndSetupCollectionItem(collectible);

                collectionItemsGrid.AddCollectionItemToGrid(collectionItem);
            }
        }
    }

    private CollectionItem GetAndSetupCollectionItem(CollectibleSO collectible)
    {
        CollectionItem collectionItem = GenericPool.GetItem<CollectionItem>();
        collectionItem.Setup(collectible, OnSelectCollectible);
        collectionItem.UpdateBorder(borderSizeDefault, borderColorDefault);

        return collectionItem;
    }

    private List<CollectibleSO> OrderCollectiblesBasedOnFilter(List<CollectibleSO> collectibles, CollectibleFilter filter)
    {
        collectibles = collectibles.OrderBy(collectible => collectible.name).ToList();

        switch (filter)
        {
            case CollectibleFilter.All:
                collectibles = collectibles.OrderByDescending(collectible => CollectibleManager.Instance.IsCollectibleUnlocked(collectible.Type)).ToList();
                break;

            case CollectibleFilter.Star:
                collectibles = collectibles.OrderByDescending(collectible => CollectibleManager.Instance.GetCollectibleLevelByType(collectible.Type)).ToList();
                break;
        }

        return collectibles;
    }

    private void SelectFirstCollectiblePossible()
    {
        CollectionItem[] collectionItems = collectionItemsContainer.GetComponentsInChildren<CollectionItem>();

        foreach (CollectionItem collectionItem in collectionItems)
        {
            if (CollectibleManager.Instance.IsCollectibleUnlocked(collectionItem.CollectibleType))
            {
                OnSelectCollectible(collectionItem);
                return;
            }
        }
    }

    private void SelectCollectibleByType(CollectibleType collectibleType)
    {
        CollectionItem[] collectionItems = collectionItemsContainer.GetComponentsInChildren<CollectionItem>();

        foreach (CollectionItem collectionItem in collectionItems)
        {
            if (collectionItem.CollectibleType == collectibleType)
            {
                OnSelectCollectible(collectionItem);
                return;
            }
        }
    }

    private void OnSelectCollectible(CollectionItem collectionItem)
    {
        if (selectedCollectible != null)
        {
            selectedCollectible.UpdateBorder(borderSizeDefault, borderColorDefault);
        }

        collectionItem.UpdateBorder(borderSizeSelected, borderColorSelected);

        selectedCollectible = collectionItem;

        if(selectedCollectibleType != collectionItem.CollectibleType)
        {
            selectedCollectibleType = collectionItem.CollectibleType;

            selectedCollectibleInfoHandler.UpdateCollectibleInfos(collectionItem.CollectibleType);

            AudioManager.Instance.Play("SelectCollectible");
        }
    }
    #endregion

    #region Find Shards UI
    private void SetupShardLocationItems()
    {
        ReleaseShardLocationItems();

        SetupShardLocationItemsFromStore();
        SetupShardLocationItemsFromWorldMap();
    }

    private void ReleaseShardLocationItems()
    {
        ShardLocationItem[] shardLocationItems = shardLocationItemsContainer.GetComponentsInChildren<ShardLocationItem>();

        foreach (ShardLocationItem shardLocationItem in shardLocationItems)
        {
            shardLocationItem.Release();
        }
    }

    private void SetupShardLocationItemsFromStore()
    {
        //TODO: update the store search to work with the new store itens and behaviour, once it's updated. Link: https://ocarinastudios.atlassian.net/browse/DQG-1850?atlOrigin=eyJpIjoiMmE3MjNhYTFkMTg3NGVlNzgyMmViZDdhZmZjMGNkYWIiLCJwIjoiaiJ9
        foreach (StoreItemSO storeItem in storeData.itemsToSell)
        {
            if (storeItem.Section == ItemType.Shards && storeItem.ShardType == selectedCollectibleType)
            {
                ShardLocationItem shardLocationItem = GenericPool.GetItem<ShardLocationItem>();
                shardLocationItem.Setup(storeItem, OpenShardsStore);
            }
        }
    }

    private void SetupShardLocationItemsFromWorldMap()
    {
        foreach (World world in WorldMap.Instance.Worlds)
        {
            foreach (StageButton stageButton in world.StageButtons)
            {
                if (stageButton.State == StageButton.StageState.Inactive)
                {
                    continue;
                }

                foreach (StageGoal stageGoal in stageButton.StageInfo.Goals)
                {
                    if (stageGoal.GoalReward.CurrencyType == CurrencyType.Shard && stageGoal.GoalReward.CollectibleType == selectedCollectibleType)
                    {
                        ShardLocationItem shardLocationItem = GenericPool.GetItem<ShardLocationItem>();
                        shardLocationItem.Setup(stageButton.StageInfo, stageGoal.GoalReward, () => OpenWorldMapAndGoToStage(stageButton));
                    }
                }
            }
        }
    }

    private void OpenShardsStore()
    {
        CanvasManager.Instance.OpenMenu(Menu.Store, new MenuSetupOptions(ItemType.Shards), true);

        AudioManager.Instance.Play("Button");
    }

    private void OpenWorldMapAndGoToStage(StageButton stageButton)
    {
        CanvasManager.Instance.OpenMenu(Menu.WorldMap, null, true);
        WorldMap.Instance.SetCurrentStage(stageButton);
    }
    #endregion

    #region Menus
    public void OpenCollectibleUpgradeUI()
    {
        collectibleUpgradeUI.Setup(selectedCollectibleType);
    }

    private void OpenAbilityInfo(CollectibleAbility ability)
    {
        abilityInfoUI.Setup(ability);
    }

    private void OnCollectibleIsUpgraded()
    {
        selectedCollectibleInfoHandler.UpdateCollectibleInfos(selectedCollectibleType);

        if (currentSubMenu == SubMenu.CollectibleProfile)
        {
            selectedCollectibleInfoHandler.SetupProfile(selectedCollectibleType);
        }
    }

    private void OnCollectibleUpgradeUIFindShardsButtonPressed()
    {
        collectibleUpgradeUI.CloseUI();

        SetSubMenu(SubMenu.FindShards);
    }
    #endregion
}
