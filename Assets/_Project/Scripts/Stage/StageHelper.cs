public static class StageHelper
{
    // STAGE REWARDS 
    // https://ocarinastudios.atlassian.net/wiki/spaces/DQ/pages/2062680131/Stage+Rewards
    // Base Number of Coins - BC (Stage Gold completion reward)
    public const float BaseGold = 100;
    // Number of goals completed - GC - 20 gold per goal
    public const float GoldPerCompletedGoal = 20;
    // Number of correct answers - CA - 2
    public const float GoldPerCorrectAnswer = 2;
    // Reached the end node - REN
    // It has a value of 1 if the user does not reach it
    // It has a value of 1.2 if the player reaches it
    public const float FinishStageBonus = 1f;
    public const float EndNodeBonus = 0.2f;
    public const float AllNodeMultiplier = 1.2f;
    public const int RewardNodeMultiplier = 10;
    public const int StreakScore = 3;

    public static int GetAnswerScoreMultiplier(QuizDifficulty.Level difficulty)
    {
        switch (difficulty)
        {
            case QuizDifficulty.Level.Easy:
                return 2;
            case QuizDifficulty.Level.Medium:
                return 5;
            case QuizDifficulty.Level.Hard:
                return 20;
        }
        return 1;
    }

    public static int GetCorrectAnswerStreakGold(int correctAnswerBestStreak)
    {
        if (correctAnswerBestStreak < 3)
        {
            return 0;
        }

        switch (correctAnswerBestStreak)
        {
            case 3:
                return 3;
            case 4:
                return 4;
            case 5:
                return 6;
            case 7:
                return 8;
            case 8:
                return 10;
            case 9:
                return 15;
            default:
                return 20;
        }
    }
}
