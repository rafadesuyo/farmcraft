using DreamQuiz;
using System.Linq;

public class DestroyerAbilityBehaviour : BaseTargetableAbilityBehaviour<NodeBase>
{
    private QuizSystemAbilityRestriction quizSystemAbilityRestriction;

    public override bool CanUseAbility()
    {
        return quizSystemAbilityRestriction.IsRestricted() == false;
    }

    public override AbilityId GetAbilityId()
    {
        return AbilityId.Destroyer;
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
        var pawns = node.GetNonPlayerPawnsInNode();
        var questionPawn = pawns.First(p => p is QuestionPawn);

        if (questionPawn != null)
        {
            validTarget = true;
            questionPawn.RemovePawnFromBoard();
            ConsumeUsePerStage();
        }

        return validTarget;
    }
}
