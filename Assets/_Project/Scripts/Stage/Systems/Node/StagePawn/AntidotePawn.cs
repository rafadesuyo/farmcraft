using UnityEngine;

namespace DreamQuiz
{
    public class AntidotePawn : QuestionPawnAnimated
    {
        //Variables
        [Header("Variables")]
        [SerializeField] private int antidoteCount = 1;

        protected override void OnQuizStateChanged(QuizState quizState)
        {
            if (quizState == QuizState.Passed && pawnInQuiz != null)
            {
                pawnInQuiz.PlayerStageData.AddAntidoteCount(antidoteCount);

                WaitForDisappearingAnimation();
            }

            base.OnQuizStateChanged(quizState);
        }

        public override string GetDescription()
        {
            return "Buff: Gain an antidote to cure poison.";
        }
    }
}