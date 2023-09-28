using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using UnityEngine;
using DreamQuiz.Player;

public static class PlayerProgress
{
    // FIXME: Add a custom "unlock requirement" in the collectible object
    // https://ocarinastudios.atlassian.net/browse/DQG-897?atlOrigin=eyJpIjoiYmNiNzc4YTk2NmI0NDNkNGFhZjNjNWU5Mjg5NThhNzEiLCJwIjoiaiJ9
    private static readonly int craigUnlockLevel = 2;
    private static readonly int caremeUnlockLevel = 4;
    private static readonly int charlotteUnlockLevel = 13;
    public static readonly int timeToResetStore = 3000;

    private static PlayerLevelXPDatabaseSO playerLevelXPDatabase = null;
    private static GameData gameData;

    private const string playerLevelXPDataBase_Path = "ScriptableObjects/PlayerLevelXPDatabase";

    public static GameData SaveState
    {
        get
        {
            return gameData;
        }
    }

    public static DateTime LastSaveTime
    {
        get
        {
            return gameData.lastSaveDate;
        }
    }

    public static int GameplayHours
    {
        get
        {
            return (int)TotalPlayTime.TotalHours;
        }
    }

    public static int MaxSleepingTime
    {
        get
        {
            return gameData.playerInfo.maxSleepingTime;
        }
    }

    public static int MaxDreamEnergy
    {
        get
        {
            return gameData.playerInfo.maxDreamEnergy;
        }
    }

    public static int TotalGold
    {
        get => gameData.totalGold;
        set
        {
            gameData.totalGold = value;
            EventsManager.Publish(EventsManager.onGoldChange);
        }
    }

    public static int TotalOneekoin
    {
        get => gameData.totalOneekoin;
        set
        {
            gameData.totalOneekoin = value;
            EventsManager.Publish(EventsManager.onOneekoinChange);
        }
    }

    public static int TotalLullabyNote
    {
        get => gameData.totalLullabyNote;
        set
        {
            gameData.totalLullabyNote = value;
            EventsManager.Publish(EventsManager.onLullabyNoteChange);
        }
    }

    public static List<CollectibleSaveData> CollectiblesSaved
    {
        get
        {
            return gameData.playerInfo.collectiblesWithProgress;
        }
    }

    public static string Username
    {
        get
        {
            return gameData.playerInfo.username;
        }
    }

    public static TimeSpan TotalPlayTime
    {
        get
        {
            return gameData.totalTimePlayed;
        }
    }

    public static int TotalScore
    {
        get
        {
            return gameData.totalScore;
        }
    }

    public static int TotalAnsweredQuestions
    {
        get
        {
            return gameData.totalAnsweredQuestions;
        }
    }

    public static DailyMissionsManagerSave DailyMissionsManagerSave
    {
        get
        {
            return gameData.dailyMissionsManagerSave;
        }
    }

    public static DailyRewardsManagerSave DailyRewardsManagerSave
    {
        get
        {
            return gameData.dailyRewardsManagerSave;
        }
    }

    public static QuizLanguageType QuizLanguage
    {
        get
        {
            if (gameData == null || gameData.playerInfo == null)
            {
                return QuizLanguageType.en;
            }

            return gameData.playerInfo.quizLanguage;
        }
        set
        {
            gameData.playerInfo.quizLanguage = value;
        }
    }

    public static List<CollectibleType> CurrentTeam => gameData.playerInfo.currentTeam;

    public static void PrepareForSave()
    {
        gameData.hasSaveData = true;
        gameData.lastSaveDate = DateTime.Now;
        gameData.playerInfo.currentHeartCount = HeartManager.Instance.CurrentHeartCount;
        gameData.playerInfo.achievementProgresses = AchievementManager.Instance.AchievementProgresses;
        gameData.dailyMissionsManagerSave = new DailyMissionsManagerSave(DailyMissionsManager.Instance);
        gameData.dailyRewardsManagerSave = new DailyRewardsManagerSave(DailyRewardsManager.Instance);

        UpdateGameplayTime();

        List<CollectibleSaveData> newCollectibleData = new List<CollectibleSaveData>();

        foreach (Collectible collectible in CollectibleManager.Instance.PlayerCollectibles)
        {
            CollectibleProgress collectibleProgress = new CollectibleProgress(collectible.CurrentLevel, collectible.CurrentShards, collectible.IsUnlocked);
            newCollectibleData.Add(new CollectibleSaveData(collectible.Data.Type, collectibleProgress));
        }

        gameData.playerInfo.collectiblesWithProgress = newCollectibleData;
    }

    public static void EvaluateLoad(GameData data)
    {
        gameData = data;

        if (!data.hasSaveData)
        {
            SetupPlayerDefaultValues();
        }

        gameData.lastGameplayTimeRegistryDate = DateTime.Now;
        CanResetStoreInfo();
    }

