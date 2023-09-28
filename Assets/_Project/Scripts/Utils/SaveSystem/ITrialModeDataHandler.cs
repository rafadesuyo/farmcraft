using System;

public interface ITrialModeDataHandler
{
    public TrialModeCardInfo LoadScoreData(QuizCategory quizCategory);
    public void UpdateScoreData(QuizCategory quizCategory, int highScoreValue, int previousScoreValue);

    public TrialModeCardInfo LoadTimeData(QuizCategory quizCategory);
    public void UpdateTimeData(QuizCategory quizCategory, string lastTimePlayed, int currentCharges);
}
