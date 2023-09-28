using UnityEngine;

public class ShardRewardPackage : MonoBehaviour, IRewardPackage
{
    public int Amount { get; private set; }
    public bool Unpacked { get; private set; }
    public CollectibleType CollectibleType { get; private set; }

    public ShardRewardPackage(int amount, CollectibleType collectibleType)
    {
        Amount = amount;
        CollectibleType = collectibleType;
    }

    public void Unpack()
    {
        if (Unpacked == false)
        {
            return;
        }

        Unpacked = true;
        CollectibleManager.Instance.GiveShardsTo(CollectibleType, Amount);
    }
}