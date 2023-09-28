using System;
using System.Collections;
using System.Collections.Generic;

namespace DreamQuiz
{
    public interface IQuestionFormatter
    {
        IEnumerator FetchDataRelationship();
        FormatterResponseData GetFormatterResponseData();
        bool TryParseQuestion(QuestionDto questionDto, out QuestionModel questionModel);
        bool TryParseCategory(string categoryId, out QuizCategory quizCategory);
        List<QuestionModel> ParseQuestionList(QuestionListDto questionListDto);
        string GetCategoryId(QuizCategory quizCategory);
        string GetDifficultyId(QuizDifficulty.Level quizDifficulty);
    }
}