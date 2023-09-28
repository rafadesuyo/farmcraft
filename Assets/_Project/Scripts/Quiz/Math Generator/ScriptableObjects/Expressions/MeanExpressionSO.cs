using UnityEngine;

[CreateAssetMenu(menuName = "Math/Mean")]
public class MeanExpressionSO : MathExpressionSO
{
    [SerializeField] private int[] firstIncorrectAnswerPlusMinusPossitiblities = null;
    [SerializeField] private int[] secondIncorrectAnswerPlusMinusPossitiblities = null;
    [SerializeField] private int[] thirdIncorrectAnswerPlusMinusPossitiblities = null;

    protected override string QuestionTitle
    {
        get
        {
            string questionTitle = MathQuestionLocalization.GetTranslatedQuestionTitle("What is the mean of ");
            foreach (int number in expressionNumbers)
            {
                questionTitle = $"{questionTitle}{number}, ";
            }

            //Remove last ", ".
            questionTitle = $"{questionTitle.Substring(0, questionTitle.Length - 2)}?";
            return questionTitle;
        }
    }

    protected override float CorrectAnswer
    {
        get
        {
            float finalAnswer = 0;

            foreach (int number in expressionNumbers)
            {
                finalAnswer += number;
            }

            return finalAnswer / expressionNumbers.Length;
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
}
