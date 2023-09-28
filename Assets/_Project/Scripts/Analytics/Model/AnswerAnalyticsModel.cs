using System;
using UnityEngine;

namespace DreamQuiz
{
    public class AnswerAnalyticsModel
    {
        public string UserID { get; private set; }
        public Guid QuestionID { get; set; }
        public short Result { get; set; }
        public int Duration { get; set; }

        public AnswerAnalyticsModel(QuizAnswerEventArgs quizAnswerEventArgs)
        {
            UserID = LoginManager.Instance.UserModel.UserId;
            QuestionID = quizAnswerEventArgs.QuestionID;
            Result = Convert.ToInt16(quizAnswerEventArgs.IsCorrect);
            Duration = Mathf.RoundToInt(quizAnswerEventArgs.Duration);
        }

        public AnswerAnalyticsDto ToDTO()
        {
            return AnalyticsHelper.Map<AnswerAnalyticsModel, AnswerAnalyticsDto>(this);
        }
    }
}