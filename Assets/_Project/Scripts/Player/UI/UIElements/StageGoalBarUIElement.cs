using UnityEngine;

namespace DreamQuiz.Player
{
    public class StageGoalBarUIElement : PlayerUIElement
    {
        [SerializeField] private GameObject goalPrefab = null;
        [SerializeField] private GameObject divisorPrefab = null;
        [SerializeField] private StageTimeUI timeInfoUI = null;
        [SerializeField] private RectTransform goalsContainer = null;

        protected override void InitializeUI()
        {
            GenericPool.CreatePool<StageGoalItem>(goalPrefab, goalsContainer);
            GenericPool.CreatePool<StageGoalDivisor>(divisorPrefab, goalsContainer);

            PopulateGoals();
        }

        private void PopulateGoals()
        {
            var stageGoalProgressList = playerStageInstance.PlayerStageGoal.StageGoalProgressList;
            int numberOfGoalsCreated = 0;

            for (int i = 0; i < stageGoalProgressList.Count; i++)
            {
                if (stageGoalProgressList[i].StageGoal.Requisite == StageGoal.StageGoalRequisite.SecondsToFinishTheStage)
                {
                    continue;
                }

                if (numberOfGoalsCreated > 0)
                {
                    GenericPool.GetItem<StageGoalDivisor>();
                }

                StageGoalItem item = GenericPool.GetItem<StageGoalItem>();
                item.Setup(stageGoalProgressList[i]);

                numberOfGoalsCreated++;
            }

            if (stageGoalProgressList.Count > 0)
            {
                GenericPool.GetItem<StageGoalDivisor>();
                timeInfoUI?.transform.SetAsLastSibling();
            }

            var timeGoalProgress = stageGoalProgressList.Find(p => p.StageGoal.Requisite == StageGoal.StageGoalRequisite.SecondsToFinishTheStage);
            timeInfoUI?.Setup(timeGoalProgress);
        }
    }
}