using UnityEngine;

[CreateAssetMenu(menuName = "Math/Multiplication/Medium")]
public class MultiplicationExpressionMediumSO : MultiplicationExpressionSO
{
    [SerializeField] private int numbersMultiplier = 10;

    protected override string FirstIncorrectAnswer
    {
        get
        {
            float wrongAnswer = GetRandomPlusMinusFromNumber(CorrectAnswer, GetRandomValueFromIntArray(firstIncorrectAnswerPlusMinusPossitiblities));
            return $"{wrongAnswer * numbersMultiplier}";
        }
    }

    protected override string SecondIncorrectAnswer
    {
        get
        {
            float wrongAnswer = GetRandomPlusMinusFromNumber(CorrectAnswer, GetRandomValueFromIntArray(secondIncorrectAnswerPlusMinusPossitiblities));
            return $"{wrongAnswer * numbersMultiplier}";
        }
    }

    protected override string ThirdIncorrectAnswer
    {
        get
        {
            float wrongAnswer = GetRandomPlusMinusFromNumber(CorrectAnswer, GetRandomValueFromIntArray(thirdIncorrectAnswerPlusMinusPossitiblities));
            return $"{wrongAnswer * numbersMultiplier}";
        }
    }

    protected override void SetupExpressionNumbers()
    {
        base.SetupExpressionNumbers();
        UpdateNumbersWithMultiplier();
    }

    private void UpdateNumbersWithMultiplier()
    {
        for (int i = 0; i < expressionNumbers.Length; i++)
        {
            expressionNumbers[i] *= 10;
        }
    }
}
