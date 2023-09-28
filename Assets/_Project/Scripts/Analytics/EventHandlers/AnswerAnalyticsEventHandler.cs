using UnityEngine;

namespace DreamQuiz
{
    [RequireComponent(typeof(QuizSystem))]
    public class AnswerAnalyticsEventHandler : MonoBehaviour
    {
        private QuizSystem quizSystem;

        private void Awake()
        {
            quizSystem = GetComponent<QuizSystem>();
        }

        private void OnEnable()
        {
            quizSystem.OnAnswer += QuizSystem_OnAnswer;
        }

        private void OnDisable()
        {
            quizSystem.OnAnswer += QuizSystem_OnAnswer;
        }

        private void QuizSystem_OnAnswer(QuizAnswerEventArgs quizAnswerEventArgs)
        {
            SendAnalytics(quizAnswerEventArgs);
        }

        private void SendAnalytics(QuizAnswerEventArgs quizAnswerEventArgs)
        {
            if (quizAnswerEventArgs.Category == QuizCategory.Math || quizAnswerEventArgs.Category == QuizCategory.Puzzles)
            {
                return;
            }

            AnswerAnalyticsModel answerAnalyticsModel = new AnswerAnalyticsModel(quizAnswerEventArgs);
            AnalyticsManager.Instance.SendAnswerAnalytics(answerAnalyticsModel);
        }
    }
}