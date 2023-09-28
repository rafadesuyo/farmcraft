using System;

[Serializable]
public class AchievementProgress
{
    public string id = "";
    public int value = 0;
    public bool rewardCollected = false;

    public AchievementProgress(string achievementId)
    {
        id = achievementId;
    }
}
