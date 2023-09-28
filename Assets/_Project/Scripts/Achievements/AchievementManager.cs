using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : LocalSingleton<AchievementManager>
{
    [SerializeField] private AchievementSO[] achievementsData = null;
    private List<Achievement> achievements = new List<Achievement>();
    private List<AchievementProgress> achievementsProgressess = new List<AchievementProgress>();

    public List<Achievement> Achievements => achievements;
    public List<AchievementProgress> AchievementProgresses => achievementsProgressess;

    private void OnEnable()
    {
        SetupAchievements();
    }

    public void SetupAchievements()
    {
        achievements.Clear();
        achievementsProgressess.Clear();

        achievementsProgressess = PlayerProgress.SaveState.playerInfo.achievementProgresses;
        BuildAchievements();
    }

    private void BuildAchievements()
    {
        foreach (AchievementSO achievement in achievementsData)
        {
            achievements.Add(new Achievement(achievement, GetProgressByAchievementId(achievement.Id)));
        }
    }

    private AchievementProgress GetProgressByAchievementId(string id)
    {
        AchievementProgress progress = achievementsProgressess.Find(progress => progress.id == id);

        if (progress == null)
        {
            progress = new AchievementProgress(id);
            achievementsProgressess.Add(progress);
        }

        return progress;
    }
}
