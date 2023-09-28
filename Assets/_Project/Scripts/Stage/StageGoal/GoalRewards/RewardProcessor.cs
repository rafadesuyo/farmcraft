using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class RewardProcessor
{
    private static Dictionary<CurrencyType, StageRewardGiver> rewardGiverDict = new Dictionary<CurrencyType, StageRewardGiver>();
    private static bool initialized = false;

    private static void InitializeRewardGivers()
    {
        rewardGiverDict.Clear();

        var allRewardGiverTypes = Assembly.GetAssembly(typeof(StageRewardGiver)).GetTypes()
            .Where(t => typeof(StageRewardGiver).IsAssignableFrom(t) && !t.IsAbstract);

        foreach (var rewardGiverType in allRewardGiverTypes)
        {
            var rewardGiverInstance = Activator.CreateInstance(rewardGiverType) as StageRewardGiver;
            var currencyType = rewardGiverInstance.GetCurrencyType();

            if (rewardGiverDict.ContainsKey(currencyType))
            {
                Debug.LogError($"[StageRewardGiver] Duplicate currency '{currencyType}' type implementation");
                return;
            }

            rewardGiverDict.Add(rewardGiverInstance.GetCurrencyType(), rewardGiverInstance);
        }

        initialized = true;
    }

    public static IRewardPackage GiveReward(StageGoalProgress stageGoalProgress)
    {
        if (!initialized)
        {
            InitializeRewardGivers();
        }

        var currencyType = stageGoalProgress.StageGoal.GoalReward.CurrencyType;

        if (rewardGiverDict.TryGetValue(currencyType, out var currencyGiver))
        {
            return currencyGiver.GetReward(stageGoalProgress);
        }

        Debug.LogError($"[StageRewardGiver] Missing '{currencyType}' currency implementation");
        return null;
    }
}
