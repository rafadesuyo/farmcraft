using DreamQuiz;

public class SleepyTouchAbilityBehaviour : BaseTargetableAbilityBehaviour<NodeBase>
{
    private int goldCount;
    private QuizSystemAbilityRestriction quizSystemAbilityRestriction;

    public override bool CanUseAbility()
    {
        return quizSystemAbilityRestriction.IsRestricted() == false;
    }

    public override AbilityId GetAbilityId()
    {
        return AbilityId.SleepyTouch;
    }

    public override void OnInitialize()
    {
        base.OnInitialize();

        var quizSystem = StageSystemLocator.GetSystem<QuizSystem>();
        quizSystemAbilityRestriction = new QuizSystemAbilityRestriction(QuizSystemAbilityRestriction.QuizResctictionType.UseOnlyWaitingForAnswer, quizSystem);
        quizSystemAbilityRestriction.OnRestrictionChange += QuizSystemAbilityRestriction_OnRestrictionChange;

        UpdateAbility();
    }

    private void QuizSystemAbilityRestriction_OnRestrictionChange(bool restriction)
    {
        UpdateAbility();
    }

    protected override bool OnTargetAcquired(NodeBase node)
    {
        bool validTarget = false;

        if (node.OnlyPlayerCanStay == false)
        {
            var nodeSystem = StageSystemLocator.GetSystem<NodeSystem>();

            if (nodeSystem.SpawnPawnToNode(node, NodeSystem.PawnType.Gold))
            {
                validTarget = true;
            }
        }

        return validTarget;
    }

    protected override void ApplyEnhancements(AbilityEnhancementSO abilityEnhancementSO)
    {
        base.ApplyEnhancements(abilityEnhancementSO);

        var sleepyTouchAbilityEnhancementSO = GetEnhancementFromBase<SleepyTouchAbilityEnhancementSO>(abilityEnhancementSO);
        goldCount = sleepyTouchAbilityEnhancementSO.GoldAmount;
    }
}
