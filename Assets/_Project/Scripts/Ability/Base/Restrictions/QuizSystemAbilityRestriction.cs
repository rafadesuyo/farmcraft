using System;

public class QuizSystemAbilityRestriction : IAbilityRestriction
{
    public enum QuizResctictionType { UseOnlyOutsideQuiz, UseOnlyWaitingForAnswer }

    private QuizResctictionType quizRestrictionType;
    private bool restricted = false;

    public event Action<bool> OnRestrictionChange;

    public QuizSystemAbilityRestriction(QuizResctictionType quizResctictionType, QuizSystem quizSystem)
    {
        this.quizRestrictionType = quizResctictionType;
        quizSystem.OnQuizStateChange += QuizSystem_OnQuizStateChange;
    }

    private void QuizSystem_OnQuizStateChange(QuizState quizState)
    {
        restricted = false;

        switch (quizRestrictionType)
        {
            case QuizResctictionType.UseOnlyOutsideQuiz:

                if (quizState != QuizState.None)
                {
                    restricted = true;
                }

                break;
            case QuizResctictionType.UseOnlyWaitingForAnswer:

                if (quizState != QuizState.WaitingForAnswer)
                {
                    restricted = true;
                }

                break;
        }

        OnRestrictionChange?.Invoke(restricted);
    }

    public bool IsRestricted()
    {
        return restricted;
    }
}