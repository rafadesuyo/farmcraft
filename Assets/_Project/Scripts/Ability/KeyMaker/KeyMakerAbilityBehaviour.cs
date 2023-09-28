using DreamQuiz;

public class KeyMakerAbilityBehaviour : BaseTargetableAbilityBehaviour<NodeBase>
{
    private QuizSystemAbilityRestriction quizSystemAbilityRestriction;

    public override bool CanUseAbility()
    {
        return quizSystemAbilityRestriction.IsRestricted() == false;
    }

    public override AbilityId GetAbilityId()
    {
        return AbilityId.KeyMaker;
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
        var doorNode = node as DoorNodeBase;

        if (doorNode != null)
        {
            validTarget = true;
            doorNode.OpenDoor();
        }

        return validTarget;
    }
}
