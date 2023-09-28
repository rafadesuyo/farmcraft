namespace DreamQuiz
{
    public class DeathPawn : QuestionPawnAnimated
    {
        protected override void OnQuizStateChanged(QuizState quizState)
        {
            if (quizState == QuizState.WrongAnswer && pawnInQuiz != null)
            {
                pawnInQuiz.PlayerStageData.SleepingTime.SetValue(-1);

                WaitForDisappearingAnimation();
            }

            base.OnQuizStateChanged(quizState);
        }

        public override string GetDescription()
        {
            return base.GetDescription() + "\nInstantly lose if any wrong answer is chosen.";
        }
    }
}