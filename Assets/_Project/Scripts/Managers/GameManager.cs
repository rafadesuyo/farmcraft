using System;
using UnityEngine;

public class GameManager : PersistentSingleton<GameManager>
{
    [SerializeField] private GameObject globalManagersContainer = null;

    public static event Action OnLoadData;

    public void Initialize()
    {
        LoadGame();
    }

    public void SaveGame()
    {
        PlayerProgress.PrepareForSave();
        SaveManager.SaveData<GameData>(SaveManager.playerData_SaveInfo, PlayerProgress.SaveState);
    }

    public void LoadGame()
    {
        GameData gameData = SaveManager.LoadData<GameData>(SaveManager.playerData_SaveInfo);
        AfterDataLoad(gameData);
    }

    public void DeleteSavedData()
    {
        SaveManager.DeleteData(SaveManager.playerData_SaveInfo);
    }

    private void AfterDataLoad(GameData data)
    {
        PlayerProgress.EvaluateLoad(data);
        globalManagersContainer.SetActive(true);

        OnLoadData?.Invoke();
    }
}
