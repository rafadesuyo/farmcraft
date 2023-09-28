using UnityEngine;

public class QuickNapAbilityBehaviour : BaseAbilityBehaviour
{
    int sleepingTimeRecoveryAmount = 0;

    public override bool CanUseAbility()
    {
        return true; 
    }

    public override AbilityId GetAbilityId()
    {
        return AbilityId.QuickNap;
    }

    public override void UseAbility()
    {
        PlayerStageData.SleepingTime.Add(sleepingTimeRecoveryAmount);
        ConsumeUsePerStage();
    }

    protected override void ApplyEnhancements(AbilityEnhancementSO abilityEnhancementSO)
    {
        var quickNapAbilityEnhancementSO = GetEnhancementFromBase<QuickNapAbilityEnhancementSO>(abilityEnhancementSO);
        sleepingTimeRecoveryAmount = quickNapAbilityEnhancementSO.SleepingTimeRecoveryAmount;
    }
}
