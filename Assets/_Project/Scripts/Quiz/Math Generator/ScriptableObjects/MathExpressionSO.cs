using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MathExpressionSO : ScriptableObject
{
    [SerializeField] private QuizDifficulty.Level difficulty = QuizDifficulty.Level.Easy;
    [SerializeField] protected MathNumber[] expressionPossibleNumbers = null;

    public QuizDifficulty.Level Difficulty => difficulty;

    protected int[] expressionNumbers = null;

    protected virtual string QuestionTitle => "";
    protected virtual float CorrectAnswer => 0;
    protected virtual string FirstIncorrectAnswer => "";
    protected virtual string SecondIncorrectAnswer => "";
    protected virtual string ThirdIncorrectAnswer => "";

    public QuestionModel GetQuestion()
    {
        SetupExpressionNumbers();

        QuestionModel question = new QuestionModel(Guid.Empty, QuestionTitle, GetAnswersAsString().ToList(), QuizCategory.Puzzles, difficulty, 0, string.Empty);

        return question;
    }

    protected virtual string GetCorrectAnswerAsString()
    {
        return $"{CorrectAnswer}";
    }

    protected virtual string[] GetAnswersAsString()
    {
        string[] answers = new string[4] { $"{GetCorrectAnswerAsString()}", $"{FirstIncorrectAnswer}", $"{SecondIncorrectAnswer}", $"{ThirdIncorrectAnswer}" };
        return answers;
    }

    protected virtual void SetupExpressionNumbers()
    {
        expressionNumbers = new int[expressionPossibleNumbers.Length];

        for (int i = 0; i < expressionNumbers.Length; i++)
        {
            expressionNumbers[i] = expressionPossibleNumbers[i].GetNumber();
        }
    }

    protected int GetRandomValueFromIntArray(int[] array)
    {
        return array[Random.Range(0, array.Length)];
    }

    protected float GetRandomPlusMinusFromNumber(float baseNumber, int plusMinusDifference)
    {
        int plusMinusResult = plusMinusDifference * (int)Mathf.Sign(Random.Range(-1, 0));
        return baseNumber + plusMinusResult;
    }
}
