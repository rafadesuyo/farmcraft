using UnityEngine;

[CreateAssetMenu(fileName = "RealSleeperAbilityEnhancementLvl0", menuName = "Ability/Enhancement/Real Sleeper Ability")]
public class RealSleeperAbilityEnhancementSO : AbilityEnhancementSO
{
    [SerializeField] float sleepingTimeUsageReduced = 0.1f;

    public float SleepingTimeUsageReduced => sleepingTimeUsageReduced;

    public override string GetModifierText()
    {
        return $"-{Mathf.FloorToInt(sleepingTimeUsageReduced * 100)}%";
    }
}
