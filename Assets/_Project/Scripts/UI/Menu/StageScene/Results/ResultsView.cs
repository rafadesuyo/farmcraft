using UnityEngine;
using TMPro;
using DreamQuiz.Player;
using UnityEngine.UI;
using DreamQuiz;

public class ResultsView : MenuView
{
    public override Menu Type => Menu.StageResult;

    //Title
    [SerializeField] private TextMeshProUGUI titleTxt = null;
    [SerializeField] private TextMeshProUGUI stageMessageTxt = null;

    //Texts that will need to be changed for translation purposes
    private string congratulationsText = "CONGRATULATIONS!";
    private string ohNoText = "OH NO!";
    private string stageText = "STAGE";
    private string completedText = "COMPLETED";
    private string notCompletedText = "NOT COMPLETED";

    //Background Image
    [SerializeField] private Image backgroundImage = null;
    [SerializeField] private Sprite backgroundWin = null;
    [SerializeField] private Sprite backgroundLose = null;

    //Dreamer Image
    [SerializeField] private Image dreamerImage = null;
    [SerializeField] private Sprite dreamerLose = null;
    [SerializeField] private Sprite dreamerWin = null;

    //Reward Image
    [SerializeField] private Image rewardImage = null;
    [SerializeField] private Sprite rewardWin = null;
    [SerializeField] private Sprite rewardLose = null;

    //Reward Image Container
    [SerializeField] private Image rewardContainer = null;

    //System
    private TimeTrackingSystem timeTrackingSystem;
    [SerializeField] private TextMeshProUGUI stageTimer = null;

    //Botton buttom
    [SerializeField] private GameObject playAgainButton = null;

    //Lose Panel
    [SerializeField] private GameObject improvePerformancePanel = null;
    [SerializeField] private GameObject noRewardText = null;

    [Header("Stage Status")]
    [SerializeField] private RewardCalculationItem totalWovesCollected = null;
    [SerializeField] private RewardCalculationItem totalSheepsCollected = null;
    [SerializeField] private RewardCalculationItem correctAnswersItem = null;
    [SerializeField] private RewardCalculationItem totalGold = null;
    [SerializeField] private EndNodeCalculationItem endNodeItem = null;
    [SerializeField] private ShardCalculationItem stageShardsItem = null;

    protected override void Setup(MenuSetupOptions setupOptions)
    {
        PopulateInfos();
    }
    private void PopulateInfos()
    {
        if (PlayerManager.CurrentPlayerInstance.PlayerStageGoal.State == PlayerStageGoal.PlayerStageGoalState.Win)
        {
            SetupRewardItemsWin();
            titleTxt.text = congratulationsText;
            dreamerImage.sprite = dreamerWin;
            backgroundImage.sprite = backgroundWin;
            stageMessageTxt.text = $"{stageText}<br><color=white>{StageManager.Instance.StageId}</color><br>{completedText}!";
            TextFormatter.ChangeTextColor(HexColors.Maize, titleTxt);
        }

        else
        {
            SetupRewardItemsLose();
            EnableLoseScreen();
            titleTxt.text = ohNoText;
            dreamerImage.sprite = dreamerLose;
            backgroundImage.sprite = backgroundLose;
            TextFormatter.ChangeTextColor(HexColors.PaleViolet, stageMessageTxt);
            stageMessageTxt.text = $"{stageText}<br><color=white>{StageManager.Instance.StageId}</color><br>{notCompletedText}!";
            TextFormatter.ChangeTextColor(HexColors.SunsetOrange, titleTxt);
        }

        SetupStageTime();
    }

    private void SetupStageTime()
    {
        timeTrackingSystem = StageSystemLocator.GetSystem<TimeTrackingSystem>();

        int systemTime = timeTrackingSystem.ElapsedTime;

        stageTimer.text = TextFormatter.TimeToMMSS(systemTime);
    }

    private void SetupRewardItemsWin()
    {
        var reachEndGoalProgress = PlayerManager.CurrentPlayerInstance.PlayerStageGoal.StageGoalProgressList.Find(p => p.StageGoal.Requisite == StageGoal.StageGoalRequisite.ReachEndNode);

        int gold = Mathf.RoundToInt(reachEndGoalProgress.StageGoal.GoalReward.TotalReward * reachEndGoalProgress.ProgressRatio);

        endNodeItem.Setup();

        totalGold.SetupWin(gold + PlayerManager.CurrentPlayerInstance.PlayerStageData.TotalStageScore);
        totalWovesCollected.SetupWin(PlayerManager.CurrentPlayerInstance.PlayerStageData.WolvesCollected);
        totalSheepsCollected.SetupWin(PlayerManager.CurrentPlayerInstance.PlayerStageData.SheepsCollected);
        correctAnswersItem.SetupWin(PlayerManager.CurrentPlayerInstance.PlayerStageData.TotalCorrectAnswers);

        //Shards
        var shardGoalProgressList = PlayerManager.CurrentPlayerInstance.PlayerStageGoal.StageGoalProgressList.FindAll(p => p.StageGoal.GoalReward.CurrencyType == CurrencyType.Shard);

        int shardTotal = 0;

        foreach (var shardGoal in shardGoalProgressList)
        {
            shardTotal += Mathf.RoundToInt(shardGoal.StageGoal.GoalReward.TotalReward * shardGoal.ProgressRatio);
        }

        stageShardsItem.gameObject.SetActive(shardTotal > 0);

        if (shardTotal > 1)
        {
            stageShardsItem.SetupWin(shardTotal);
        }
    }

    private void SetupRewardItemsLose()
    {
        rewardImage.sprite = rewardLose;

        endNodeItem.Setup();
        correctAnswersItem.SetupLose(PlayerManager.CurrentPlayerInstance.PlayerStageData.TotalCorrectAnswers);
        totalWovesCollected.SetupLose(PlayerManager.CurrentPlayerInstance.PlayerStageData.WolvesCollected);
        totalSheepsCollected.SetupLose(PlayerManager.CurrentPlayerInstance.PlayerStageData.SheepsCollected);

        totalGold.DisableItem();
        stageShardsItem.DisableItem();

        noRewardText.SetActive(true);
        playAgainButton.SetActive(true);

        TextFormatter.ChangeTextColor(HexColors.SunsetOrange, stageTimer);
    }

    public void EnableLoseScreen()
    {
        ImageFormatter.ChangeImageColor(rewardContainer, HexColors.SunsetOrange);
        ImageFormatter.ChangeImageOpacity(rewardContainer, 0.5f);
    }

    public void ReturnToHomeMenu()
    {
        StageLoadManager.Instance.ReturnToWorldMap();
    }

    public void ReplayStage()
    {
        //TODO:https://ocarinastudios.atlassian.net/browse/DQG-2079
        //StageLoadManager.Instance.LoadStage(StageLoadManager.Instance.CurrentStageInfo);
    }

    public void OpenStore()
    {
        //TODO:https://ocarinastudios.atlassian.net/browse/DQG-2083
        //CanvasManager.Instance.OpenMenu(Menu.Store, null, true);
    }
}
