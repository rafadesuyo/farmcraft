using System.Collections.Generic;

public enum QuizLanguageType
{
    en,
    pt
}

// This will be called PlayerData after refactoring the PlayerData.cs
[System.Serializable]
public class PlayerInfo
{
    public string username = "test";
    public int maxSleepingTime = 0;
    public int maxDreamEnergy = 0;
    public int currentHeartCount = 0;
    public int currentPillowCount = 0;
    public int currentLullabyNote = 0;
    public QuizLanguageType quizLanguage = QuizLanguageType.en;

    public List<AchievementProgress> achievementProgresses = new List<AchievementProgress>();
    public List<CollectibleSaveData> collectiblesWithProgress = new List<CollectibleSaveData>();
    public List<CollectibleType> currentTeam = new List<CollectibleType>();
}
