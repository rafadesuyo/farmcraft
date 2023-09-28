using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldRewardPackage : MonoBehaviour, IRewardPackage
{
    public int Amount { get; private set; }
    public bool isUnpacked { get; private set; }

    public GoldRewardPackage(int amount)
    {
        Amount = amount;
        isUnpacked = false;
    }

    public void Unpack()
    {
        if (isUnpacked)
        {
            return;
        }

        isUnpacked = true;
        PlayerProgress.TotalGold += Amount;
    }
}
