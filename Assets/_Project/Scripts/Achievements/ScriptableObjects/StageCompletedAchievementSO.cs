using UnityEngine;

[CreateAssetMenu(menuName = "Achievement/StageCompleted")]
public class StageCompletedAchievementSO : AchievementSO
{
    public override int GetProgress()
    {
        if (PlayerProgress.IsStageCompleted(TargetValue))
        {
            return 1;
        }

        return 0;
    }
}
