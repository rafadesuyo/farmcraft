using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace DreamQuiz.Tests.DailyMissions
{
    [TestFixture]
    public class DailyMissionTest
    {
        //Variables
        DailyMissionSO dailyMissionSO;
        List<DailyMissionSO> allDailyMissionTypes;

        DailyMission dailyMission;
        DailyMissionSave dailyMissionSave;
        DailyMission loadedDailyMission;

        [SetUp]
        public void SetUp()
        {
            dailyMissionSO = ScriptableObject.CreateInstance<DailyMission_TestMission>();
            dailyMissionSO.name = "TestMission";

            allDailyMissionTypes = new List<DailyMissionSO> { dailyMissionSO };
        }

        [Test]
        public void DailyMission_Constructors_HappyPath()
        {
            dailyMission = DailyMission_Constructor_Normal_HappyPath(dailyMissionSO);

            dailyMissionSave = DailyMissionSave_Constructor_HappyPath(dailyMission);

            loadedDailyMission = DailyMission_Constructor_FromSave_HappyPath(dailyMission, dailyMissionSave, allDailyMissionTypes);
        }

        [TearDown]
        public void TearDown()
        {
            //Disable missions to remove them from events
            dailyMission?.DisableMission();
            loadedDailyMission?.DisableMission();

            //Clear variables
            dailyMissionSO = null;
            allDailyMissionTypes = null;

            dailyMission = null;
            dailyMissionSave = null;
            loadedDailyMission = null;
        }

        private DailyMission DailyMission_Constructor_Normal_HappyPath(DailyMissionSO dailyMissionSO)
        {
            DailyMission dailyMission = new DailyMission(dailyMissionSO);

            Assert.IsTrue(dailyMission.DailyMissionSO == dailyMissionSO, $"Expected DailyMissionSO: \"{dailyMissionSO}\", Received: \"{dailyMission.DailyMissionSO}\".");
            Assert.IsTrue(dailyMission.State == DailyMission.MissionState.Active, $"Expected State: \"{DailyMission.MissionState.Active}\", Received: \"{dailyMission.State}\".");

            return dailyMission;
        }

        private DailyMission DailyMission_Constructor_FromSave_HappyPath(DailyMission originalDailyMission, DailyMissionSave dailyMissionSave, List<DailyMissionSO> allDailyMissionTypes)
        {
            DailyMission loadedDailyMission;

            Assert.DoesNotThrow(() => loadedDailyMission = new DailyMission(dailyMissionSave, allDailyMissionTypes));

            loadedDailyMission = new DailyMission(dailyMissionSave, allDailyMissionTypes);

            Assert.IsTrue(loadedDailyMission.DailyMissionSO == originalDailyMission.DailyMissionSO, $"Expected DailyMissionSO: \"{originalDailyMission.DailyMissionSO}\", Received: \"{loadedDailyMission.DailyMissionSO}\".");
            Assert.IsTrue(loadedDailyMission.State == dailyMissionSave.state, $"Expected State: \"{dailyMissionSave.state}\", Received: \"{loadedDailyMission.State}\".");

            CollectionAssert.AreEqual(dailyMissionSave.currentMissionProgress, loadedDailyMission.CurrentMissionProgress);
            CollectionAssert.AreEqual(dailyMissionSave.requiredMissionProgress, loadedDailyMission.RequiredMissionProgress);

            return loadedDailyMission;
        }

        private DailyMissionSave DailyMissionSave_Constructor_HappyPath(DailyMission dailyMission)
        {
            DailyMissionSave dailyMissionSave = new DailyMissionSave(dailyMission);

            Assert.IsTrue(dailyMissionSave.dailyMissionSOKey == dailyMission.DailyMissionSO.name, $"Expected DailyMissionSOKey: \"{dailyMission.DailyMissionSO.name}\", Received: \"{dailyMissionSave.dailyMissionSOKey}\".");
            Assert.IsTrue(dailyMissionSave.state == dailyMission.State, $"Expected State: \"{dailyMission.State}\", Received: \"{dailyMissionSave.state}\".");

            CollectionAssert.AreEqual(dailyMission.CurrentMissionProgress, dailyMissionSave.currentMissionProgress);
            CollectionAssert.AreEqual(dailyMission.RequiredMissionProgress, dailyMissionSave.requiredMissionProgress);

            return dailyMissionSave;
        }
    }

    public class DailyMissionProgressTest
    {
        [Test]
        public void DailyMissionProgress_Constructor_HappyPath()
        {
            int targetType = 1;
            int targetValue = 1;

            DailyMission.DailyMissionProgress dailyMissionProgress = new DailyMission.DailyMissionProgress(targetType, targetValue);

            Assert.IsTrue(dailyMissionProgress.targetType == targetType, $"Expected Target Type: \"{targetType}\", Received: \"{dailyMissionProgress.targetType}\".");
            Assert.IsTrue(dailyMissionProgress.targetValue == targetValue, $"Expected Target Value: \"{targetValue}\", Received: \"{dailyMissionProgress.targetValue}\".");
        }
    }
}
