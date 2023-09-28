using UnityEngine;
using System.Linq;

[System.Serializable]
public class DivisionNumber
{
    [SerializeField] private int[] incorrectAnswerQuotientPlusMinusPossitiblities = null;
    [SerializeField] private int[] incorrectAnswerRemainderPlusMinusPossitiblities = null;

    public int[] IncorrectAnswerQuotientPlusMinusPossitiblities => incorrectAnswerQuotientPlusMinusPossitiblities;
    public int[] IncorrectAnswerRemainderPlusMinusPossitiblities => incorrectAnswerRemainderPlusMinusPossitiblities;
}

[CreateAssetMenu(menuName = "Math/Division/Easy")]
public class DivisionExpressionSO : MathExpressionSO
{
    [SerializeField] private DivisionNumber[] divisionNumbers = null;
    [SerializeField] private int finalNumbersMultiplier = 1;

    protected int[] incorrectAnswersQuotientsPlusMinus = new int[3];
    protected int[] incorrectAnswersRemaindersPlusMinus = new int[3];

    protected override string FirstIncorrectAnswer
    {
        get
        {
            return GetAnswerText(incorrectAnswersQuotientsPlusMinus[0], incorrectAnswersRemaindersPlusMinus[0]);
        }
    }

    protected override string SecondIncorrectAnswer
    {
        get
        {
            return GetAnswerText(incorrectAnswersQuotientsPlusMinus[1], incorrectAnswersRemaindersPlusMinus[1]);
        }
    }

    protected override string ThirdIncorrectAnswer
    {
        get
        {
            return GetAnswerText(incorrectAnswersQuotientsPlusMinus[2], incorrectAnswersRemaindersPlusMinus[2]);
        }
    }

    private int firstNumber = 0;
    private int secondNumber = 0;
    private int quotient = 0;
    private int remainder = 0;

    protected override string QuestionTitle
    {
        get
        {
            return $"{MathQuestionLocalization.GetTranslatedQuestionTitle("What is the quotient and the remainder from ")}{firstNumber} / {secondNumber}?";
        }
    }

    protected override void SetupExpressionNumbers()
    {
        base.SetupExpressionNumbers();
        expressionNumbers = expressionNumbers.OrderByDescending(n => n).ToArray();

        firstNumber = expressionNumbers[0] * finalNumbersMultiplier;
        secondNumber = expressionNumbers[1] * finalNumbersMultiplier;

        quotient = firstNumber / secondNumber;
        remainder = firstNumber % secondNumber;

        for (int i = 0; i < incorrectAnswersQuotientsPlusMinus.Length; i++)
        {
            incorrectAnswersQuotientsPlusMinus[i] = GetRandomQuotient(divisionNumbers[i].IncorrectAnswerQuotientPlusMinusPossitiblities);
        }

        for (int i = 0; i < incorrectAnswersRemaindersPlusMinus.Length; i++)
        {
            incorrectAnswersRemaindersPlusMinus[i] = GetRandomRemainder(divisionNumbers[i].IncorrectAnswerRemainderPlusMinusPossitiblities);
        }
    }

    protected override string GetCorrectAnswerAsString()
    {
        return GetAnswerText(quotient, remainder);
    }

    private int GetRandomQuotient(int[] quotientPossibilities)
    {
        int answerQuotient = quotient;

        answerQuotient = (int)GetRandomPlusMinusFromNumber(quotient, GetRandomValueFromIntArray(quotientPossibilities) * finalNumbersMultiplier);

        return answerQuotient;
    }

    private int GetRandomRemainder(int[] remainderPossibilities)
    {
        int answerReminder = remainder;

        // While the documentation says "wrong quotient MUST BE...", it also says
        // "the remaind CAN BE...".
        // While there is no more definition about the "can" probability, what will define it is a 
        // Random, with 50% chance.

        if (Random.value > 0.5f)
        {
            answerReminder = (int)GetRandomPlusMinusFromNumber(remainder, GetRandomValueFromIntArray(remainderPossibilities) * finalNumbersMultiplier);
        }

        return answerReminder;
    }

    private string GetAnswerText(int answerQuotient, int answerReminder)
    {
        return $"{answerQuotient}{MathQuestionLocalization.GetTranslatedQuestionTitle(" is the quotient and ")}{answerReminder}{MathQuestionLocalization.GetTranslatedQuestionTitle(" is the remainder.")}";
    }
}
