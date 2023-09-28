using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Collectible Shard Data")]
public class CollectibleShardDataSO : ScriptableObject
{
    [SerializeField] private CollectibleShardDataPair[] collectibleShardIconDataPairs = new CollectibleShardDataPair[0];

    public Sprite GetCollectibleShardIconByType(CollectibleType collectibleType)
    {
        foreach (CollectibleShardDataPair collectibleShardIconReference in collectibleShardIconDataPairs)
        {
            if (collectibleShardIconReference.CollectibleType == collectibleType)
            {
                return collectibleShardIconReference.ShardIcon;
            }
        }

        Debug.LogError($"There's no Shard Icon for the collectible type {collectibleType} in the list!");
        return null;
    }

    [System.Serializable]
    public struct CollectibleShardDataPair
    {
        //Variables
        [SerializeField] private CollectibleType collectibleType;

        [Space(10)]

        [SerializeField] private Sprite shardIcon;

        //Getters
        public CollectibleType CollectibleType => collectibleType;
        public Sprite ShardIcon => shardIcon;
    }
}
