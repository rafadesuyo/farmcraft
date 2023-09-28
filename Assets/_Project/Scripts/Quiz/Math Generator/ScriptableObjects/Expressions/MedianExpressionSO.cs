using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Math/Median")]
public class MedianExpressionSO : MathExpressionSO
{
    private int firstNumber = 0;
    private List<int> incorrectAnswers = new List<int>();
    protected override string QuestionTitle
    {
        get
        {
            string questionTitle = MathQuestionLocalization.GetTranslatedQuestionTitle("What is the median of ");
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
            Array.Sort(expressionNumbers);
            int middleIndex = expressionNumbers.Length / 2;
            float median;

            if (expressionNumbers.Length % 2 == 0)
            {
                median = (expressionNumbers[middleIndex] + expressionNumbers[middleIndex - 1]) / 2f;
            }
            else
            {
                median = expressionNumbers[middleIndex];
            }

            return median;
        }
    }

    protected override string FirstIncorrectAnswer
    {
        get
        {
            if (incorrectAnswers.Count >= 1)
            {
                return incorrectAnswers[0].ToString();
            }
            return "";
        }
    }

    protected override string SecondIncorrectAnswer
    {
        get
        {
            if (incorrectAnswers.Count >= 2)
            {
                return incorrectAnswers[1].ToString();
            }
            return "";
        }
    }

    protected override string ThirdIncorrectAnswer
    {
        get
        {
            if (incorrectAnswers.Count >= 3)
            {
                return incorrectAnswers[2].ToString();
            }
            return "";
        }
    }

    private int GenerateIncorrectAnswer(int correctAnswer)
    {
        int offset = UnityEngine.Random.Range(0, 6);
        int incorrectAnswer = correctAnswer + offset;

        while (incorrectAnswer == correctAnswer || expressionNumbers.Contains(incorrectAnswer) || incorrectAnswers.Contains(incorrectAnswer))
        {
            offset = UnityEngine.Random.Range(0, 6);
            incorrectAnswer = correctAnswer + offset;
        }

        return incorrectAnswer;
    }

    protected override void SetupExpressionNumbers()
    {
        base.SetupExpressionNumbers();
        firstNumber = expressionNumbers[0];
        expressionNumbers = new int[4];

        expressionNumbers[0] = firstNumber;
        expressionNumbers[1] = UnityEngine.Random.Range(firstNumber + 1, firstNumber + 6);
        expressionNumbers[2] = UnityEngine.Random.Range(firstNumber + 1, firstNumber + 6);
        expressionNumbers[3] = UnityEngine.Random.Range(firstNumber + 1, firstNumber + 6);
        expressionNumbers = ShuffleArray(expressionNumbers);

        incorrectAnswers.Clear();
        incorrectAnswers.Add(GenerateIncorrectAnswer(expressionNumbers[1]));
        incorrectAnswers.Add(GenerateIncorrectAnswer(expressionNumbers[2]));
        incorrectAnswers.Add(GenerateIncorrectAnswer(expressionNumbers[3]));
    }

    private int[] ShuffleArray(int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int temp = array[i];
            int randomIndex = UnityEngine.Random.Range(i, array.Length);
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
        return array;
    }
}