using System.Collections.Generic;
using System;

[Serializable]
public class QuizData
{
    private Dictionary<QuizCategory, int> questionsAnswered = new Dictionary<QuizCategory, int>();

    public void IncreaseCategoryCorrectAnswers(QuizCategory category)
    {
        if (!questionsAnswered.TryGetValue(category, out int questionCount))
        {
            questionsAnswered.Add(category, questionCount);
        }

        questionCount++;
        questionsAnswered[category] = questionCount;
    }

    public int GetCorrectAnswersCountByCategory(QuizCategory category)
    {
        if (questionsAnswered.ContainsKey(category))
        {
            return questionsAnswered[category];
        }

        return 0;
    }

    public int GetCorrectAnswersCount()
    {
        int totalAnswers = 0;

        foreach (var categoryAnswersKeyPair in questionsAnswered)
        {
            totalAnswers += categoryAnswersKeyPair.Value;
        }

        return totalAnswers;
    }
}
