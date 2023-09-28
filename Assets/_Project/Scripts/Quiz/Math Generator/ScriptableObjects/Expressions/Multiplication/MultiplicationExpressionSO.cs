using UnityEngine;

[CreateAssetMenu(menuName = "Math/Multiplication/Easy")]
public class MultiplicationExpressionSO : MathExpressionSO
{
    [SerializeField] protected int[] firstIncorrectAnswerPlusMinusPossitiblities = null;
    [SerializeField] protected int[] secondIncorrectAnswerPlusMinusPossitiblities = null;
    [SerializeField] protected int[] thirdIncorrectAnswerPlusMinusPossitiblities = null;

    protected override string QuestionTitle
    {
        get
        {
            string questionTitle = MathQuestionLocalization.GetTranslatedQuestionTitle("What is ");
            foreach (int number in expressionNumbers)
            {
                questionTitle = $"{questionTitle}{number} * ";
            }

            //Remove last " * ".
            questionTitle = $"{questionTitle.Substring(0, questionTitle.Length - 3)}?";
            return questionTitle;
        }
    }

    protected override float CorrectAnswer
    {
        get
        {
            float finalAnswer = 1;

            foreach (int number in expressionNumbers)
            {
                finalAnswer *= number;
            }

            return finalAnswer;
        }
    }

    protected override string FirstIncorrectAnswer
    {
        get
        {
            return $"{GetRandomPlusMinusFromNumber(CorrectAnswer, GetRandomValueFromIntArray(firstIncorrectAnswerPlusMinusPossitiblities))}";
        }
    }

    protected override string SecondIncorrectAnswer
    {
        get
        {
            return $"{GetRandomPlusMinusFromNumber(CorrectAnswer, GetRandomValueFromIntArray(secondIncorrectAnswerPlusMinusPossitiblities))}";
        }
    }

    protected override string ThirdIncorrectAnswer
    {
        get
        {
            return $"{GetRandomPlusMinusFromNumber(CorrectAnswer, GetRandomValueFromIntArray(thirdIncorrectAnswerPlusMinusPossitiblities))}";
        }
    }

    protected override void SetupExpressionNumbers()
    {
        base.SetupExpressionNumbers();
    }
}
