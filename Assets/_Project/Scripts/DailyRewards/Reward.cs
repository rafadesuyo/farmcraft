using UnityEngine;

[System.Serializable]
public struct Reward
{
    //Events
    public delegate void RewardsEvent(Reward[] rewards);

    //Variables
    [SerializeField] private CurrencyType type;

    [Tooltip("The collectible that will receive the shards, if the Reward Type is \"Shard\".\n\"None\" will give the shards to a random collectible.")]
    [SerializeField] private CollectibleType collectibleType;

    [Tooltip("The store item that will be given in the reward, if the Reward Type is \"Store Item\".")]
    [SerializeField] private StoreItemSO storeItem;

    [Space(10)]

    [SerializeField] private int rewardValue;

    //Getters
    public CurrencyType Type => type;
    public CollectibleType CollectibleType => collectibleType;
    public StoreItemSO StoreItem => storeItem;
    public int RewardValue => rewardValue;

    public void CollectReward()
    {
        switch (type)
        {
            case CurrencyType.Gold:
                PlayerProgress.TotalGold += rewardValue;
                break;

            case CurrencyType.Shard:
                {
                    CollectibleType collectibleTypeToReceiveShards = collectibleType;

                    if (collectibleTypeToReceiveShards == CollectibleType.None)
                    {
                        int collectybleTypesLenght = System.Enum.GetValues(typeof(CollectibleType)).Length;

                        collectibleTypeToReceiveShards = (CollectibleType)Random.Range(1, collectybleTypesLenght); //Start in 1 to exclude the "None" value.
                    }

                    CollectibleManager.Instance.GiveShardsTo(collectibleTypeToReceiveShards, rewardValue, true);
                }
                break;

            case CurrencyType.Heart:
                HeartManager.Instance.AddHearts(rewardValue, true);
                break;

            case CurrencyType.PhoenixFeather:
                //TODO: give the Phoenix Feather to the Player. Link: https://ocarinastudios.atlassian.net/browse/DQG-1864?atlOrigin=eyJpIjoiODQ2Zjc2ODZjMzcwNDNmNzgzZWYxZjk3MzBiZTQ5OWYiLCJwIjoiaiJ9
                break;

            case CurrencyType.StoreItem:
                //TODO: give the Store Item to the Player. Link: https://ocarinastudios.atlassian.net/browse/DQG-1864?atlOrigin=eyJpIjoiODQ2Zjc2ODZjMzcwNDNmNzgzZWYxZjk3MzBiZTQ5OWYiLCJwIjoiaiJ9
                break;

            case CurrencyType.XP:
                //TODO: give the XP to the Player. Link: https://ocarinastudios.atlassian.net/browse/DQG-1864?atlOrigin=eyJpIjoiODQ2Zjc2ODZjMzcwNDNmNzgzZWYxZjk3MzBiZTQ5OWYiLCJwIjoiaiJ9
                break;

            default:
                throw new System.ArgumentOutOfRangeException($"The value \"{type}\" of the Reward Type is invalid!");
        }
    }
}
