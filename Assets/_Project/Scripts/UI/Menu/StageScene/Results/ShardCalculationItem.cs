using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShardCalculationItem : RewardCalculationItem
{
    [SerializeField] private Image iconImg = null;
    [SerializeField] private TextMeshProUGUI titleTxt = null;

    public void Setup(GoalReward shardReward)
    {
        valueTxt.text = $"+{shardReward.TotalReward}";
        iconImg.sprite = ProjectAssetsDatabase.Instance.GetStageRewardIcon(shardReward.CurrencyType, shardReward.CollectibleType);
        titleTxt.text = $"{shardReward.Description}";
    }
}
