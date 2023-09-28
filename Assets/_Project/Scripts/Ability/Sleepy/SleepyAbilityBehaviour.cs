public class SleepyAbilityBehaviour : BaseAbilityBehaviour
{
    private int sleepingTimeToAdd;

    public override bool CanUseAbility()
    {
        return true;
    }

    public override AbilityId GetAbilityId()
    {
        return AbilityId.Sleepy;
    }

    protected override void ApplyEnhancements(AbilityEnhancementSO abilityEnhancementSO)
    {
        var sleepyAbilityEnhancementSO = GetEnhancementFromBase<SleepyAbilityEnhancementSO>(abilityEnhancementSO);
        sleepingTimeToAdd = sleepyAbilityEnhancementSO.SleepingTimeToAdd;
    }

    public override void UseAbility()
    {
        PlayerStageData.SleepingTime.Add(sleepingTimeToAdd);
        ConsumeUsePerStage();
    }
}
