using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelInfoItem : MonoBehaviour
{
    [Space(10)]
    [SerializeField] TextMeshProUGUI levelIndexText;
    [SerializeField] TextMeshProUGUI sleepingEnergyValueText;
    [SerializeField] Button unlockInfoButton;
    [SerializeField] TextMeshProUGUI requiredXPValueText;
    [SerializeField] TextMeshProUGUI goldRewardValueText;

    public void Build(int levelIndex, PlayerLevelXPDatabaseSO playerLevelXPDatabase)
    {
        levelIndexText.text = levelIndex.ToString();
        sleepingEnergyValueText.text = playerLevelXPDatabase.GetMaxSleepingEnergyByLevel(levelIndex).ToString();
        requiredXPValueText.text = ValueToStringFormatter(playerLevelXPDatabase.GetRequiredXP(levelIndex));
        goldRewardValueText.text = ValueToStringFormatter(playerLevelXPDatabase.GetGoldRewardByLevel(levelIndex));
        unlockInfoButton.interactable = (levelIndex != 1);
    }

    private string ValueToStringFormatter(int value)
    {
        if (value == 0)
        {
            return "-";
        }
        return value.ToString();
    }
}
