using DreamQuiz.Player;

namespace DreamQuiz
{
    public class StoryModeStageFinalizer : IStageFinalizer
    {
        public void FinalizeStage()
        {
            var players = StageManager.Instance.PlayerManager.GetPlayerStageInstances();

            foreach (var player in players)
            {
                PlayerProgress.AddAnsweredQuestions(PlayerManager.CurrentPlayerInstance.PlayerStageData.TotalCorrectAnswers);

                if (player.PlayerStageGoal.State == PlayerStageGoal.PlayerStageGoalState.Win)
                {
                    PlayerProgress.OnCompleteStage(StageManager.Instance.StageId, player.PlayerStageData.RemainingNodes.Count == 0, player.PlayerStageData.TotalStageScore);
                }

                foreach (var goalProgress in player.PlayerStageGoal.StageGoalProgressList)
                {
                    var rewardPackage = goalProgress.GetReward();
                    rewardPackage?.Unpack();
                }
            }

            GameManager.Instance.SaveGame();
        }
    }
}