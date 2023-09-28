using System.Collections.Generic;
using UnityEngine;
using System;

public class DailyMissionsManager : LocalSingleton<DailyMissionsManager>
{
    //Constants
    private const int maxDaysToCompleteMissions = 1;

    //Variables
    [Header("Variables")]
    [SerializeField] private int randomMissionsPerDay = 10;

    [Space(10)]

    [SerializeField] private DailyMissionsListSO fixedDailyMissions;
    [SerializeField] private DailyMissionsListSO randomDailyMissions;

#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField] private int hoursToAddToCurrentDateTime;

    [Tooltip("Enable Debug Inputs with the Keyboard.\nSpace: Update Daily Missions.\nP: Print Current Missions.")]
    [SerializeField] private bool enableDebugInputs;

    [Space(10)]

    [SerializeField] protected bool showDebugs;
#endif

    private DateTime missionsStartDate;

    private List<DailyMission> currentFixedMissions = new List<DailyMission>();
    private List<DailyMission> currentRandomMissions = new List<DailyMission>();

    private List<DailyMissionSO> allDailyMissionTypes = new List<DailyMissionSO>();

    //Getters
    public DateTime MissionsStartDate => missionsStartDate;
    public List<DailyMission> CurrentFixedMissions => currentFixedMissions;
    public List<DailyMission> CurrentRandomMissions => currentRandomMissions;

#if UNITY_EDITOR
    private void Update()
    {
        if (enableDebugInputs == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                UpdateDailyMissions();
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                PrintCurrentMissions();
            }
        }
    }
#endif

    private void OnEnable()
    {
        InitializeDailyMissions();
    }

    public void InitializeDailyMissions()
    {
        CreateAllDailyMissionTypesList();
        LoadDailyMissionsManagerSave();
        UpdateDailyMissions();
    }

    private void LoadDailyMissionsManagerSave()
    {
        DailyMissionsManagerSave dailyMissionsManagerSave = PlayerProgress.DailyMissionsManagerSave;

        DailyMissionSave[] fixedMissionsSaves = dailyMissionsManagerSave.currentFixedMissions;
        DailyMissionSave[] randomMissionsSaves = dailyMissionsManagerSave.currentRandomMissions;

        if(fixedMissionsSaves == null || randomMissionsSaves == null)
        {
            return;
        }

        missionsStartDate = dailyMissionsManagerSave.missionsStartDate;

        if(LoadMissionsList(fixedMissionsSaves, currentFixedMissions) == false || LoadMissionsList(randomMissionsSaves, currentRandomMissions) == false)
        {
            ClearFixedMissions();
            ClearRandomMissions();
        }
    }

    /// <summary>
    /// Loads an array of DailyMissionSave to a DailyMission list.
    /// </summary>
    /// <param name="missionsSaves">Array of DailyMissionSave to load from.</param>
    /// <param name="missionsList">List of DailyMission to load to.</param>
    /// <returns>If the load was sucessful or not.</returns>
    private bool LoadMissionsList(DailyMissionSave[] missionsSaves, List<DailyMission> missionsList)
    {
        foreach (DailyMissionSave dailyMissionSave in missionsSaves)
        {
            try
            {
                DailyMission dailyMission = new DailyMission(dailyMissionSave, allDailyMissionTypes);

                missionsList.Add(dailyMission);
            }
            catch (Exception o)
            {
                Debug.LogError($"An error ocurred while trying to load the Daily Missions.\n{o}");
                return false;
            }
        }

        return true;
    }

    public void UpdateDailyMissions()
    {
        if(currentFixedMissions.Count <= 0 || currentRandomMissions.Count <= 0 || CheckIfMissionsHaveExpired() == true)
        {
#if UNITY_EDITOR
            if(showDebugs == true)
            {
                if (CheckIfMissionsHaveExpired() == true)
                {
                    Debug.Log("Daily Missions will be reset because the missions have expired.");
                }
                else
                {
                    Debug.Log("Daily Missions will be reset because one of the current missions list is empty.");
                }
            }
#endif

            ResetDailyMissions();

            GameManager.Instance.SaveGame();
        }
    }

    private void ResetDailyMissions()
    {
        missionsStartDate = GetStartOfCurrentDay();

        ClearFixedMissions();
        ClearRandomMissions();

        PopulateFixedMissions();
        PopulateRandomMissions();

#if UNITY_EDITOR
        if(showDebugs == true)
        {
            PrintCurrentMissions();
        }
#endif
    }

    private void ClearFixedMissions()
    {
        foreach(DailyMission dailyMission in currentFixedMissions)
        {
            dailyMission.DisableMission();
        }

        currentFixedMissions.Clear();
    }

    private void ClearRandomMissions()
    {
        foreach (DailyMission dailyMission in currentRandomMissions)
        {
            dailyMission.DisableMission();
        }

        currentRandomMissions.Clear();
    }

    private void PopulateFixedMissions()
    {
        for(int i = 0; i < fixedDailyMissions.DailyMissions.Count; i++)
        {
            DailyMission dailyMission = new DailyMission(fixedDailyMissions.DailyMissions[i]);

            currentFixedMissions.Add(dailyMission);
        }
    }

    private void PopulateRandomMissions()
    {
        for (int i = 0; i < randomMissionsPerDay; i++)
        {
            currentRandomMissions.Add(GetRandomMission());
        }
    }

    private DailyMission GetRandomMission()
    {
        DailyMissionSO dailyMissionSO = randomDailyMissions.DailyMissions[UnityEngine.Random.Range(0, randomDailyMissions.DailyMissions.Count)];

        DailyMission dailyMission = new DailyMission(dailyMissionSO);

        return dailyMission;
    }

    private void CreateAllDailyMissionTypesList()
    {
        allDailyMissionTypes.Clear();

        foreach (DailyMissionSO dailyMissionSO in fixedDailyMissions.DailyMissions)
        {
            allDailyMissionTypes.Add(dailyMissionSO);
        }

        foreach (DailyMissionSO dailyMissionSO in randomDailyMissions.DailyMissions)
        {
            allDailyMissionTypes.Add(dailyMissionSO);
        }
    }

    public DateTime GetStartOfCurrentDay()
    {
#if UNITY_EDITOR
        int daysToAdd = (int)(GetCurrentDateTime() - DateTime.Today).TotalDays;

        return DateTime.Today.AddDays(daysToAdd);
#else
        return DateTime.Today;
#endif
    }

    public DateTime GetCurrentDateTime()
    {
#if UNITY_EDITOR
        return DateTime.Now.AddHours(hoursToAddToCurrentDateTime);
#else
        return DateTime.Now;
#endif
    }

    public DateTime GetMissionsExpireTime()
    {
        return missionsStartDate.AddDays(maxDaysToCompleteMissions);
    }

    public TimeSpan GetTimeLeftToCompleteMissions()
    {
        DateTime currentDate = GetCurrentDateTime();

        return GetMissionsExpireTime() - currentDate;
    }

    private bool CheckIfMissionsHaveExpired()
    {
        return GetTimeLeftToCompleteMissions().TotalHours <= 0;
    }

    public bool CanAnyCurrentFixedMissionBeCollected()
    {
        foreach (DailyMission dailyMission in currentFixedMissions)
        {
            if(dailyMission.State == DailyMission.MissionState.CanBeCompleted)
            {
                return true;
            }
        }

        return false;
    }

    public bool CanAnyCurrentRandomMissionBeCollected()
    {
        foreach (DailyMission dailyMission in currentRandomMissions)
        {
            if (dailyMission.State == DailyMission.MissionState.CanBeCompleted)
            {
                return true;
            }
        }

        return false;
    }

    public bool IsAnyCurrentFixedMissionIncompleted()
    {
        foreach (DailyMission dailyMission in currentFixedMissions)
        {
            if (dailyMission.State != DailyMission.MissionState.Completed)
            {
                return true;
            }
        }

        return false;
    }

    public bool CollectFirstCurrentFixedMissionThatCanBeCollected(out Reward[] rewards)
    {
        foreach (DailyMission dailyMission in currentFixedMissions)
        {
            if (dailyMission.State == DailyMission.MissionState.CanBeCompleted)
            {
                dailyMission.CollectRewards();

                rewards = dailyMission.Rewards;
                return true;
            }
        }

        Debug.LogWarning("There's no Current Fixed Daily Mission that can be collected!");

        rewards = null;
        return false;
    }

