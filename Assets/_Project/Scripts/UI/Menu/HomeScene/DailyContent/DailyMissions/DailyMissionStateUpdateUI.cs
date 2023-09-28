using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DailyMissionStateUpdateUI : UIControllerAnimated
{
    //Instance
    private static DailyMissionStateUpdateUI instance;

    //Components
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI missionCanBeCompletedText;

    //Variaveis
    [Header("Default Variables")]
    [SerializeField] private string textToReplaceWithMission = "{mission}";
    [SerializeField][TextArea] private string textMissionCanBeCompleted = "The mission {mission} can be completed!";

    private List<DailyMission> dailyMissionsToShowUpdate = new List<DailyMission>();

    //Getters
    public static DailyMissionStateUpdateUI Instance => instance;
    public List<DailyMission> DailyMissionsToShowUpdate => dailyMissionsToShowUpdate;

    protected override void OnAwake()
    {
        //Singleton instance
        instance = this;
    }

    protected override void OnOpen()
    {
        Setup();
    }

    protected override void OnClose()
    {
        ResetVariables();
    }

    private void Setup()
    {
        DailyMission dailyMissionToShowUpdate = dailyMissionsToShowUpdate[0];

        UpdateVariables(dailyMissionToShowUpdate);
    }

    private void UpdateVariables(DailyMission dailyMission)
    {
        missionCanBeCompletedText.text = textMissionCanBeCompleted.Replace(textToReplaceWithMission, dailyMission.MissionName);
    }

    private void ResetVariables()
    {
        missionCanBeCompletedText.text = string.Empty;

        dailyMissionsToShowUpdate.Clear();
    }

    public void HandleNextDailyMissionStateUpdate()
    {
        dailyMissionsToShowUpdate.RemoveAt(0);

        if (dailyMissionsToShowUpdate.Count <= 0)
        {
            CloseUI();
        }
        else
        {
            Setup();
        }
    }
}
