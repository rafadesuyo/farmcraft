using System;
using System.Collections;
using System.Collections.Generic;

namespace DreamQuiz
{
    public interface IQuestionRepository
    {
        IEnumerator FetchQuestionsFromRepository(int questionCount, QuizCategory category, QuizDifficulty.Level difficulty, QuizLanguageType language, Action<List<QuestionModel>> onSuccess, Action<string> onError);
    }
}