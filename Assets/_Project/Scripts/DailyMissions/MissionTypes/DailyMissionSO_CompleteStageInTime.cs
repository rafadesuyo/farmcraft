using UnityEngine;

public class DailyMissionSO_CompleteStageInTime : DailyMissionSO
{
    //TODO: implement this mission type. Link: https://ocarinastudios.atlassian.net/browse/DQG-1967?atlOrigin=eyJpIjoiODA0M2U4YWM5MmU3NGMwYWJlNTUzMWM0MzU0Yjk3YmEiLCJwIjoiaiJ9

    public override string GetMissionEventName()
    {
        throw new System.NotImplementedException();
    }

    public override DailyMission.DailyMissionProgress[] GetRequiredMissionProgress(DailyMission.DailyMissionProgress[] startMissionProgress)
    {
        throw new System.NotImplementedException();
    }

    public override void GoToMission()
    {
        throw new System.NotImplementedException();
    }

    public override void HandleMissionUpdate(DailyMission.DailyMissionProgress[] currentMissionProgress, IGameEvent gameEvent)
    {
        throw new System.NotImplementedException();
    }
}
