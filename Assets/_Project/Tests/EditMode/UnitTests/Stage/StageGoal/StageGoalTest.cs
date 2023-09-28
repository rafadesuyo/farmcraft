using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DreamQuiz.Tests.Stage
{
    public class StageGoalTest : MonoBehaviour
    {
        [Test]
        public void StageGoal_Constructor_HappyPath()
        {
            StageGoal.StageGoalRequisite stageGoalRequisite = StageGoal.StageGoalRequisite.CorrectAnswers;
            int value = 1;

            var stageGoal = new StageGoal(stageGoalRequisite, value, true, new GoalReward(1, CurrencyType.Gold, CollectibleType.None, true));

            Assert.IsTrue(stageGoal.Requisite == stageGoalRequisite,
                $"[Constructor] Expected requisite '{stageGoalRequisite}' and recieved '{stageGoal.Requisite}'");
            Assert.IsTrue(stageGoal.TargetValue == value,
                $"[Constructor] Expected value '{value}' and recieved '{stageGoal.TargetValue}'");
        }
    }
}