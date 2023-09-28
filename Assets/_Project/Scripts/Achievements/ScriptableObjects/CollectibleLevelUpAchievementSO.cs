using UnityEngine;

[CreateAssetMenu(menuName = "Achievement/CollectibleLevelUp")]
public class CollectibleLevelUpAchievementSO : AchievementSO
{
    public override int GetProgress()
    {
        var collectibles = CollectibleManager.Instance.PlayerCollectibles;
        int collectiblesAtRequiredLevel = collectibles.FindAll(c => c.CurrentLevel >= TargetValue).Count;
        return collectiblesAtRequiredLevel;
    }
}
