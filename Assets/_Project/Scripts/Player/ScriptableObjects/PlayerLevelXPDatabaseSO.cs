using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerLevelXPDatabase", menuName = "PlayerLevelXPDatabase")]
public class PlayerLevelXPDatabaseSO : ScriptableObject
{
    [System.Serializable]
    public class LevelData
    {
        public int level;
        public int xpRequired;
        public int maxSleepingEnergy;
        public int goldReward;
        public string levelUnlockReward;
    }

    public List<LevelData> levelDataList;

    public bool IsAtMaxLevel(int currentLevel)
    {
        return currentLevel >= levelDataList[levelDataList.Count - 1].level;
    }

    public bool CanLevelUp(int currentLevel, int currentXP)
    {
        if (IsAtMaxLevel(currentLevel))
        {
            return false;
        }

        for (int i = 0; i < levelDataList.Count; i++)
        {
            if (levelDataList[i].level == currentLevel + 1)
            {
                return (currentXP >= levelDataList[i].xpRequired);
            }
        }

        Debug.LogError("Could not find CanLevelUp info.");
        return false;
    }

    public int GetLevelByCurrentXP(int currentXP)
    {
        if (currentXP < levelDataList[0].xpRequired)
        {
            return 1;
        }

        if (currentXP >= levelDataList[levelDataList.Count - 1].xpRequired)
        {
            return levelDataList[levelDataList.Count - 1].level;
        }

        for (int i = levelDataList.Count - 1; i >= 0; i--)
        {
            if (currentXP >= levelDataList[i].xpRequired)
            {
                return levelDataList[i].level;
            }
        }

        return 0;
    }

    public int GetMaxSleepingEnergyByLevel(int currentLevel)
    {
        foreach (var item in levelDataList)
        {
            if (item.level == currentLevel)
            {
                return item.maxSleepingEnergy;
            }
        }

        return -1;
    }

    public int GetGoldRewardByLevel(int currentLevel)
    {
        foreach (var item in levelDataList)
        {
            if (item.level == currentLevel)
            {
                return item.goldReward;
            }
        }

        return -1;
    }

    public string GetUnlockRewardTextByLevel(int currentLevel)
    {
        foreach (var item in levelDataList)
        {
            if (item.level == currentLevel)
            {
                return item.levelUnlockReward;
            }
        }

        return "NULL";
    }

    public int GetXPToNextLevel(int currentLevel, int currentXP)
    {
        int nextLevel = currentLevel + 1;

        foreach (var item in levelDataList)
        {
            if (item.level == nextLevel)
            {
                return item.xpRequired - currentXP;
            }
        }

        return -1;
    }

    public int GetRequiredXP(int currentLevel)
    {

        foreach (var item in levelDataList)
        {
            if (item.level == currentLevel)
            {
                return item.xpRequired;
            }
        }

        return 0;
    }
}
