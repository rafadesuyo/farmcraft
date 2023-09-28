using System;
using UnityEngine;

public class QuestionHelper
{
    private const int defaultMathAnswerCount = 4;

    // public static QuizQuestion BuildQuestion(string questionText, float result, int index)
    // {
    //     QuizQuestion question = new QuizQuestion();

    //     question.question = questionText;
    //     question.correctAnswer = result.ToString();
    //     question.answers = new string[defaultMathAnswerCount];

    //     for (int i = 0; i < defaultMathAnswerCount; i++)
    //     {
    //         if (i == index)
    //         {
    //             question.answers[i] = result.ToString();
    //         }
    //         else
    //         {
    //             // If the index is from from a wrong question, get a random number based on the correctly result.
    //             question.answers[i] = ((result * i) + UnityEngine.Random.Range(1, 10)).ToString();
    //         }
    //     }

    //     return question;
    // }

    public static int GreatesCommonDivisor(int a, int b)
    {
        int remainder;

        while (b != 0)
        {
            remainder = a % b;
            a = b;
            b = remainder;
        }

        return a;
    }

    public static int Factorial(int a)
    {
        if (a == 0)
        {
            return 1;
        }
        else
        {
            return a * Factorial(a - 1);
        }
    }

    public static float GetMedian(float[] sourceNumbers)
    {
        float[] sortedPNumbers = (float[])sourceNumbers.Clone();
        Array.Sort(sortedPNumbers);

        int size = sortedPNumbers.Length;
        int mid = size / 2;
        float median = (size % 2 != 0) ? sortedPNumbers[mid] : (sortedPNumbers[mid] + sortedPNumbers[mid - 1]) / 2;
        return median;
    }

    public static float[] FillArrayForMeanAndMedian(float[] sourceArray)
    {
        for (int i = 0; i < sourceArray.Length; i++)
        {
            sourceArray[i] = UnityEngine.Random.Range(1, 100);
        }

        return sourceArray;
    }
}
