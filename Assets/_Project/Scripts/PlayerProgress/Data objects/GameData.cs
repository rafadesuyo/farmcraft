using System.Collections.Generic;
using System;
using Newtonsoft.Json;

[Serializable]
public class StageBeatInfo
{
    public int id;
    public int score;
    public bool isStageCompleted = false;
}

[Serializable]
public class StoreInfo
{
    public int id;
    public int purchaseCount;
    public int randomQuantity;
}

[Serializable]
public class GameData
{
    public bool hasSaveData = false;
    public PlayerInfo playerInfo = new PlayerInfo();
    public QuizData quizData = new QuizData();

    public DailyMissionsManagerSave dailyMissionsManagerSave = new DailyMissionsManagerSave();

    public DailyRewardsManagerSave dailyRewardsManagerSave = new DailyRewardsManagerSave();

    public List<StageBeatInfo> stagesBeat = new List<StageBeatInfo>();
    public List<StoreInfo> storeInfo = new List<StoreInfo>();

    public int totalGold = 0;
    public int totalOneekoin = 0;
    public int totalLullabyNote = 0;
    public int totalHearts = 1;

    public int totalScore = 0;

    public int totalXP = 0;
    public int currentPlayerLevel = 1;

    public int totalAnsweredQuestions = 0;

    // This will work as "time when finished the app" as well, since it will change whenever something is saved.
    public DateTime lastSaveDate = DateTime.Now;

    public DateTime lastGameplayTimeRegistryDate = DateTime.Now;
    public TimeSpan totalTimePlayed = TimeSpan.Zero;

    [JsonIgnoreAttribute]
    public List<StageBeatInfo> StagesCompleted
    {
        get
        {
            return stagesBeat.FindAll(stage => stage.isStageCompleted);
        }
    }

    //TODO: Add analytics variables if necessary
}
