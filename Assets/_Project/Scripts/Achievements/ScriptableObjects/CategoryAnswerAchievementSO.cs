using UnityEngine;

[CreateAssetMenu(menuName = "Achievement/CategoryAnswer")]
public class CategoryAnswerAchievementSO : AchievementSO
{
    public override int GetProgress()
    {
        // TODO: Use custom inspector to draw the category when creating the achievement
        // instead of a int;

        // 0 = no category defined, so, the only requeriment is to correctly answer a question
        if (TargetValue == 0)
        {
            return PlayerProgress.GetCorrectAnswersCount();
        }
        else
        {
            return PlayerProgress.GetCorrectAnswersCountByCategory((QuizCategory)TargetValue);
        }
    }
}