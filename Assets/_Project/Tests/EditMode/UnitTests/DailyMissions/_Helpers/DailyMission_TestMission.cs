using UnityEngine;

namespace DreamQuiz.Tests.DailyMissions
{
    public class DailyMission_TestMission : DailyMissionSO
    {
        //Constants
        private const string testEventName = "TestEvent";

        public override string GetMissionEventName()
        {
            return testEventName;
        }

        public override DailyMission.DailyMissionProgress[] GetRequiredMissionProgress(DailyMission.DailyMissionProgress[] startMissionProgress)
        {
            DailyMission.DailyMissionProgress requiredMissionProgress = new DailyMission.DailyMissionProgress(0, 1);

            return new DailyMission.DailyMissionProgress[] { requiredMissionProgress };
        }

        public override void GoToMission()
        {
            Debug.LogWarning($"The mission type \"{this}\" doesn't have a GoToMission, as it is a test class.");
        }

        public override void HandleMissionUpdate(DailyMission.DailyMissionProgress[] currentMissionProgress, IGameEvent gameEvent)
        {
            currentMissionProgress[0].targetValue++;
        }
    }
}
