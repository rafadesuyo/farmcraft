using UnityEngine;
using System.Linq;

public class Statiscs
{
    public static int[] GetNumbers(int level)
    {
        int[] number = new int[3];
        int firstNumber = 0;
        int secondNumber = 0;
        int arraySize = 0;

        if (level == 0)
        {
            firstNumber = Random.Range(10, 15);
            secondNumber = Random.Range(0, 9);
            arraySize = Random.Range(4, 6);
        }
        else if (level == 1)
        {
            firstNumber = Random.Range(10, 20);
            secondNumber = Random.Range(0, 9);
            arraySize = Random.Range(6, 8);
        }
        else
        {
            firstNumber = Random.Range(10, 30);
            secondNumber = Random.Range(0, 9);
            arraySize = Random.Range(8, 10);
        }

        number[0] = firstNumber;
        number[1] = secondNumber;
        number[2] = arraySize;
        return number;
    }

    // public static QuizQuestion Combination(int level)
    // {
    //     int[] numbers = Statiscs.GetNumbers(level);

    //     int firstNumber = numbers[0];
    //     int secondNumber = numbers[1];
    //     int correctIndex = Random.Range(0, 3);
    //     int factorialObjects = QuestionHelper.Factorial(firstNumber);
    //     int factorialGroups = QuestionHelper.Factorial(secondNumber);
    //     int factorialObjectsMinusGroups = QuestionHelper.Factorial(firstNumber - secondNumber);
    //     int solution = factorialObjects / (factorialGroups * factorialObjectsMinusGroups);
    //     string questionText = $"Find the number of combinations from {firstNumber} objects picked {secondNumber} at a time.";

    //     return QuestionHelper.BuildQuestion(questionText, solution, correctIndex);
    // }

    // public static QuizQuestion Permutation(int level)
    // {
    //     int[] numbers = Statiscs.GetNumbers(level);

    //     int firstNumber = numbers[0];
    //     int secondNumber = numbers[1];
    //     int correctIndex = Random.Range(0, 3);
    //     int factorialObjects = QuestionHelper.Factorial(firstNumber);
    //     int factorialGroups = QuestionHelper.Factorial(secondNumber);
    //     int solution = factorialObjects / factorialGroups;
    //     string questionText = $"Find the number of permutations from {firstNumber} objects picked {secondNumber} at a time.";

    //     return QuestionHelper.BuildQuestion(questionText, solution, correctIndex);
    // }

    // public static QuizQuestion Mean(int level)
    // {
    //     int[] numbers = Statiscs.GetNumbers(level);
    //     float[] arrayValues = new float[numbers[2]];

    //     arrayValues = QuestionHelper.FillArrayForMeanAndMedian(arrayValues);

    //     int correctIndex = Random.Range(0, 3);
    //     float solution = arrayValues.Average();
    //     string questionText = $"Given the series of numbers [{string.Join(", ", arrayValues)}], find the arithmatic mean of the serie.";

    //     return QuestionHelper.BuildQuestion(questionText, solution, correctIndex);
    // }

    // public static QuizQuestion Median(int level)
    // {
    //     int[] numbers = Statiscs.GetNumbers(level);
    //     float[] arrayValues = new float[numbers[2]];

    //     arrayValues = QuestionHelper.FillArrayForMeanAndMedian(arrayValues);

    //     int correctIndex = Random.Range(0, 3);
    //     float solution = QuestionHelper.GetMedian(arrayValues);
    //     string questionText = $"Given the series of numbers [{string.Join(", ", arrayValues)}], find the arithmatic median of the serie.";

    //     return QuestionHelper.BuildQuestion(questionText, solution, correctIndex);
    // }
}