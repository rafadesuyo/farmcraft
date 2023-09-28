using UnityEngine;

public class DailyRewardsUI : DailyContentTab
{
    //Constants
    private const string poolDailyRewardItemSmallID = "dailyRewardsUI_DailyRewardItem_Small";
    private const string poolDailyRewardItemMediumID = "dailyRewardsUI_DailyRewardItem_Medium";
    private const string poolDailyRewardItemBigID = "dailyRewardsUI_DailyRewardItem_Big";

    public const string poolRewardInfoNormalID = "dailyRewardsUI_RewardInfo_Normal";
    public const string poolRewardInfoVertical = "dailyRewardsUI_RewardInfo_Vertical";

    //Components
    [Header("Components")]
    [SerializeField] private GameObject dailyRewardItemSmallPrefab;
    [SerializeField] private GameObject dailyRewardItemMediumPrefab;
    [SerializeField] private GameObject dailyRewardItemBigPrefab;
    [SerializeField] private GameObject rewardInfoNormalPrefab;
    [SerializeField] private GameObject rewardInfoVerticalPrefab;

    [Space(10)]

    [SerializeField] private RectTransform dailyRewardItemsSmallContainer;
    [SerializeField] private RectTransform dailyRewardItemsMediumContainer;
    [SerializeField] private RectTransform dailyRewardItemsBigContainer;

    //Variables
    [Header("Default Variables")]
    [SerializeField] private string textDailyRewards = "Daily Rewards";

    [Space(10)]

    [SerializeField] private string textDays = "day(s)";

    private DailyRewardsListSO currentDailyRewardsList;

    protected void OnEnable()
    {
        DailyRewardItem.OnDailyRewardIsCollected += InvokeOnRewardIsCollected;
    }

    protected void OnDisable()
    {
        DailyRewardItem.OnDailyRewardIsCollected -= InvokeOnRewardIsCollected;
    }

    public override string GetTitleText()
    {
        return textDailyRewards;
    }

    public override string GetRefreshTextValue()
    {
        if(currentDailyRewardsList == null)
        {
            return string.Empty;
        }

        return $"{currentDailyRewardsList.GetDaysLeftToCollectRewards()} {textDays}";
    }

    public override void Initialize()
    {
        GenericPool.CreatePool<DailyRewardItem>(dailyRewardItemSmallPrefab, dailyRewardItemsSmallContainer, poolDailyRewardItemSmallID);
        GenericPool.CreatePool<DailyRewardItem>(dailyRewardItemMediumPrefab, dailyRewardItemsMediumContainer, poolDailyRewardItemMediumID);
        GenericPool.CreatePool<DailyRewardItem>(dailyRewardItemBigPrefab, dailyRewardItemsBigContainer, poolDailyRewardItemBigID);

        GenericPool.CreatePool<RewardInfo>(rewardInfoNormalPrefab, contentLayout, poolRewardInfoNormalID);
        GenericPool.CreatePool<RewardInfo>(rewardInfoVerticalPrefab, contentLayout, poolRewardInfoVertical);
    }

    public override void Setup()
    {
        if (DailyRewardsManager.Instance.Rewards.Length <= 0)
        {
            Debug.LogError("There's no DailyRewardsList in the DailyRewardsManager!");
            return;
        }
        else if(DailyRewardsManager.Instance.Rewards.Length > 1)
        {
            Debug.LogWarning($"There's more than one DailyRewardsList in the Daily Rewards Manager ({DailyRewardsManager.Instance.Rewards.Length}), the DailyRewardsUI doesn't support this, only the first DailyRewardsList will be shown.");
        }

        currentDailyRewardsList = DailyRewardsManager.Instance.Rewards[0];

        UpdateDailyRewardsInfo(currentDailyRewardsList);
    }

    public override void ResetVariables()
    {
        currentDailyRewardsList = null;

        ReleaseDailyRewardItems();
    }

    private void UpdateDailyRewardsInfo(DailyRewardsListSO dailyRewardsList)
    {
        if (dailyRewardsList == null)
        {
            Debug.LogWarning("The Daily Rewards UI was opened with a empty DailyRewardsList!");
            return;
        }

        if (dailyRewardsList.State != DailyRewardsListSO.DailyRewardsListState.Enabled)
        {
            Debug.LogWarning($"The Daily Rewards UI was opened a DailyRewardsList that is not Enabled!\nDaily Rewards List: {dailyRewardsList}, State: {dailyRewardsList.State}.", dailyRewardsList);
            return;
        }

        CreateDailyRewardItems(dailyRewardsList);
    }

    private void CreateDailyRewardItems(DailyRewardsListSO dailyRewardsList)
    {
        ReleaseDailyRewardItems();

        for(int i = 0; i < dailyRewardsList.DailyRewards.Length; i++)
        {
            DailyReward dailyReward = dailyRewardsList.DailyRewards[i];
            string poolID;

            if (i < dailyRewardsList.MediumRewardsStartIndex)
            {
                poolID = poolDailyRewardItemSmallID;
            }
            else if (i < dailyRewardsList.BigRewardsStartIndex)
            {
                poolID = poolDailyRewardItemMediumID;
            }
            else
            {
                poolID = poolDailyRewardItemBigID;
            }

            DailyRewardItem dailyRewardItem = GenericPool.GetItem<DailyRewardItem>(poolID);
            dailyRewardItem.Setup(dailyReward, i);
        }
    }

    private void ReleaseDailyRewardItems()
    {
        foreach (DailyRewardItem dailyRewardItem in dailyRewardItemsSmallContainer.GetComponentsInChildren<DailyRewardItem>())
        {
            dailyRewardItem.Release(poolDailyRewardItemSmallID);
        }

        foreach (DailyRewardItem dailyRewardItem in dailyRewardItemsMediumContainer.GetComponentsInChildren<DailyRewardItem>())
        {
            dailyRewardItem.Release(poolDailyRewardItemMediumID);
        }

        foreach (DailyRewardItem dailyRewardItem in dailyRewardItemsBigContainer.GetComponentsInChildren<DailyRewardItem>())
        {
            dailyRewardItem.Release(poolDailyRewardItemBigID);
        }
    }
}
