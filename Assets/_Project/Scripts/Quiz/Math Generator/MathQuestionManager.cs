using System.Collections.Generic;
using UnityEngine;

public class MathQuestionManager : LocalSingleton<MathQuestionManager>
{
    [SerializeField] private List<MathExpressionSO> possibleExpressions = new List<MathExpressionSO>();

    public QuestionModel GetRandomQuestionByDifficulty(QuizDifficulty.Level difficulty)
    {
        if (difficulty == QuizDifficulty.Level.Hard)
        {
            // https://ocarinastudios.atlassian.net/browse/DQG-1450?atlOrigin=eyJpIjoiMTQ4YWVhZjIxZjFlNDcyYzk1Njk0YTU4ZDJlODM1ZDkiLCJwIjoiaiJ9
            // No hard math questions currently
            difficulty = QuizDifficulty.Level.Medium;
        }

        List<MathExpressionSO> expressionsByDifficulty = possibleExpressions.FindAll(e => e.Difficulty == difficulty);
        return expressionsByDifficulty[Random.Range(0, expressionsByDifficulty.Count)].GetQuestion();
    }
}
