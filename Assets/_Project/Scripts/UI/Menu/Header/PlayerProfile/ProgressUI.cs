using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressUI : MonoBehaviour
{
    [SerializeField] PlayerLevelXPDatabaseSO playerLevelXPDatabase;
    [Space(10)]
    [Header("Text references")]
    [SerializeField] TextMeshProUGUI levelInfoText;
    [SerializeField] TextMeshProUGUI sleepingTimeValueText;
    [SerializeField] TextMeshProUGUI goldRewardValueText;
    [SerializeField] TextMeshProUGUI levelUnlockRewardText;
    [SerializeField] TextMeshProUGUI xpToNextLevelText;
    [SerializeField] Color xpToNextLevelColor = Color.green;
    [Header("LevelInfoItem")]
    [SerializeField] GameObject levelInfoItemPrefab;
    [SerializeField] Transform itemsContainer;

    private const string levelInfoText_Prefix = "LEVEL ";
    private const string maxLevelReachedText = "MAX LEVEL!";
    int lastUpdatedPlayerLevel = 0;

    private void OnEnable()
    {
        UpdateValuesIfNecessary();
        UpdateXPRequiredToNextLevel();
    }

    private void Start()
    {
        CreateLevelInfoItems();
    }

    public void UpdateValuesIfNecessary()
    {
        if (lastUpdatedPlayerLevel == PlayerProgress.SaveState.currentPlayerLevel)
        {
            return;
        }

        levelInfoText.text = levelInfoText_Prefix + PlayerProgress.SaveState.currentPlayerLevel;
        sleepingTimeValueText.text = playerLevelXPDatabase.GetMaxSleepingEnergyByLevel(PlayerProgress.SaveState.currentPlayerLevel).ToString();
        goldRewardValueText.text = playerLevelXPDatabase.GetGoldRewardByLevel(PlayerProgress.SaveState.currentPlayerLevel).ToString();
        levelUnlockRewardText.text = playerLevelXPDatabase.GetUnlockRewardTextByLevel(PlayerProgress.SaveState.currentPlayerLevel).ToString();

        lastUpdatedPlayerLevel = PlayerProgress.SaveState.currentPlayerLevel;
    }

    private void UpdateXPRequiredToNextLevel()
    {
        if (playerLevelXPDatabase.IsAtMaxLevel(PlayerProgress.SaveState.currentPlayerLevel))
        {
            xpToNextLevelText.text = maxLevelReachedText;
        }
        else
        {
            string colorHex = ColorUtility.ToHtmlStringRGBA(xpToNextLevelColor);
            int xpToNextLevel = playerLevelXPDatabase.GetXPToNextLevel(PlayerProgress.SaveState.currentPlayerLevel, PlayerProgress.SaveState.totalXP);
            xpToNextLevelText.text = $"GET <color=#{colorHex}>+{xpToNextLevel}xp</color> to " +
                                     $"upgrade to \n<color=#{colorHex}>LEVEL {PlayerProgress.SaveState.currentPlayerLevel + 1}</color>";
        }
    }

    private void CreateLevelInfoItems()
    {
        foreach (var item in playerLevelXPDatabase.levelDataList)
        {
            GameObject instantiatedLevelInfoItem = Instantiate(levelInfoItemPrefab, itemsContainer);
            instantiatedLevelInfoItem.GetComponent<LevelInfoItem>().Build(item.level, playerLevelXPDatabase);
        }
    }
}
