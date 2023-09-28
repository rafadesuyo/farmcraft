using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyMissionsUI : DailyContentTab
{
    //Constants
    public const string poolDailyMissionItemSmallID = "dailyMissionsUI_DailyMissionItem_Small";
    public const string poolDailyMissionItemNormalID = "dailyMissionsUI_DailyMissionItem_Normal";

    public const string poolRewardInfoID = "dailyMissionsUI_RewardInfo";

    //Components
    [Header("Components")]
    [SerializeField] private GameObject dailyMissionItemSmallPrefab;
    [SerializeField] private GameObject dailyMissionItemNormalPrefab;
    [SerializeField] private GameObject rewardInfoNormalPrefab;

    [Space(10)]

    [SerializeField] private RectTransform dailyMissionItemsSmallContainer;
    [SerializeField] private RectTransform dailyMissionItemsNormalContainer;

    [Space(10)]

    [SerializeField] private Button collectFixedRewardButton;
    [SerializeField] private TextMeshProUGUI playMoreToCollectFixedRewardsText;
    [SerializeField] private TextMeshProUGUI allFixedRewardsCollectedText;

    //Variables
    [Header("Default Variables")]
    [SerializeField] private string textDailyMissions = "Daily Missions";

    [Space(10)]

    [SerializeField] private string textHours = "hour(s)";
    [SerializeField] private string textMinutes = "minute(s)";
    [SerializeField] private string textSeconds = "second(s)";

    protected void OnEnable()
    {
        DailyMissionItem.OnDailyMissionRewardIsCollected += InvokeOnRewardIsCollected;
        EventsManager.AddListener(EventsManager.onMissionIsCompleted, UpdateFixedMissionsState);
    }

    protected void OnDisable()
    {
        DailyMissionItem.OnDailyMissionRewardIsCollected -= InvokeOnRewardIsCollected;
        EventsManager.RemoveListener(EventsManager.onMissionIsCompleted, UpdateFixedMissionsState);
    }

    public override string GetTitleText()
    {
        return textDailyMissions;
    }

    public override string GetRefreshTextValue()
    {
        TimeSpan timeLeftToCompleteMissions = DailyMissionsManager.Instance.GetTimeLeftToCompleteMissions();

        if (timeLeftToCompleteMissions.TotalHours >= 1)
        {
            return $"{timeLeftToCompleteMissions.ToString(@"hh")} {textHours}";
        }
        else if (timeLeftToCompleteMissions.TotalMinutes >= 1)
        {
            return $"{timeLeftToCompleteMissions.ToString(@"mm")} {textMinutes}";
        }
        else
        {
            return $"{timeLeftToCompleteMissions.ToString(@"ss")} {textSeconds}";
        }
    }

    public override void Initialize()
    {
        GenericPool.CreatePool<DailyMissionItem>(dailyMissionItemSmallPrefab, dailyMissionItemsSmallContainer, poolDailyMissionItemSmallID);
        GenericPool.CreatePool<DailyMissionItem>(dailyMissionItemNormalPrefab, dailyMissionItemsNormalContainer, poolDailyMissionItemNormalID);

        GenericPool.CreatePool<RewardInfo>(rewardInfoNormalPrefab, contentLayout, poolRewardInfoID);
    }

    public override void Setup()
    {
        CreateDailyMissionItems();
        UpdateFixedMissionsState(null);
    }

    public override void ResetVariables()
    {
        ReleaseDailyMissionItems();
    }

    private void CreateDailyMissionItems()
    {
        ReleaseDailyMissionItems();

        foreach (DailyMission dailyMission in DailyMissionsManager.Instance.CurrentFixedMissions)
        {
            SetupDailyMissionItem(dailyMission, poolDailyMissionItemSmallID);
        }

        foreach (DailyMission dailyMission in DailyMissionsManager.Instance.CurrentRandomMissions)
        {
            SetupDailyMissionItem(dailyMission, poolDailyMissionItemNormalID);
        }

        void SetupDailyMissionItem(DailyMission dailyMission, string poolID)
        {
            DailyMissionItem dailyMissionItem = GenericPool.GetItem<DailyMissionItem>(poolID);
            dailyMissionItem.Setup(dailyMission);
        }
    }

    private void ReleaseDailyMissionItems()
    {
        foreach (DailyMissionItem dailyMissionItem in dailyMissionItemsSmallContainer.GetComponentsInChildren<DailyMissionItem>())
        {
            dailyMissionItem.Release(poolDailyMissionItemSmallID);
        }

        foreach (DailyMissionItem dailyMissionItem in dailyMissionItemsNormalContainer.GetComponentsInChildren<DailyMissionItem>())
        {
            dailyMissionItem.Release(poolDailyMissionItemNormalID);
        }
    }

    private void UpdateDailyMissionItemsState()
    {
        foreach (DailyMissionItem dailyMissionItem in dailyMissionItemsSmallContainer.GetComponentsInChildren<DailyMissionItem>())
        {
            dailyMissionItem.UpdateItemState();
        }

        foreach (DailyMissionItem dailyMissionItem in dailyMissionItemsNormalContainer.GetComponentsInChildren<DailyMissionItem>())
        {
            dailyMissionItem.UpdateItemState();
        }
    }

    private void UpdateFixedMissionsState(IGameEvent _)
    {
        UpdateDailyMissionItemsState();

        if(DailyMissionsManager.Instance.CanAnyCurrentFixedMissionBeCollected() == true)
        {
            collectFixedRewardButton.gameObject.SetActive(true);
            playMoreToCollectFixedRewardsText.gameObject.SetActive(false);
            allFixedRewardsCollectedText.gameObject.SetActive(false);
        }
        else if (DailyMissionsManager.Instance.IsAnyCurrentFixedMissionIncompleted() == true)
        {
            collectFixedRewardButton.gameObject.SetActive(false);
            playMoreToCollectFixedRewardsText.gameObject.SetActive(true);
            allFixedRewardsCollectedText.gameObject.SetActive(false);
        }
        else
        {
            collectFixedRewardButton.gameObject.SetActive(false);
            playMoreToCollectFixedRewardsText.gameObject.SetActive(false);
            allFixedRewardsCollectedText.gameObject.SetActive(true);
        }
    }

    public void CollectFirstCurrentFixedMissionThatCanBeCollected()
    {
        bool rewardsWereCollected = DailyMissionsManager.Instance.CollectFirstCurrentFixedMissionThatCanBeCollected(out Reward[] rewards);
        
        if(rewardsWereCollected == true)
        {
            InvokeOnRewardIsCollected(rewards);
        }
    }
}
