using UnityEngine;

namespace DreamQuiz
{
    public class WolfPawn : QuestionPawnAnimated
    {
        //Variables
        [Header("Variables")]
        [SerializeField] private int wolfCount = 1;

        protected override void OnQuizStateChanged(QuizState quizState)
        {
            if (quizState == QuizState.Passed && pawnInQuiz != null)
            {
                pawnInQuiz.PlayerStageData.AddWolvesCollected(wolfCount);

                WaitForDisappearingAnimation();
            }

            base.OnQuizStateChanged(quizState);
        }

        public override string GetDescription()
        {
            return base.GetDescription() + "\nCorrectly answers all questions or go back. The number of remaining questions will not reset if a wrong answer is given.";
        }
    }
}