    public static void AddAnsweredQuestions(int count)
    {
        gameData.totalAnsweredQuestions += count;
    }

    public static int StagesBeatCount()
    {
        return gameData.stagesBeat.Count;
    }

    public static int StagesCompletedCount()
    {
        return gameData.StagesCompleted.Count;
    }

    public static bool CanPlayStage(int id)
    {
        return IsStageUnlocked(id);
    }

    public static int GetOverallRanking()
    {
        int totalScore = 0;

        foreach (var stageBeat in gameData.stagesBeat)
        {
            totalScore += stageBeat.score;
        }

        return totalScore;
    }

    public static void OnCompleteStage(int id, bool walkedIntoAllNodes, int stageScore)
    {
        UpdateStageBeat(id, walkedIntoAllNodes, stageScore);
        if (IsStageCompleted(id))
        {
            return;
        }

        // Give Heart to player only if it is the first time beating the stage.
        if (!IsStageBeat(id))
        {
            HeartManager.Instance.AddHearts(1);
        }

        // Give stage collectibles
        if (id == caremeUnlockLevel)
        {
            var collectibleData = CollectibleManager.Instance.GetCollectibleDataByType(CollectibleType.Careme);
            CollectibleManager.Instance.GiveShardsTo(CollectibleType.Careme, collectibleData.ShardsRequiredPerLevel[0]);
        }
        else if (id == craigUnlockLevel)
        {
            var collectibleData = CollectibleManager.Instance.GetCollectibleDataByType(CollectibleType.CraigNolan);
            CollectibleManager.Instance.GiveShardsTo(CollectibleType.CraigNolan, collectibleData.ShardsRequiredPerLevel[0]);
        }
        else if (id == charlotteUnlockLevel)
        {
            var collectibleData = CollectibleManager.Instance.GetCollectibleDataByType(CollectibleType.Charlotte);
            CollectibleManager.Instance.GiveShardsTo(CollectibleType.Charlotte, collectibleData.ShardsRequiredPerLevel[0]);
        }

        EventsManager.Publish(EventsManager.onBeatStage);

        if (walkedIntoAllNodes)
        {
            EventsManager.Publish(EventsManager.onCompleteStage);
        }

        //  AUX. THIS WILL EXIST ONLY IN THE MVP, TO HELP UNLOCK A SPECIFIC ACHIEVEMENT
        int firstFarmStageIndex = 7;
        int lastFarmStageIndex = 12;

        for (int i = firstFarmStageIndex; i <= lastFarmStageIndex; i++)
        {
            if (!IsStageBeat(i))
            {
                return;
            }
        }

        EventsManager.Publish(EventsManager.onBeatAllFarmingStages);
    }

    public static void AddCorrectAnswersToCategory(QuizCategory category)
    {
        gameData.quizData.IncreaseCategoryCorrectAnswers(category);
    }

    public static int GetCorrectAnswersCountByCategory(QuizCategory category)
    {
        return gameData.quizData.GetCorrectAnswersCountByCategory(category);
    }

    public static int GetCorrectAnswersCount()
    {
        return gameData.quizData.GetCorrectAnswersCount();
    }

    public static void UpdateUsername(string newUsername)
    {
        gameData.playerInfo.username = newUsername;
        LeaderboardManager.Instance.CreateUserScoreEntry();
    }

    public static bool IsStageCompleted(int stageId)
    {
        return gameData.stagesBeat.Find(stage => stage.id == stageId && stage.isStageCompleted) != null;
    }

    public static bool IsStageBeat(int stageId)
    {
        return gameData.stagesBeat.Find(stage => stage.id == stageId) != null;
    }

    private static void UpdateStageBeat(int id, bool completed, int score)
    {
        StageBeatInfo stageBeatInfo = GetStageBeatInfoById(id);

        if (stageBeatInfo == null)
        {
            stageBeatInfo = new StageBeatInfo();
            gameData.stagesBeat.Add(stageBeatInfo);
            stageBeatInfo.id = id;
        }

        stageBeatInfo.isStageCompleted = completed;

        if (score > stageBeatInfo.score)
        {
            stageBeatInfo.score = score;
            LeaderboardManager.Instance.UpdateUserScore(PlayerProgress.GetOverallRanking());
        }
    }

    private static StageBeatInfo GetStageBeatInfoById(int stageId)
    {
        return gameData.stagesBeat.Find(stage => stage.id == stageId);
    }

    private static bool IsStageUnlocked(int stageId)
    {
        if (stageId == 0)
        {
            return true;
        }
        else
        {
            return IsStageBeat(stageId - 1);
        }
    }

