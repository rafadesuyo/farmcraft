using UnityEngine;

namespace DreamQuiz
{
    public class PowerPawn : QuestionPawnAnimated
    {
        //Variables
        [Header("Variables")]
        [SerializeField] private int powerCount = 1;

        protected override void OnQuizStateChanged(QuizState quizState)
        {
            if (quizState == QuizState.Passed && pawnInQuiz != null)
            {
                pawnInQuiz.PlayerStageData.AddPowerCount(powerCount);

                WaitForDisappearingAnimation();
            }

            base.OnQuizStateChanged(quizState);
        }

        public override string GetDescription()
        {
            return base.GetDescription() + "\nPower can break walls.";
        }
    }
}