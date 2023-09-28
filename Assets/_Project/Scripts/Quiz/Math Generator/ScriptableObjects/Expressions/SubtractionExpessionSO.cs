using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Math/Subtraction")]
public class SubtractionExpessionSO : MathExpressionSO
{
    [SerializeField] private int[] firstIncorrectAnswerPlusMinusPossitiblities = null;
    [SerializeField] private int[] secondIncorrectAnswerPlusMinusPossitiblities = null;
    [SerializeField] private int[] thirdIncorrectAnswerPlusMinusPossitiblities = null;

    protected override string QuestionTitle
    {
        get
        {
            string questionTitle = MathQuestionLocalization.GetTranslatedQuestionTitle("What is ");
            foreach (int number in expressionNumbers)
            {
                questionTitle = $"{questionTitle}{number} - ";
            }

            //Remove last " - ".
            questionTitle = $"{questionTitle.Substring(0, questionTitle.Length - 3)}?";
            return questionTitle;
        }
    }

    protected override float CorrectAnswer
    {
        get
        {
            float finalAnswer = expressionNumbers[0];

            for (int i = 1; i < expressionNumbers.Length; i++)
            {
                finalAnswer -= expressionNumbers[i];

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
        expressionNumbers = expressionNumbers.OrderByDescending(n => n).ToArray();
    }
}