    private static void SetupPlayerDefaultValues()
    {
#if UNITY_EDITOR
        // Only for progress internal testing purpouse. This way anyone can test any level of any collectible.
        TotalGold = 50000;
        TotalOneekoin = 10000;
        TotalLullabyNote = 20000;

#endif
        var playerBaseStats = UnityEngine.Resources.Load("ScriptableObjects/PlayerStats") as PlayerStatsSO;

        gameData.playerInfo.maxSleepingTime = playerBaseStats.BaseSleepingTime;
        gameData.playerInfo.maxDreamEnergy = playerBaseStats.BaseDreamEnergy;

        UpdateUsername($"User{Random.Range(0, 10001)}");
    }

    private static void UpdateGameplayTime()
    {
        gameData.totalTimePlayed += DateTime.Now - gameData.lastGameplayTimeRegistryDate;
        gameData.lastGameplayTimeRegistryDate = DateTime.Now;
    }

    public static void SavingPurchaseCount(StoreItemSO item, int numberOfPurchases = 1)
    {
        int newPurchaseCount = item.PurchaseCount + numberOfPurchases;

        StoreInfo newItem = new StoreInfo
        {
            id = item.ItemId,
            purchaseCount = newPurchaseCount
        };

        StoreInfo existingItem = gameData.storeInfo.Find(itemInfo => itemInfo.id == item.ItemId);
        if (existingItem != null)
        {
            existingItem.purchaseCount = newPurchaseCount;
        }
        else
        {
            gameData.storeInfo.Add(newItem);
        }
        item.PurchaseCount = newPurchaseCount;
    }

    public static int LoadingPurchaseCount(StoreItemSO item)
    {
        StoreInfo storeInfo = null;

        if (gameData != null && gameData.storeInfo != null)
        {
            storeInfo = gameData.storeInfo.Find(itemInfo => itemInfo.id == item.ItemId);
        }

        if (storeInfo != null)
        {
            return storeInfo.purchaseCount;
        }
        return 0;
    }

    public static void SavingRandomQuantity(StoreItemSO item, int randomQuantity)
    {
        StoreInfo newItem = new StoreInfo
        {
            id = item.ItemId,
            randomQuantity = randomQuantity // Save random quantity
        };

        StoreInfo existingItem = gameData.storeInfo.Find(itemInfo => itemInfo.id == item.ItemId);
        if (existingItem != null)
        {
            existingItem.randomQuantity = randomQuantity; // Update random quantity
        }
        else
        {
            gameData.storeInfo.Add(newItem);
        }
    }
    public static int LoadingRandomQuantity(StoreItemSO item)
    {
        StoreInfo storeInfo = gameData.storeInfo.Find(itemInfo => itemInfo.id == item.ItemId);
        if (storeInfo != null)
        {
            return storeInfo.randomQuantity;
        }
        return 0;
    }

    public static void ResetStoreInfo()
    {
        gameData.storeInfo.Clear();
    }

    public static void CanResetStoreInfo()
    {
        double offlineSeconds = (DateTime.Now - PlayerProgress.LastSaveTime).TotalSeconds;

        for (double remainingTime = offlineSeconds; remainingTime >= timeToResetStore; remainingTime -= timeToResetStore)
        {
            ResetStoreInfo();
        }
    }

    private static void GetPlayerLevelXPDatabase()
    {
        if (playerLevelXPDatabase == null)
        {
            playerLevelXPDatabase = UnityEngine.Resources.Load(playerLevelXPDataBase_Path) as PlayerLevelXPDatabaseSO;
        }
    }

    public static void IncreaseXP(int amount)
    {
        if (amount < 0)
        {
            UnityEngine.Debug.LogWarning("NEGATIVE VALUE ADDED TO IncreaseXP");
        }

        GetPlayerLevelXPDatabase();
        gameData.totalXP += UnityEngine.Mathf.Abs(amount);

        if (playerLevelXPDatabase.CanLevelUp(gameData.currentPlayerLevel, gameData.totalXP))
        {
            gameData.currentPlayerLevel++;
        }
    }

    public static void SetXP(int amount)
    {
        if (amount < 0)
        {
            UnityEngine.Debug.LogWarning("NEGATIVE VALUE SET TO SetXP");
        }

        GetPlayerLevelXPDatabase();
        gameData.totalXP = UnityEngine.Mathf.Abs(amount);
        gameData.currentPlayerLevel = playerLevelXPDatabase.GetLevelByCurrentXP(gameData.totalXP);
    }

    public static PlayerData GetPlayerData()
    {
        var playerPawnPrefab = Resources.Load("Prefabs/Pawns/PlayerPawn", typeof(PlayerPawn)) as PlayerPawn;

        return new PlayerData()
        {
            Id = 0,
            PlayerPawnPrefab = playerPawnPrefab,
            MaxSleepingTime = MaxSleepingTime,
            Team = CollectibleManager.Instance.GetCurrentCollectibleTeam()
        };
    }
}
