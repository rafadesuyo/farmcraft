using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    private const string _fileName = "game-data";
    private const bool _useEncryption = false;
    private List<IDataPersistence> _dataPersistenceObjects;
    private JSONFileHandler _jsonFileHandler;

    public GameData GameData { get; private set; }

    public static DataPersistenceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        _jsonFileHandler = new JSONFileHandler(Application.persistentDataPath, _fileName, _useEncryption);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void UpdateDataPersistenceObjects()
    {
        _dataPersistenceObjects = FindAllDataPersistenceObjects();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        return FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>().ToList();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateDataPersistenceObjects();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        SaveGame();
    }

    public void NewGame()
    {
        GameData = new GameData();
    }

    public void LoadGame()
    {
        GameData = _jsonFileHandler.Load();

        if (GameData == null)
        {
            NewGame();
        }

        foreach (var dataPersistenceObject in _dataPersistenceObjects)
        {
            dataPersistenceObject.LoadData(GameData);
        }
    }

    public void SaveGame()
    {
        foreach (var dataPersistenceObject in _dataPersistenceObjects)
        {
            dataPersistenceObject.SaveData(GameData);
        }

        GameData.LastSave = DateTime.Now;
        _jsonFileHandler.Save(GameData);
    }
}