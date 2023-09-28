using TMPro;
using UnityEngine;

public class YouGotRewardsUI : UIControllerAnimated
{
    //Constants
    public const string poolRewardInfoID = "youGotRewardsUI";

    //Components
    [Header("Components")]
    [SerializeField] private GameObject rewardInfoPrefab;

    [Space(10)]

    [SerializeField] private TextMeshProUGUI currentGoldText;
    [SerializeField] private RectTransform rewardInfosContainer;

    protected override void OnAwake()
    {
        GenericPool.CreatePool<RewardInfo>(rewardInfoPrefab, rewardInfosContainer, poolRewardInfoID);
    }

    protected override void OnClose()
    {
        ResetVariables();
    }

    public void Setup(Reward[] rewards)
    {
        OpenUI();

        currentGoldText.text = PlayerProgress.TotalGold.ToString();

        CreateRewardInfos(rewards);
    }

    private void ResetVariables()
    {
        currentGoldText.text = string.Empty;

        ReleaseRewardInfos();
    }

    private void CreateRewardInfos(Reward[] rewards)
    {
        ReleaseRewardInfos();

        foreach (Reward reward in rewards)
        {
            RewardInfo rewardInfo = GenericPool.GetItem<RewardInfo>(poolRewardInfoID);
            rewardInfo.Setup(reward);
        }
    }

    private void ReleaseRewardInfos()
    {
        RewardInfo[] rewardInfos = rewardInfosContainer.GetComponentsInChildren<RewardInfo>();

        foreach (RewardInfo rewardInfo in rewardInfos)
        {
            rewardInfo.Release(poolRewardInfoID);
        }
    }
}
