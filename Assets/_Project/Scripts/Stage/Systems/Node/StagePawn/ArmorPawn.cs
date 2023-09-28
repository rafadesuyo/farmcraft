namespace DreamQuiz
{
    public class ArmorPawn : QuestionPawnAnimated
    {
        protected override void OnQuizStateChanged(QuizState quizState)
        {
            if (quizState == QuizState.Passed && pawnInQuiz != null)
            {
                pawnInQuiz.PlayerStageData.AddArmor(1);

                WaitForDisappearingAnimation();
            }

            base.OnQuizStateChanged(quizState);
        }

        public override string GetDescription()
        {
            return $"Buff: Spend 50% less sleeping time when walking or answering wrong questions.";
        }
    }
}