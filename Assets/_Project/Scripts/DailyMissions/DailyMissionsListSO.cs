using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Daily Missions/Daily Missions List")]
public class DailyMissionsListSO : ScriptableObject
{
    //Variables
    [SerializeField] private List<DailyMissionSO> dailyMissions = new List<DailyMissionSO>();

    //Getters
    public List<DailyMissionSO> DailyMissions => dailyMissions;
}
