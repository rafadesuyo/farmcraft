using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardInfo : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private Image rewardIcon;
    [SerializeField] private TextMeshProUGUI rewardValueText;

    //Variables
    [Header("Icons")]
    [SerializeField] private Sprite goldIcon;
    [SerializeField] private Sprite randomShardIcon;
    [SerializeField] private Sprite heartIcon;
    [SerializeField] private Sprite phoenixFeatherIcon;

    [Header("Colors")]
    [SerializeField] private Color goldColor;
    [SerializeField] private Color shardColor;
    [SerializeField] private Color heartColor;
    [SerializeField] private Color phoenixFeatherColor;
    [SerializeField] private Color storeItemColor;
    [SerializeField] private Color xpColor;

    public void Setup(Reward reward)
    {
        rewardIcon.sprite = GetRewardIcon(reward);
        rewardIcon.gameObject.SetActive(GetRewardIconState(reward));

        rewardValueText.color = GetRewardColor(reward);
        rewardValueText.text = $"+{reward.RewardValue}";
    }

    private void ResetVariables()
    {
        rewardIcon.sprite = null;
        rewardValueText.text = string.Empty;
    }

    public void Release(string poolID)
    {
        ResetVariables();

        GenericPool.ReleaseItem(GetType(), this, poolID);
    }

    private Sprite GetRewardIcon(Reward reward)
    {
        switch (reward.Type)
        {
            case CurrencyType.Gold:
                return goldIcon;

            case CurrencyType.Shard:
                if (reward.CollectibleType == CollectibleType.None)
                {
                    return randomShardIcon;
                }
                else
                {
                    return ProjectAssetsDatabase.Instance.GetCollectibleShardIcon(reward.CollectibleType);
                }

            case CurrencyType.Heart:
                return heartIcon;

            case CurrencyType.PhoenixFeather:
                return phoenixFeatherIcon;

            case CurrencyType.StoreItem:
                return reward.StoreItem.Icon;

            case CurrencyType.XP:
                return null;

            default:
                throw new System.ArgumentOutOfRangeException($"The value \"{reward.Type}\" of the Reward Type is invalid!");
        }
    }

    private bool GetRewardIconState(Reward reward)
    {
        return reward.Type != CurrencyType.XP;
    }

    private Color GetRewardColor(Reward reward)
    {
        switch (reward.Type)
        {
            case CurrencyType.Gold:
                return goldColor;

            case CurrencyType.Shard:
                return shardColor;

            case CurrencyType.Heart:
                return heartColor;

            case CurrencyType.PhoenixFeather:
                return phoenixFeatherColor;

            case CurrencyType.StoreItem:
                return storeItemColor;

            case CurrencyType.XP:
                return xpColor;

            default:
                throw new System.ArgumentOutOfRangeException($"The value \"{reward.Type}\" of the Reward Type is invalid!");
        }
    }
}
