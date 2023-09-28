using UnityEngine;

[System.Serializable]
public class GoalReward
{
    [SerializeField] private int totalReward;
    [SerializeField] private CurrencyType currencyType;
    [SerializeField] private CollectibleType collectibleType;
    [SerializeField] private bool givePartialReward;

    public int TotalReward
    {
        get
        {
            return totalReward;
        }
    }

    public CurrencyType CurrencyType
    {
        get
        {
            return currencyType;
        }
    }

    public CollectibleType CollectibleType
    {
        get
        {
            return collectibleType;
        }
    }

    public bool GivePartialReward
    {
        get
        {
            return givePartialReward;
        }
    }

    public string Description
    {
        get
        {
            string rewardTypeText = $"{currencyType}";

            if (collectibleType != CollectibleType.None)
            {
                rewardTypeText = $"{collectibleType} shards";
            }

            return $"{rewardTypeText}";
        }
    }

    public string ValueAsDescription
    {
        get
        {
            return $"+{totalReward}";
        }
    }

    public GoalReward(int value, CurrencyType currencyType, CollectibleType collectibleType, bool givePartialReward = false)
    {
        this.totalReward = value;
        this.currencyType = currencyType;
        this.collectibleType = collectibleType;
        this.givePartialReward = givePartialReward;
    }
}
