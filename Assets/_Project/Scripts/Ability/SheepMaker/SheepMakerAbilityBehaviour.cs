using DreamQuiz;

public class SheepMakerAbilityBehaviour : BaseTargetableAbilityBehaviour<NodeBase>
{
    private QuizSystemAbilityRestriction quizSystemAbilityRestriction;

    public override bool CanUseAbility()
    {
        return quizSystemAbilityRestriction.IsRestricted() == false;
    }

    public override AbilityId GetAbilityId()
    {
        return AbilityId.SheepMaker;
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

            if (nodeSystem.SpawnPawnToNode(node, NodeSystem.PawnType.Sheep))
            {
                validTarget = true;
            }
        }

        return validTarget;
    }
}
