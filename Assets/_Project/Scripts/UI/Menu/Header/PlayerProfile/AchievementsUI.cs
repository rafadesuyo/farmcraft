using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsUI : MonoBehaviour
{
    [SerializeField] private Transform itemContainer = null;
    [SerializeField] private GameObject achievementItemPrefab = null;

    [Space(10)]

    [SerializeField] private RectTransform bonusInfoUI = null;

    [Space(10)]

    [SerializeField] private RectTransform achievementCollectedUI = null;
    [SerializeField] private Image achievementIconImage = null;
    [SerializeField] private TextMeshProUGUI achievementNameText = null;
    [SerializeField] private TextMeshProUGUI goldValueText = null;
    [SerializeField] private TextMeshProUGUI currentGoldValueText = null;

    private void OnEnable()
    {
        Setup();
    }

    public void Setup()
    {
        GenericPool.CreatePool<AchievementItem>(achievementItemPrefab, itemContainer);
        PopulateAchievements();
    }

    public void BonusInfoUIVisible(bool value)
    {
        bonusInfoUI.gameObject.SetActive(value);
    }

    public void OpenAchievementCollectedUI(Achievement achievement)
    {
        achievementCollectedUI.gameObject.SetActive(true);

        achievementIconImage.sprite = achievement.Data.Icon;
        achievementNameText.text = achievement.Data.Id;
        goldValueText.text = $"+{achievement.Data.GoldRewardValue}";
        currentGoldValueText.text = PlayerProgress.TotalGold.ToString();
    }

    public void CloseAchievementCollectedUI()
    {
        achievementCollectedUI.gameObject.SetActive(false);

        achievementIconImage.sprite = null;
        achievementNameText.text = string.Empty;
        goldValueText.text = string.Empty;
        currentGoldValueText.text = string.Empty;
    }

    private void PopulateAchievements()
    {
        for (int i = 0; i < itemContainer.childCount; i++)
        {
            itemContainer.GetChild(i).gameObject.SetActive(false);
        }

        List<Achievement> achievements = new List<Achievement>();

        achievements.AddRange(AchievementManager.Instance.Achievements.FindAll(a => a.CanCollectReward));
        achievements.AddRange(AchievementManager.Instance.Achievements.FindAll(a => a.Unlocked && !a.CanCollectReward));
        achievements.AddRange(AchievementManager.Instance.Achievements.FindAll(a => !a.Unlocked));

        foreach (Achievement achievement in achievements)
        {
            var achievementObject = GenericPool.GetItem<AchievementItem>();
            achievementObject.Setup(achievement, OnAchievementCollected);
        }
    }

    private void OnAchievementCollected(Achievement achievement)
    {
        OpenAchievementCollectedUI(achievement);
        PopulateAchievements();
    }
}
