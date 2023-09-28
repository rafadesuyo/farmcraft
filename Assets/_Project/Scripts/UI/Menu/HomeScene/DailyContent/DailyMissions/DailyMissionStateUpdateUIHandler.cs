using UnityEngine;

public class DailyMissionStateUpdateUIHandler : MonoBehaviour
{
    protected void OnEnable()
    {
        EventsManager.AddListener(EventsManager.onMissionCanBeCompleted, OpenMissionStateUpdateView);
    }

    protected void OnDisable()
    {
        EventsManager.RemoveListener(EventsManager.onMissionCanBeCompleted, OpenMissionStateUpdateView);
    }

    private void OpenMissionStateUpdateView(IGameEvent gameEvent)
    {
        OnMissionStateChangedEvent onMissionStateChangedEvent = (OnMissionStateChangedEvent)gameEvent;

        //Only opens the menu if it's closed
        if(DailyMissionStateUpdateUI.Instance.DailyMissionsToShowUpdate.Count <= 0)
        {
            DailyMissionStateUpdateUI.Instance.DailyMissionsToShowUpdate.Add(onMissionStateChangedEvent.dailyMission);
            DailyMissionStateUpdateUI.Instance.OpenUI();
        }
        else
        {
            DailyMissionStateUpdateUI.Instance.DailyMissionsToShowUpdate.Add(onMissionStateChangedEvent.dailyMission);
        }
    }
}
