using UnityEngine;

public class Algebra
{
    public static int[] GetNumbers(int level)
    {
        int[] number = new int[3];
        int firstNumber = 0;
        int secondNumber = 0;
        int thirdNumber = 0;

        if (level == 0)
        {
            firstNumber = Random.Range(1, 10);
            secondNumber = Random.Range(1, 10);
            thirdNumber = Random.Range(secondNumber, 10);
        }
        else if (level == 1)
        {
            firstNumber = Random.Range(1, 20);
            secondNumber = Random.Range(1, 20);
            thirdNumber = Random.Range(secondNumber, 50);
        }
        else
        {
            firstNumber = Random.Range(1, 30);
            secondNumber = Random.Range(1, 30);
            thirdNumber = Random.Range(secondNumber, 100);
        }

        number[0] = firstNumber;
        number[1] = secondNumber;
        number[2] = thirdNumber;
        return number;
    }

    // public static QuizQuestion BasicAlgebra(int level)
    // {
    //     int[] numbers = Algebra.GetNumbers(level);

    //     int firstNumber = numbers[0];
    //     int secondNumber = numbers[1];
    //     int thirdNumber = numbers[2];
    //     int correctIndex = Random.Range(0, 3);

    //     int greatesCommonDivisor = QuestionHelper.GreatesCommonDivisor((thirdNumber - secondNumber), firstNumber);

    //     float equationNumericSimplified = (thirdNumber - secondNumber) / greatesCommonDivisor;
    //     float equationCharSimplified = firstNumber / greatesCommonDivisor;
    //     float result = (float)System.Math.Round(equationNumericSimplified / equationCharSimplified, 2);
    //     string questionText = $"Solve the equation: {firstNumber}x + {secondNumber} = {thirdNumber}";

    //     if (thirdNumber - secondNumber == 0)
    //     {
    //         result = 0;
    //     }
    //     else if (firstNumber == 1 || firstNumber == greatesCommonDivisor)
    //     {
    //         result = (thirdNumber - secondNumber);
    //     }

    //     return QuestionHelper.BuildQuestion(questionText, result, correctIndex);
    // }
}
