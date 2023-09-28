public class RealSleeperAbilityBehaviour : BaseAbilityBehaviour
{
    public override bool CanUseAbility()
    {
        return true;
    }

    public override AbilityId GetAbilityId()
    {
        return AbilityId.RealSleeper;
    }

    public override void UseAbility()
    {
        // TODO: this ability need the QuizSystem refactor for implementation
        // https://ocarinastudios.atlassian.net/browse/DQG-1867
    }
}
