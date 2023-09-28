namespace DreamQuiz.Player
{
    public class PlayerStageQuizHandler
    {
        private const int sleepingTimeLossOnWrongAnswer = 10;

        private PlayerStageInstance playerStageInstance;
        private QuizSystem quizSystem;

        public PlayerStageQuizHandler(PlayerStageInstance playerStageInstance, QuizSystem quizSystem)
        {
            this.playerStageInstance = playerStageInstance;
            this.quizSystem = quizSystem;

            if (this.quizSystem == null)
            {
                return;
            }

            quizSystem.OnQuizStateChange += QuizSystem_OnQuizStateChange;
            quizSystem.OnAnswer += QuizSystem_OnAnswer;
        }

        private void QuizSystem_OnQuizStateChange(QuizState quizState)
        {
            switch (quizState)
            {
                case QuizState.None:
                    playerStageInstance.PlayerPawn.NodeMovement.LockMovement(false);
                    break;
                case QuizState.Starting:
                    playerStageInstance.PlayerPawn.NodeMovement.LockMovement(true);
                    break;
            }
        }

        private void QuizSystem_OnAnswer(QuizAnswerEventArgs eventArgs)
        {
            if (eventArgs.IsCorrect)
            {
                playerStageInstance.PlayerStageData.AddCorrectAnswer(eventArgs.NodeDifficulty);
                PlayerProgress.AddCorrectAnswersToCategory(eventArgs.Category);
            }
            else
            {
                playerStageInstance.PlayerStageData.AddWrongAnswer(eventArgs.NodeDifficulty);
                playerStageInstance.PlayerStageData.SleepingTime.Use(sleepingTimeLossOnWrongAnswer);
            }
        }
    }
}