#if UNITY_EDITOR
    private void PrintCurrentMissions()
    {
        Debug.Log("Daily Missions Manager Current Missions");

        Debug.Log($"Fixed Missions, Count: {currentFixedMissions.Count}.");

        foreach(DailyMission dailyMission in currentFixedMissions)
        {
            PrintDailyMission(dailyMission);
        }

        Debug.Log($"Random Missions, Count: {currentRandomMissions.Count}.");

        foreach (DailyMission dailyMission in currentRandomMissions)
        {
            PrintDailyMission(dailyMission);
        }
    }

    private void PrintDailyMission(DailyMission dailyMission)
    {
        System.Text.StringBuilder dailyMissionText = new System.Text.StringBuilder();

        dailyMissionText.Append($"Mission Type: {dailyMission.DailyMissionSO}.");

        dailyMissionText.Append($"\nCurrent Mission Progress: ");
        PrintProgressArray(dailyMission.CurrentMissionProgress);

        dailyMissionText.Append($"\nRequired Mission Progress: ");
        PrintProgressArray(dailyMission.RequiredMissionProgress);

        Debug.Log(dailyMissionText.ToString());

        void PrintProgressArray(DailyMission.DailyMissionProgress[] dailyMissionProgresses)
        {
            for (int i = 0; i < dailyMissionProgresses.Length; i++)
            {
                dailyMissionText.Append($"(Target Type: {dailyMissionProgresses[i].targetType}, Target Value: {dailyMissionProgresses[i].targetValue})");

                if (i < dailyMissionProgresses.Length - 1)
                {
                    dailyMissionText.Append(", ");
                }
                else
                {
                    dailyMissionText.Append(".");
                }
            }
        }
    }
#endif
}

[System.Serializable]
public class DailyMissionsManagerSave
{
    public DateTime missionsStartDate;

    public DailyMissionSave[] currentFixedMissions;
    public DailyMissionSave[] currentRandomMissions;

    public DailyMissionsManagerSave() { } //Necessary for the JSON Deserializer to work

    public DailyMissionsManagerSave(DailyMissionsManager dailyMissionsManager)
    {
        missionsStartDate = dailyMissionsManager.MissionsStartDate;

        //Fixed Missions
        currentFixedMissions = new DailyMissionSave[dailyMissionsManager.CurrentFixedMissions.Count];

        for (int i = 0; i < dailyMissionsManager.CurrentFixedMissions.Count; i++)
        {
            currentFixedMissions[i] = new DailyMissionSave(dailyMissionsManager.CurrentFixedMissions[i]);
        }

        //Random Missions
        currentRandomMissions = new DailyMissionSave[dailyMissionsManager.CurrentRandomMissions.Count];

        for(int i = 0; i < dailyMissionsManager.CurrentRandomMissions.Count; i++)
        {
            currentRandomMissions[i] = new DailyMissionSave(dailyMissionsManager.CurrentRandomMissions[i]);
        }
    }
}