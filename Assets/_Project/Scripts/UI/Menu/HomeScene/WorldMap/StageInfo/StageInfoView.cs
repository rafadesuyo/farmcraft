using DreamQuiz;
using DreamQuiz.Player;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageInfoView : MenuView
{
    //TODO:https://ocarinastudios.atlassian.net/browse/DQG-2070

    [SerializeField] private UnityEngine.UI.Button playBtn = null;

    [SerializeField] private TextMeshProUGUI nodeAmount = null;
    [SerializeField] private TextMeshProUGUI stageNumber = null;

    [SerializeField] private GameObject extraGoalPrefab = null;
    [SerializeField] private GameObject stageInfoPrefab = null;

    [SerializeField] private RectTransform mainGoalContainer = null;
    [SerializeField] private RectTransform extraGoalsContainer = null;
    [SerializeField] private RectTransform rewardsContainer = null;
    [SerializeField] private RectTransform categoryContainer = null;

    [Space(10)]
    [SerializeField] private string textToReplaceWithValue = "{value}";

    private StageInfoSO stageData;

    public override Menu Type => Menu.StageInfo;

    public void PlayStage()
    {
        LoadStage();
    }

    protected override void Setup(MenuSetupOptions setupOptions)
    {
        GenericPool.CreatePool<StageInfoItem>(stageInfoPrefab, rewardsContainer);
        GenericPool.CreatePool<ExtraGoalsInfoItem>(extraGoalPrefab, extraGoalsContainer);
        GenericPool.CreatePool<StageInfoItem>(stageInfoPrefab, categoryContainer);
        stageData = setupOptions.stageData;

        PopulateInfos(stageData.Id, stageData.NodeCountInStage);
        PopulateGoals(stageData.Goals);

        PopulateQuestionCategory(stageData.CategoriesInStage);

        var rewards = new List<GoalReward>();

        foreach (var goal in stageData.Goals)
        {
            rewards.Add(goal.GoalReward);
        }

        PopulateRewards(rewards.ToArray());

        playBtn.interactable = HeartManager.Instance.CurrentHeartCount > 0;
    }

    private void PopulateInfos(int stageNumber, int nodeAmount)
    {
        this.stageNumber.text = stageNumber.ToString();
        this.nodeAmount.text = nodeAmount.ToString();
    }

    private void PopulateGoals(StageGoal[] stageGoals)
    {
        if (stageGoals == null)
        {
            return;
        }

        mainGoalContainer.gameObject.SetActive(false);
        mainGoalContainer.gameObject.SetActive(true);

        foreach (StageGoal goal in stageGoals)
        {
            if (goal.MainGoal)
            {
                StageGoalDataSO.StageGoalDataPair stageGoalInfo = ProjectAssetsDatabase.Instance.GetStageGoalDataByRequisite(goal.Requisite);

                StageInfoItem stageInfo = GenericPool.GetItem<StageInfoItem>();
                stageInfo.DisableIcon();
                stageInfo.EnableText();
                stageInfo.transform.SetParent(mainGoalContainer);
                stageInfo.Setup(stageGoalInfo.GoalIcon, stageGoalInfo.GoalTextStageInfoView.Replace(textToReplaceWithValue, goal.TargetValue.ToString()));
            }
            else
            {
                StageGoalDataSO.StageGoalDataPair stageGoalInfo = ProjectAssetsDatabase.Instance.GetStageGoalDataByRequisite(goal.Requisite);
                ExtraGoalsInfoItem extraGoalsInfo = GenericPool.GetItem<ExtraGoalsInfoItem>();
                extraGoalsInfo.transform.SetParent(extraGoalsContainer);
                extraGoalsInfo.Setup(stageGoalInfo.ScreenGoalIcon, stageGoalInfo.GoalTextStageInfoView.Replace(textToReplaceWithValue, goal.TargetValue.ToString()));
            }
        }
    }

    private void PopulateRewards(GoalReward[] stageRewards)
    {
        if (stageRewards == null)
        {
            return;
        }

        rewardsContainer.gameObject.SetActive(false);
        rewardsContainer.gameObject.SetActive(true);

        Dictionary<CurrencyType, bool> currencyDict = new Dictionary<CurrencyType, bool>();

        foreach (GoalReward reward in stageRewards)
        {
            if (!currencyDict.ContainsKey(reward.CurrencyType))
            {
                currencyDict.Add(reward.CurrencyType, true);

                StageInfoItem stageInfo = GenericPool.GetItem<StageInfoItem>();
                stageInfo.DisableText();
                stageInfo.EnableIcon();
                stageInfo.transform.SetParent(rewardsContainer);
                stageInfo.Setup(ProjectAssetsDatabase.Instance.GetStageRewardIcon(reward.CurrencyType,
                    reward.CollectibleType), $"{reward.ValueAsDescription}");
            }
        }
    }

    private void PopulateQuestionCategory(QuizCategory[] categories)
    {
        if(categories == null)
        {
            Debug.LogError($"This Stage:{stageData.Id} is missing the categories, try adding them by saving the stage");
            return;
        }

        categoryContainer.gameObject.SetActive(false);
        categoryContainer.gameObject.SetActive(true);

        Dictionary<QuizCategory, bool> categoryDict = new Dictionary<QuizCategory, bool>();

        foreach (QuizCategory category in categories)
        {
            if (!categoryDict.ContainsKey(category))
            {
                categoryDict.Add(category, true);

                StageInfoItem categoryInfo = GenericPool.GetItem<StageInfoItem>();
                categoryInfo.DisableText();
                categoryInfo.EnableIcon();
                categoryInfo.transform.SetParent(categoryContainer);

                categoryInfo.Setup(ProjectAssetsDatabase.Instance.GetCategoryIcon(category), null);
            }
        }
    }

    private void LoadStage()
    {
        SoundManager.Instance.StopMusic();
        AudioManager.Instance.Play("Button");

        StoryModeStageInitializer stageInitializer = new StoryModeStageInitializer();
        List<PlayerData> playerDataList = new List<PlayerData>() { PlayerProgress.GetPlayerData() };
        StoryModeInitializerData storyModeInitializerData = new StoryModeInitializerData(stageData, playerDataList);
        stageInitializer.SetupInitializer(storyModeInitializerData);
        StoryModeStageFinalizer stageFinalizer = new StoryModeStageFinalizer();

        StageLoadManager.Instance.LoadStage(stageInitializer, stageFinalizer);
    }
}