using System;
using System.Text;
using TMPro;
using UnityEngine;

public class DailyContentView : MenuView
{
    //Components
    [Header("Components")]
    [SerializeField] private PaginationBehavior paginationBehavior;

    [Space(10)]

    [SerializeField] private TextMeshProUGUI tabTitleText;
    [SerializeField] private TextMeshProUGUI refreshText;

    [Header("Tabs")]
    [SerializeField] private DailyContentTab[] dailyContentTabs;

    [Header("UIs")]
    [SerializeField] private YouGotRewardsUI youGotRewardsUI;

    //Events
    public static event Action OnUIIsClosed;

    //Variables
    [Header("Default Variables")]
    [SerializeField] private string textToReplaceWithColor = "{color}";
    [SerializeField] private string textToReplaceWithValue = "{value}";
    [SerializeField][TextArea] private string textRefreshesIn = "Refreshes in\n<color=#{color}>{value}</color>";

    [Space(10)]

    [SerializeField] private Color refreshValueTextColor;

    private bool updateView = true;

    //Getters
    public override Menu Type => Menu.DailyContent;

    private void OnEnable()
    {
        DailyContentTab.OnRewardIsCollected += OnRewardsAreCollected;
    }

    private void OnDisable()
    {
        DailyContentTab.OnRewardIsCollected -= OnRewardsAreCollected;
    }

    public void UpdateTexts(DailyContentTab dailyContentTab)
    {
        tabTitleText.text = dailyContentTab.GetTitleText();

        SetRefreshText(dailyContentTab.GetRefreshTextValue());
    }

    public override void Initialize()
    {
        InitializeDailyContentTabs();
    }

    protected override void Setup(MenuSetupOptions setupOptions)
    {
        //Don't setup the menu again if it wasn't closed
        if (updateView == false)
        {
            return;
        }

        updateView = false;

        SetupDailyContentTabs();

        paginationBehavior.Elements[0].Open();
    }

    protected override void OnClose()
    {
        updateView = true;

        ResetVariables();

        OnUIIsClosed?.Invoke();
    }

    private void ResetVariables()
    {
        youGotRewardsUI.CloseUI();

        ResetDailyContentTabs();
    }

    private void SetRefreshText(string value)
    {
        StringBuilder valueText = new StringBuilder(textRefreshesIn);

        valueText.Replace(textToReplaceWithColor, ColorUtility.ToHtmlStringRGB(refreshValueTextColor));
        valueText.Replace(textToReplaceWithValue, value);

        refreshText.text = valueText.ToString();
    }

    private void OnRewardsAreCollected(Reward[] rewards)
    {
        youGotRewardsUI.Setup(rewards);
    }

    private void InitializeDailyContentTabs()
    {
        foreach (DailyContentTab dailyContentTab in dailyContentTabs)
        {
            dailyContentTab.Initialize();
        }
    }

    private void SetupDailyContentTabs()
    {
        foreach (DailyContentTab dailyContentTab in dailyContentTabs)
        {
            dailyContentTab.Setup();
        }
    }

    private void ResetDailyContentTabs()
    {
        foreach (DailyContentTab dailyContentTab in dailyContentTabs)
        {
            dailyContentTab.ResetVariables();
        }
    }
}
