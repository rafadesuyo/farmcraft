using UnityEngine;

public class DailyMissionSO_PlayCategoriesInTrialMode : DailyMissionSO
{
    //TODO: implement this mission type. Link: https://ocarinastudios.atlassian.net/browse/DQG-1963?atlOrigin=eyJpIjoiYzEwYjI4OTllMjk3NGU0ZjhiNmRjMWU0MDkyZWEzZWYiLCJwIjoiaiJ9

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
