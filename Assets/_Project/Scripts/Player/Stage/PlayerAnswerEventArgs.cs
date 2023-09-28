namespace DreamQuiz.Player
{
    public class PlayerAnswerEventArgs
    {
        public QuizDifficulty.Level Difficulty { get; private set; }
        public int TotalCorrectAnswer { get; private set; }
        public int TotalWrongAnswer { get; private set; }
        public int CurrentCorrectAnswerStreak { get; private set; }

        public PlayerAnswerEventArgs(QuizDifficulty.Level difficulty, int totalCorrectAnswer, int totalWrongAnswer, int currentCorrectAnswerStreak)
        {
            Difficulty = difficulty;
            TotalCorrectAnswer = totalCorrectAnswer;
            TotalWrongAnswer = totalWrongAnswer;
            CurrentCorrectAnswerStreak = currentCorrectAnswerStreak;
        }
    }
}