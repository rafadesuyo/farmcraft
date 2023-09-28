using UnityEngine;

public class DailyMissionSO_CompleteStageWithoutUsingAbilities : DailyMissionSO
{
    //TODO: implement this mission type. Link: https://ocarinastudios.atlassian.net/browse/DQG-1966?atlOrigin=eyJpIjoiYzJkOTY4YjBhNzMzNGNiYjg3MjJjNDgwMTE5YzUwNDQiLCJwIjoiaiJ9

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
