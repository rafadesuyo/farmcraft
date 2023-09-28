using UnityEngine;

[CreateAssetMenu(menuName = "Achievement/SingleEvent")]
public class SingleEventAchievementSO : AchievementSO
{
    public override int GetProgress()
    {
        return 1;
    }
}
