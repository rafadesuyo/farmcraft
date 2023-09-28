using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Stage Reward Data")]
public class StageRewardDataSO : ScriptableObject
{
    //Variables
    [SerializeField] private StageRewardDataPair[] stageRewardDataPairs;

    public Sprite GetIconByTypeAndCollectible(CurrencyType type, CollectibleType collectible = CollectibleType.None)
    {
        if (type == CurrencyType.Shard)
        {
            return ProjectAssetsDatabase.Instance.GetCollectibleShardIcon(collectible);
        }

        foreach (StageRewardDataPair stageRewardDataPair in stageRewardDataPairs)
        {
            if (stageRewardDataPair.Type == type)
            {
                return stageRewardDataPair.RewardIcon;
            }
        }

        throw new System.Exception($"The reward type \"{type}\" isn't present in the data list!");
    }

    [System.Serializable]
    public class StageRewardDataPair
    {
        //Variables
        [SerializeField] private CurrencyType type;

        [Space(10)]

        [SerializeField] private Sprite rewardIcon;

        //Getters
        public CurrencyType Type => type;
        public Sprite RewardIcon => rewardIcon;
    }
}
