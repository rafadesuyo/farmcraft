using DreamQuiz;
using DreamQuiz.Player;
using System.Collections.Generic;
using UnityEngine;

public class StageCreatorStartup : MonoBehaviour
{
    [SerializeField] StageCreator stageCreator;
    [SerializeField] private GameObject managersContainer = null;

    private void Start()
    {
        LoadGame();
    }

    public void LoadGame()
    {
        GameData gameData = SaveManager.LoadData<GameData>(SaveManager.playerData_SaveInfo);
        OnLoadData(gameData);
    }

    private void OnLoadData(GameData data)
    {
        PlayerProgress.EvaluateLoad(data);
        managersContainer.SetActive(true);

        CollectibleManager.Instance.SetupCollectibles();
        SetupStage();
    }

    private void SetupStage()
    {
#if UNITY_EDITOR
        if (stageCreator.StageInfo == null)
        {
            Debug.LogError("The Stage Info is empty, please choose one to be able to play the stage.", this);
            return;
        }

        StageManager stageManager = FindObjectOfType<StageManager>();

        List<PlayerData> playerData = new List<PlayerData>() { PlayerProgress.GetPlayerData() };
        stageManager.SetupStage(stageCreator.StageInfo, playerData);
#endif
    }
}
