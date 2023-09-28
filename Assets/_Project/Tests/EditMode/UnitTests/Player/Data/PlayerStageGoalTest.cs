using NUnit.Framework;
using UnityEngine;
using DreamQuiz.Player;
using System.Collections.Generic;
using System;
using System.Linq;

namespace DreamQuiz.Tests.Player
{
    public class PlayerStageGoalTest : MonoBehaviour
    {
        [Test]
        public void PlayerStageGoal_Constructor_HappyPath()
        {
            PlayerData playerData = new PlayerData()
            {
                Id = 0,
                MaxSleepingTime = 1
            };

            PlayerStageData playerStageData = new PlayerStageData(playerData);

            List<StageGoal> stageGoals = new List<StageGoal>();
            List<StageGoal.StageGoalRequisite> stageGoalRequisites = Enum.GetValues(typeof(StageGoal.StageGoalRequisite))
                .Cast<StageGoal.StageGoalRequisite>()
                .ToList();


            foreach (var stageGoalRequisite in stageGoalRequisites)
            {
                int rewardValue = 100;
                var goalReward = new GoalReward(rewardValue, CurrencyType.Gold, CollectibleType.None, true);

                int targetGoalValue = 1;
                bool isMainGoal = true;
                var newStageGoal = new StageGoal(stageGoalRequisite, targetGoalValue, isMainGoal, goalReward);

                stageGoals.Add(newStageGoal);
            }

            PlayerStageGoal playerStageGoal = new PlayerStageGoal(playerStageData, stageGoals.ToArray());

            foreach (var stageGoal in stageGoals)
            {
                var stageGoalProgress =
                    playerStageGoal.StageGoalProgressList.Any(p => p.StageGoal.Requisite == stageGoal.Requisite);

                Assert.IsNotNull(stageGoalProgress,
                    $"[Constructor] Expected StageGoalProgress for the {stageGoal.Requisite}");
            }
        }
    }
}