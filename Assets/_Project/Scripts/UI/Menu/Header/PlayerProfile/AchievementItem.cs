using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class AchievementItem : MonoBehaviour
{
    [SerializeField] private Image iconImg = null;
    [SerializeField] private TextMeshProUGUI nameText = null;
    [SerializeField] private TextMeshProUGUI descriptionText = null;
    [SerializeField] private Image containerBackgroundImg = null;
    [SerializeField] private Image completedContainerBackgroundImg = null;
    [SerializeField] private Outline containerOutline = null;

    [Space(10)]

    [SerializeField] private Color inProgressContainerColor = Color.white;
    [SerializeField] private Color noRewardsContainerColor = Color.white;

    [Space(10)]

    [SerializeField] private Color inProgressOutlineColor = Color.white;
    [SerializeField] private Color completedOutlineColor = Color.white;
    [SerializeField] private Color noRewardsOutlineColor = Color.white;

    [Header("Rewards")]
    [SerializeField] private GameObject bonusRewardContainer = null;
    [SerializeField] private Image bonusIcon = null;
    [SerializeField] private TextMeshProUGUI bonusRewardText = null;

    [Header("Progress")]
    [SerializeField] private GameObject collectRewardButton = null;
    [SerializeField] private GameObject progressContainer = null;
    [SerializeField] private TextMeshProUGUI progressText = null;
    [SerializeField] private Slider progressSlider = null;
    [SerializeField] private Image progressSliderOutline = null;
    [SerializeField] private Color progressSliderOutlineColor = Color.white;
    [SerializeField] private Image progressBarImg = null;
    [SerializeField] private Color inProgressBarColor = Color.white;
    [SerializeField] private Color completedBarColor = Color.white;

    private Achievement achievement = null;

    public delegate void AchievementEvent(Achievement achievement);
    private AchievementEvent rewardCallback = null;

    public void Setup(Achievement newAchievement, AchievementEvent onCollectRewardCallback)
    {
        achievement = newAchievement;
        rewardCallback += onCollectRewardCallback;

        iconImg.sprite = achievement.Data.Icon;
        nameText.text = achievement.Data.Id;
        descriptionText.text = achievement.Data.Description;

        SetupBonusRewards();
        SetupProgress();
    }

    // Used by UI
    public void CollectReward()
    {
        achievement.CollectReward();
        rewardCallback?.Invoke(achievement);
    }

    private void OnDisable()
    {
        this.ReleaseItem();
    }

    private void SetupBonusRewards()
    {
        if (achievement.Data.RewardType == AchievementReward.None)
        {
            bonusRewardContainer.SetActive(false);
            return;
        }

        bonusRewardContainer.SetActive(true);
        // bonusIcon.sprite = TODO: Get icon by type from achievement manager
        bonusRewardText.text = $"+{achievement.Data.RewardType} max";
    }

    private void SetupProgress()
    {
        iconImg.sprite = achievement.Data.Icon;
        if (achievement.CanCollectReward)
        {
            collectRewardButton.SetActive(true);
            progressContainer.SetActive(false);
            containerBackgroundImg.gameObject.SetActive(false);
            completedContainerBackgroundImg.gameObject.SetActive(true);
            containerOutline.effectColor = completedOutlineColor;
            return;
        }

        int progress = Mathf.Clamp(achievement.Progress.value, 0, achievement.Data.ProgressNeeded);
        progressSlider.value = Mathf.InverseLerp(0, achievement.Data.ProgressNeeded, progress);
        progressText.text = $"{progress}/{achievement.Data.ProgressNeeded}";

        collectRewardButton.SetActive(false);
        progressContainer.SetActive(true);

        if (achievement.Unlocked)
        {
            containerBackgroundImg.color = noRewardsContainerColor;
            containerOutline.effectColor = noRewardsOutlineColor;
            progressBarImg.color = completedBarColor;
            progressText.color = completedBarColor;
            progressSliderOutline.color = progressSliderOutlineColor;
            containerBackgroundImg.gameObject.SetActive(true);
            completedContainerBackgroundImg.gameObject.SetActive(false);
            return;
        }

        containerBackgroundImg.color = inProgressContainerColor;
        containerOutline.effectColor = inProgressOutlineColor;
        progressBarImg.color = inProgressBarColor;
        progressText.color = inProgressBarColor;
        iconImg.sprite = achievement.Data.NegativeIcon;
    }
}
