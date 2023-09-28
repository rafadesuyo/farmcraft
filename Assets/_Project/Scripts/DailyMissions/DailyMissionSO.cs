using UnityEngine;

public abstract class DailyMissionSO : ScriptableObject
{
    //Variables
    [Header("Mission Info")]
    [SerializeField] protected string missionName;
    [SerializeField][TextArea] protected string description;

    [Space(10)]

    [SerializeField] private DailyMissionContainerColorSO containerColor;

    [Space(10)]

    [SerializeField] protected Reward[] rewards;

#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField] protected bool showDebugs;
#endif

    //Getters
    public string MissionName => missionName;
    public DailyMissionContainerColorSO ContainerColor => containerColor;
    public Reward[] Rewards => rewards;

    public virtual string GetDescription(DailyMission dailyMission)
    {
        return description;
    }

    public virtual string GetProgressText(DailyMission.DailyMissionProgress[] currentTargetValues, DailyMission.DailyMissionProgress[] requiredTargetValues)
    {
        return $"{currentTargetValues[0].targetValue}/{requiredTargetValues[0].targetValue}";
    }

    public abstract string GetMissionEventName();

    public virtual DailyMission.DailyMissionProgress[] GetInitialMissionProgress()
    {
        return new DailyMission.DailyMissionProgress[] { new DailyMission.DailyMissionProgress(0, 0) };
    }

    public abstract DailyMission.DailyMissionProgress[] GetRequiredMissionProgress(DailyMission.DailyMissionProgress[] startMissionProgress);

    public abstract void HandleMissionUpdate(DailyMission.DailyMissionProgress[] currentMissionProgress, IGameEvent gameEvent);

    public virtual bool CanDailyMissionBeCompleted(DailyMission.DailyMissionProgress[] currentMissionProgress, DailyMission.DailyMissionProgress[] requiredMissionProgress)
    {
        if(currentMissionProgress.Length != requiredMissionProgress.Length)
        {
            Debug.LogError($"The lenght of the Daily Mission current target values is different from the required target values!\nMission Type: {this}", this);
            return false;
        }

        for(int i = 0; i < currentMissionProgress.Length; i++)
        {
            if(currentMissionProgress[i].targetValue < requiredMissionProgress[i].targetValue)
            {
                return false;
            }
        }

#if UNITY_EDITOR
        if (showDebugs == true)
        {
            Debug.Log($"Daily Mission of Type \"{this}\" can be completed.");
        }
#endif

        return true;
    }

    public abstract void GoToMission();
}
