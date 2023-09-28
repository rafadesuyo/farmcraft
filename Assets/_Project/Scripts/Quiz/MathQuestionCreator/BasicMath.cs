using UnityEngine;

public class BasicMath
{
    public static int[] GetNumbers(int level)
    {
        int[] number = new int[2];
        int firstNumber = 0;
        int secondNumber = 0;

        if (level == 0)
        {
            firstNumber = Random.Range(1, 100);
            secondNumber = Random.Range(1, 100);
        }
        else if (level == 1)
        {
            firstNumber = Random.Range(1, 500);
            secondNumber = Random.Range(1, 500);
        }
        else
        {
            firstNumber = Random.Range(1, 1000);
            secondNumber = Random.Range(1, 1000);
        }

        number[0] = firstNumber;
        number[1] = secondNumber;
        return number;
    }

    // public static QuizQuestion AbsoluteDifference(int level)
    // {
    //     int[] numbers = BasicMath.GetNumbers(level);

    //     int firstNumber = numbers[0];
    //     int secondNumber = numbers[1];
    //     float absDiff = System.Math.Abs(firstNumber - secondNumber);
    //     int correctIndex = Random.Range(0, 3);
    //     string questionText = $"Value of: | {firstNumber}  -  {secondNumber} |?";

    //     return QuestionHelper.BuildQuestion(questionText, absDiff, correctIndex);
    // }

    // public static QuizQuestion Addition(int level)
    // {
    //     int[] numbers = BasicMath.GetNumbers(level);

    //     int firstNumber = numbers[0];
    //     int secondNumber = numbers[1];
    //     float sum = firstNumber + secondNumber;
    //     int correctIndex = Random.Range(0, 3);
    //     string questionText = $"Value of: {firstNumber}  +  {secondNumber}?";

    //     return QuestionHelper.BuildQuestion(questionText, sum, correctIndex);
    // }

    // public static QuizQuestion Difference(int level)
    // {
    //     int[] numbers = BasicMath.GetNumbers(level);

    //     int firstNumber = numbers[0];
    //     int secondNumber = numbers[1];
    //     float dif = firstNumber - secondNumber;
    //     int correctIndex = Random.Range(0, 3);
    //     string questionText = $"Value of: {firstNumber} - {secondNumber}?";

    //     return QuestionHelper.BuildQuestion(questionText, dif, correctIndex);
    // }

    // public static QuizQuestion Division(int level)
    // {
    //     int[] numbers = BasicMath.GetNumbers(level);

    //     int firstNumber = numbers[0];
    //     int secondNumber = numbers[1];
    //     int divisor = firstNumber * secondNumber;
    //     int dividend = secondNumber;
    //     float quotient = divisor / dividend;
    //     int correctIndex = Random.Range(0, 3);
    //     string questionText = $"Value of: {divisor} / {dividend}?";

    //     return QuestionHelper.BuildQuestion(questionText, quotient, correctIndex);
    // }

    // public static QuizQuestion FractionToDecimal(int level)
    // {
    //     int[] numbers = BasicMath.GetNumbers(level);

    //     int firstNumber = numbers[0];
    //     int secondNumber = numbers[1];
    //     float quotient = firstNumber / secondNumber;
    //     int correctIndex = Random.Range(0, 3);
    //     string questionText = $"What is the closest number to the value of:  {firstNumber} / {secondNumber}?";

    //     return QuestionHelper.BuildQuestion(questionText, quotient, correctIndex);
    // }

    // public static QuizQuestion Multiply(int level)
    // {
    //     int[] numbers = BasicMath.GetNumbers(level);

    //     int firstNumber = numbers[0];
    //     int secondNumber = numbers[1];
    //     float result = firstNumber * secondNumber;
    //     int correctIndex = Random.Range(0, 3);
    //     string questionText = $"Value of: {firstNumber} * {secondNumber}?";

    //     return QuestionHelper.BuildQuestion(questionText, result, correctIndex);
    // }
}
