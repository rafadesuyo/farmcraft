using System.Collections.Generic;

public enum Menu
{
    None,
    Home,
    Profile,
    Achievements,
    Store,
    Collection,
    CollectibleInfo,
    AbilitiesInfo,
    WorldMap,
    TeamPreview,
    StageInfo,
    Stage,
    Quiz,
    StageResult,
    HeartPopup,
    GoldPopup,
    ShardsReceived,
    PurchaseConfirmation,
    Leaderboard,
    TeamSelection,
    DailyContent,
    LootboxPurchaseConfirmationView,
    PillowShop
}

public class CanvasManager : LocalSingleton<CanvasManager>
{
    private List<MenuView> gameMenus = new List<MenuView>();
    private List<MenuHistoryItem> menuHistory = new List<MenuHistoryItem>();

    private Menu CurrentMenu => menuHistory[menuHistory.Count - 1].menu;
    private Menu PreviousMenu => menuHistory[menuHistory.Count - 2].menu;

    public void OpenMenu(Menu type, MenuSetupOptions setupOptions = null, bool hidePreviousMenu = false, bool addToHistory = true)
    {
        if (hidePreviousMenu)
        {
            CloseMenu(CurrentMenu);
        }

        if (addToHistory)
        {
            HandleHistory(type, setupOptions, hidePreviousMenu);
        }

        GetMenuByType(type).Open(setupOptions);
    }

    public void ReturnMenu()
    {
        ReturnHistory(GetHistoryItemByMenu(PreviousMenu).setupOptions);
    }

    private void OnEnable()
    {
        EventsManager.AddListener(EventsManager.onSceneLoad, OnSceneLoad);
    }

    private void OnDisable()
    {
        EventsManager.RemoveListener(EventsManager.onSceneLoad, OnSceneLoad);
    }

    private void SetupSceneMenus()
    {
        ClearMenuHistory();
        ClearHistoryObjects();

        MenuView[] sceneMenus = FindObjectsOfType<MenuView>(true);

        foreach (MenuView menu in sceneMenus)
        {
            InitializeMenu(menu);
        }
    }

    private void InitializeMenu(MenuView menuView)
    {
        menuView.Initialize();
        gameMenus.Add(menuView);
        CloseMenu(menuView.Type);
    }

    private void ClearHistoryObjects()
    {
        gameMenus.Clear();
    }

    private void ClearMenuHistory()
    {
        menuHistory.Clear();
        menuHistory.Add(new MenuHistoryItem());
    }

    private void HandleHistory(Menu openRequested, MenuSetupOptions menuSetupOptions, bool hidePreviousMenu)
    {
        HandleHistoryDuplicity(openRequested);
        AddNewHistory(openRequested, menuSetupOptions);
    }

    private void HandleHistoryDuplicity(Menu openRequested)
    {
        if (HasHistoryItemOfMenu(openRequested))
        {
            MenuHistoryItem historyItem = menuHistory.Find(history => history.menu == openRequested);
            int index = menuHistory.IndexOf(historyItem);

            for (int i = menuHistory.Count - 1; i >= index; i--)
            {
                CloseMenu(menuHistory[i].menu);
                RemoveHistoryItem();
            }
        }
    }

    private void ReturnHistory(MenuSetupOptions setupOptions = null)
    {
        OpenMenu(PreviousMenu, setupOptions, true, false);
        RemoveHistoryItem();
    }

    private bool HasHistoryItemOfMenu(Menu menu)
    {
        return GetHistoryItemByMenu(menu) != null;
    }

    private MenuHistoryItem GetHistoryItemByMenu(Menu menu)
    {
        return menuHistory.Find(history => history.menu == menu);
    }

    private void AddNewHistory(Menu menu, MenuSetupOptions setupOptions = null)
    {
        menuHistory.Add(new MenuHistoryItem(menu, setupOptions));
    }

    private void RemoveHistoryItem()
    {
        menuHistory.RemoveAt(menuHistory.Count - 1);
    }

    private void CloseMenu(Menu type)
    {
        if (type == Menu.None)
        {
            return;
        }

        var menu = GetMenuByType(type);

        if (menu == null)
        {
            return;
        }

        menu.Close();
    }

    private MenuView GetMenuByType(Menu type)
    {
        return gameMenus.Find(menu => menu.Type == type);
    }

    private void OnSceneLoad(IGameEvent gameEvent)
    {
        SetupSceneMenus();
    }
}
