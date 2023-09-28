using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DreamQuiz.Tests.Stage
{
    public class StageGoalEvaluatorTest : MonoBehaviour
    {
        [Test]
        public void StageGoalEvaluator_AssemblyMapping_GoalEvaluatorMissingRequisite()
        {
            var allGoalEvaluatorTypes = Assembly.GetAssembly(typeof(StageGoalEvaluator)).GetTypes()
                 .Where(t => typeof(StageGoalEvaluator).IsAssignableFrom(t) && !t.IsAbstract);

            var implementedRequisites = new List<StageGoal.StageGoalRequisite>();

            foreach (var goalEvaluatorType in allGoalEvaluatorTypes)
            {
                StageGoalEvaluator goalEvaluatorInstance = Activator.CreateInstance(goalEvaluatorType) as StageGoalEvaluator;
                StageGoal.StageGoalRequisite stageGoalRequisite;

                Assert.DoesNotThrow(() =>
                {
                    stageGoalRequisite = goalEvaluatorInstance.GetGoalRequisite();
                },
                $"[StageGoalEvaluator] Requisite for the GoalEvaluator type '{goalEvaluatorType}' not implemented");

                stageGoalRequisite = goalEvaluatorInstance.GetGoalRequisite();

                Assert.IsFalse(implementedRequisites.Contains(stageGoalRequisite),
                    $"[StageGoalEvaluator] Duplicate requisite '{stageGoalRequisite}' for the GoalEvaluator type '{goalEvaluatorType}'");

                implementedRequisites.Add(stageGoalRequisite);
            }
        }

        [Test]
        public void StageGoalEvaluator_AssemblyMapping_RequisiteMissingGoalEvaluatorImplementation()
        {
            List<StageGoal.StageGoalRequisite> stageGoalRequisites = Enum.GetValues(typeof(StageGoal.StageGoalRequisite))
                .Cast<StageGoal.StageGoalRequisite>()
                .ToList();

            var allGoalEvaluatorTypes = Assembly.GetAssembly(typeof(StageGoalEvaluator)).GetTypes()
                 .Where(t => typeof(StageGoalEvaluator).IsAssignableFrom(t) && !t.IsAbstract);

            var stageGoalEvaluatorDict = new Dictionary<StageGoal.StageGoalRequisite, StageGoalEvaluator>();

            foreach (var goalEvaluatorType in allGoalEvaluatorTypes)
            {
                StageGoalEvaluator goalEvaluatorInstance = Activator.CreateInstance(goalEvaluatorType) as StageGoalEvaluator;
                stageGoalEvaluatorDict.Add(goalEvaluatorInstance.GetGoalRequisite(), goalEvaluatorInstance);
            }

            foreach (var stageGoalRequisite in stageGoalRequisites)
            {
                Assert.IsTrue(stageGoalEvaluatorDict.ContainsKey(stageGoalRequisite),
                    $"[StageGoalEvaluator] StageGoalEvaluator for the requisite '{stageGoalRequisite}' not implemented");
            }
        }
    }
}