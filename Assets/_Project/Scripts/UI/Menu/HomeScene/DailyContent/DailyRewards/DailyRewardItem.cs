using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardItem : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI rewardDayText;
    [SerializeField] private RectTransform rewardInfosContainer;

    [Space(10)]

    [SerializeField] private Button collectRewardButton;
    [SerializeField] private TextMeshProUGUI rewardCollectedText;

    //Enums
    public enum RewardInfoType { Normal, Vertical }

    //Events
    public static event Reward.RewardsEvent OnDailyRewardIsCollected;

    //Variables
    [Header("Default Variables")]
    [SerializeField] private Color colorRewardCantBeCollected;
    [SerializeField] private Color colorRewardCanBeCollected;
    [SerializeField] private Color colorRewardWasCollected;

    [Header("Variables")]
    [SerializeField] private RewardInfoType rewardInfoType = RewardInfoType.Normal;

    private DailyReward currentDailyReward;

    public void Setup(DailyReward dailyReward, int rewardIndex)
    {
        currentDailyReward = dailyReward;

        UpdateVariables(rewardIndex);
    }

    public void CollectReward()
    {
        if(currentDailyReward.State != DailyReward.RewardState.CanBeCollected)
        {
            Debug.LogWarning("The Collect Reward Button is enabled but the Daily Reward state isn't \"Can Be Collected\"!");
            return;
        }

        currentDailyReward.CollectRewards();

        UpdateItemState();

        GameManager.Instance.SaveGame();

        OnDailyRewardIsCollected?.Invoke(currentDailyReward.Rewards);
    }

    public void Release(string poolID)
    {
        ResetVariables();

        GenericPool.ReleaseItem(GetType(), this, poolID);
    }

    private void UpdateVariables(int rewardIndex)
    {
        rewardDayText.text = $"Day {rewardIndex + 1}";

        CreateRewardInfos();

        UpdateItemState();
    }

    private void ResetVariables()
    {
        currentDailyReward = null;

        rewardDayText.text = string.Empty;

        ReleaseRewardInfos();
    }

    private void CreateRewardInfos()
    {
        ReleaseRewardInfos();
        
        string poolID = GetRewardInfoPoolID();

        foreach(Reward reward in currentDailyReward.Rewards)
        {
            RewardInfo rewardInfo = GenericPool.GetItem<RewardInfo>(poolID);
            rewardInfo.transform.SetParent(rewardInfosContainer);

            rewardInfo.Setup(reward);
        }
    }

    private void ReleaseRewardInfos()
    {
        RewardInfo[] rewardInfos = rewardInfosContainer.GetComponentsInChildren<RewardInfo>();
        string poolID = GetRewardInfoPoolID();

        foreach (RewardInfo rewardInfo in rewardInfos)
        {
            rewardInfo.Release(poolID);
        }
    }

    private string GetRewardInfoPoolID()
    {
        switch(rewardInfoType)
        {
            case RewardInfoType.Normal:
                return DailyRewardsUI.poolRewardInfoNormalID;

            case RewardInfoType.Vertical:
                return DailyRewardsUI.poolRewardInfoVertical;

            default:
                throw new System.Exception($"The value \"{rewardInfoType}\" of the Reward Info Type is invalid!");
        }
    }

    private void UpdateItemState()
    {
        collectRewardButton.gameObject.SetActive(currentDailyReward.State != DailyReward.RewardState.WasCollected);
        collectRewardButton.interactable = (currentDailyReward.State == DailyReward.RewardState.CanBeCollected);

        rewardCollectedText.gameObject.SetActive(currentDailyReward.State == DailyReward.RewardState.WasCollected);

        switch(currentDailyReward.State)
        {
            case DailyReward.RewardState.CantBeCollected:
                backgroundImage.color = colorRewardCantBeCollected;
                break;

            case DailyReward.RewardState.CanBeCollected:
                backgroundImage.color = colorRewardCanBeCollected;
                break;

            case DailyReward.RewardState.WasCollected:
                backgroundImage.color = colorRewardWasCollected;
                break;
        }
    }
}
