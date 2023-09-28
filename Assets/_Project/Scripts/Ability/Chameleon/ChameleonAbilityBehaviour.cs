using UnityEngine;

public class ChameleonAbilityBehaviour : BaseAbilityBehaviour
{
    private QuizSystem quizSystem;
    private QuizSystemAbilityRestriction quizSystemAbilityRestriction;

    public override bool CanUseAbility()
    {
        return quizSystemAbilityRestriction.IsRestricted() == false;
    }

    public override AbilityId GetAbilityId()
    {
        return AbilityId.Chameleon;
    }

    public override void OnInitialize()
    {
        base.OnInitialize();

        quizSystem = StageSystemLocator.GetSystem<QuizSystem>();
        quizSystemAbilityRestriction = new QuizSystemAbilityRestriction(QuizSystemAbilityRestriction.QuizResctictionType.UseOnlyWaitingForAnswer, quizSystem);
        quizSystemAbilityRestriction.OnRestrictionChange += QuizSystemAbilityRestriction_OnRestrictionChange;

        UpdateAbility();
    }

    private void QuizSystemAbilityRestriction_OnRestrictionChange(bool restriction)
    {
        UpdateAbility();
    }

    public override void UseAbility()
    {
        quizSystem.SetupAndShowNextQuestion();
        ConsumeUsePerStage();
    }
}