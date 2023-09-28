[System.Serializable]
public class MenuHistoryItem
{
    public Menu menu;
    public MenuSetupOptions setupOptions;

    public MenuHistoryItem()
    {
        menu = Menu.None;
        setupOptions = null;
    }

    public MenuHistoryItem(Menu newMenu, MenuSetupOptions newOptions = null)
    {
        menu = newMenu;
        setupOptions = newOptions;
    }
}
