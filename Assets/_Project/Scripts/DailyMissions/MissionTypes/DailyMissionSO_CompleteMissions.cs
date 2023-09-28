using UnityEngine;

[CreateAssetMenu(menuName = "Daily Missions/Missions Types/Complete Missions")]
public class DailyMissionSO_CompleteMissions : DailyMissionSO
{
    //Variables
    [Header("Parameters")]
    [SerializeField] private int numberOfMissionsToComplete;

    [Space(10)]

    [SerializeField] private string textToReplaceWithNumberOfMissionsToComplete = "{value}";

    public override string GetDescription(DailyMission dailyMission)
    {
        return description.Replace(textToReplaceWithNumberOfMissionsToComplete, numberOfMissionsToComplete.ToString());
    }

    public override string GetMissionEventName()
    {
        return EventsManager.onMissionIsCompleted;
    }

    public override DailyMission.DailyMissionProgress[] GetRequiredMissionProgress(DailyMission.DailyMissionProgress[] startMissionProgress)
    {
        DailyMission.DailyMissionProgress requiredMissionProgress = new DailyMission.DailyMissionProgress(0, numberOfMissionsToComplete);

        return new DailyMission.DailyMissionProgress[] { requiredMissionProgress };
    }

    public override void HandleMissionUpdate(DailyMission.DailyMissionProgress[] currentMissionProgress, IGameEvent gameEvent)
    {
        OnMissionStateChangedEvent onMissionStateChangedEvent = (OnMissionStateChangedEvent)gameEvent;

#if UNITY_EDITOR
        if (showDebugs == true)
        {
            Debug.Log($"HandleMissionUpdate called for Daily Mission of Type \"{this}\".\nCompleted Mission Type: {onMissionStateChangedEvent.dailyMission.DailyMissionSO.GetType()}.");
        }
#endif

        //Don't increase the progress if the mission is of the same type as this one
        if (onMissionStateChangedEvent.dailyMission.DailyMissionSO.GetType() == this.GetType())
        {
            return;
        }

        currentMissionProgress[0].targetValue++;

        GameManager.Instance.SaveGame();

#if UNITY_EDITOR
        if (showDebugs == true)
        {
            Debug.Log($"Daily Mission of Type \"{this}\" was updated.\nProgress: {currentMissionProgress[0].targetValue}.");
        }
#endif
    }

    public override void GoToMission()
    {
        Debug.LogWarning($"The mission type \"{this}\" doesn't have a GoToMission, it shouldn't be possible to click on this button.");
    }
}
