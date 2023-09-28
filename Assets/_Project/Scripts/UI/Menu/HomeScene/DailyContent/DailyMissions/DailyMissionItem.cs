using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyMissionItem : MonoBehaviour
{
    //Components
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI missionNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private RectTransform rewardInfosContainer;

    [Space(10)]

    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private Image progressIconImage;
    [SerializeField] private Image progressIconContainerImage;

    [Space(10)]

    [SerializeField] private Image containerImage;
    [SerializeField] private Image containerOutlineImage;

    [Space(10)]

    [SerializeField] private Button goToMissionButton;
    [SerializeField] private Button collectRewardButton;
    [SerializeField] private TextMeshProUGUI rewardCollectedText;

    //Events
    public static event Reward.RewardsEvent OnDailyMissionRewardIsCollected;

    //Variables
    [Header("Default Variables")]
    [SerializeField] private DailyMissionContainerColorSO completedMissionContainerColor;

    [Space(10)]

    [SerializeField] private Sprite activeMissionProgressSprite;
    [SerializeField] private Sprite activeMissionContainerSprite;

    [SerializeField] private Sprite completedMissionProgressSprite;
    [SerializeField] private Sprite completedMissionContainerSprite;

    private DailyMission currentMission;

    public void Setup(DailyMission mission)
    {
        currentMission = mission;

        UpdateVariables();
    }

    public void GoToMission()
    {
        Action goToMission = currentMission.DailyMissionSO.GoToMission;

        CanvasManager.Instance.ReturnMenu(); //Close the DailyContetView

        goToMission.Invoke();
    }

    public void CollectRewards()
    {
        currentMission.CollectRewards();

        GameManager.Instance.SaveGame();

        OnDailyMissionRewardIsCollected?.Invoke(currentMission.Rewards);
    }

    public void Release(string poolID)
    {
        ResetVariables();

        GenericPool.ReleaseItem(GetType(), this, poolID);
    }

    private void UpdateVariables()
    {
        missionNameText.text = currentMission.MissionName;
        descriptionText.text = currentMission.Description;

        CreateRewardInfos();
        
        UpdateItemState();
    }

    private void ResetVariables()
    {
        currentMission = null;

        missionNameText.text = string.Empty;
        descriptionText.text = string.Empty;
        progressText.text = string.Empty;

        ReleaseRewardInfos();
    }

    private void UpgradeProgressState()
    {
        progressText.text = currentMission.GetProgressText();

        if(currentMission.State == DailyMission.MissionState.Active)
        {
            progressIconImage.sprite = activeMissionProgressSprite;
            progressIconContainerImage.sprite = activeMissionContainerSprite;
        }
        else
        {
            progressIconImage.sprite = completedMissionProgressSprite;
            progressIconContainerImage.sprite = completedMissionContainerSprite;
        }
    }

    public void UpdateItemState()
    {
        goToMissionButton.gameObject.SetActive(currentMission.State == DailyMission.MissionState.Active);
        collectRewardButton.gameObject.SetActive(currentMission.State == DailyMission.MissionState.CanBeCompleted);
        rewardCollectedText.gameObject.SetActive(currentMission.State == DailyMission.MissionState.Completed);

        UpgradeProgressState();

        if (currentMission.State != DailyMission.MissionState.Completed)
        {
            SetContainerColor(currentMission.DailyMissionSO.ContainerColor);
        }
        else
        {
            SetContainerColor(completedMissionContainerColor);
        }
    }

    private void SetContainerColor(DailyMissionContainerColorSO containerColor)
    {
        missionNameText.color = containerColor.MissionNameAndProgressTextColor;
        progressText.color = containerColor.MissionNameAndProgressTextColor;

        containerImage.color = containerColor.ContainerColor;
        containerOutlineImage.color = containerColor.ContainerOutlineColor;
    }

    private void CreateRewardInfos()
    {
        ReleaseRewardInfos();

        foreach (Reward reward in currentMission.Rewards)
        {
            RewardInfo rewardInfo = GenericPool.GetItem<RewardInfo>(DailyMissionsUI.poolRewardInfoID);
            rewardInfo.transform.SetParent(rewardInfosContainer);

            rewardInfo.Setup(reward);
        }
    }

    private void ReleaseRewardInfos()
    {
        RewardInfo[] rewardInfos = rewardInfosContainer.GetComponentsInChildren<RewardInfo>();

        foreach (RewardInfo rewardInfo in rewardInfos)
        {
            rewardInfo.Release(DailyMissionsUI.poolRewardInfoID);
        }
    }
}
