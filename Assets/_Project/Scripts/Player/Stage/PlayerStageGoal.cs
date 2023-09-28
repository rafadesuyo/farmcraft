using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DreamQuiz.Player
{
    public class PlayerStageGoal
    {
        public enum PlayerStageGoalState { Unfinished, Lose, Win }
        public List<StageGoalProgress> StageGoalProgressList { get; private set; }
        public PlayerStageGoalState State { get; private set; }

        private Dictionary<StageGoal.StageGoalRequisite, StageGoalEvaluator> stageGoalEvaluatorDict =
            new Dictionary<StageGoal.StageGoalRequisite, StageGoalEvaluator>();
        private PlayerStageData playerStageData;

        public event Action<PlayerStageGoalState> OnGoalStateChange;

        public PlayerStageGoal(PlayerStageData playerStageData, StageGoal[] stageGoals)
        {
            InitializeGoalEvaluators();

            State = PlayerStageGoalState.Unfinished;
            this.playerStageData = playerStageData;
            this.playerStageData.SleepingTime.OnSleepingTimeChanged += SleepingTime_OnSleepingTimeChanged;

            SetupGoals(stageGoals);
        }

        private void SetupGoals(StageGoal[] stageGoals)
        {
            StageGoalProgressList = new List<StageGoalProgress>();

            for (int i = 0; i < stageGoals.Length; i++)
            {
                var stageGoal = stageGoals[i];
                StageGoalProgress stageGoalProgress = new StageGoalProgress(playerStageData, stageGoal, GetStageGoalEvaluator(stageGoal.Requisite));
                stageGoalProgress.OnComplete += StageGoalProgress_OnComplete;
                StageGoalProgressList.Add(stageGoalProgress);
            }
        }

        private void StageGoalProgress_OnComplete()
        {
            ChckIfMainGoalWasReached();
        }

        private void ChckIfMainGoalWasReached()
        {
            var remainingMainGoals = StageGoalProgressList.Where(p => p.StageGoal.MainGoal && !p.IsComplete).ToList();

            if (remainingMainGoals.Count == 0)
            {
                ExecuteWin();
            }
        }

        private void InitializeGoalEvaluators()
        {
            stageGoalEvaluatorDict.Clear();

            var allGoalEvaluatorTypes = Assembly.GetAssembly(typeof(StageGoalEvaluator)).GetTypes()
                .Where(t => typeof(StageGoalEvaluator).IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var goalEvaluatorType in allGoalEvaluatorTypes)
            {
                StageGoalEvaluator goalEvaluatorInstance = Activator.CreateInstance(goalEvaluatorType) as StageGoalEvaluator;

                stageGoalEvaluatorDict.Add(goalEvaluatorInstance.GetGoalRequisite(), goalEvaluatorInstance);
            }
        }

        private StageGoalEvaluator GetStageGoalEvaluator(StageGoal.StageGoalRequisite stageGoalRequisite)
        {
            if (stageGoalEvaluatorDict.TryGetValue(stageGoalRequisite, out var stageGoalEvaluator))
            {
                return stageGoalEvaluator;
            }

            return null;
        }

        public int GetStageGoalsCompletedCount()
        {
            int completedGoals = 0;

            foreach (var goalProgress in StageGoalProgressList)
            {
                if (goalProgress.IsComplete)
                {
                    completedGoals++;
                }
            }

            return completedGoals;
        }

        private void SetGoalState(PlayerStageGoalState state)
        {
            if (state == State)
            {
                return;
            }

            State = state;
            OnGoalStateChange?.Invoke(state);
        }

        public void ExecuteWin()
        {
            SetGoalState(PlayerStageGoalState.Win);
        }

        public void ExecuteLose()
        {
            SetGoalState(PlayerStageGoalState.Lose);
        }

        private void SleepingTime_OnSleepingTimeChanged(int sleepingTime)
        {
            if (sleepingTime < 0)
            {
                ExecuteLose();
            }
        }
    }
}
