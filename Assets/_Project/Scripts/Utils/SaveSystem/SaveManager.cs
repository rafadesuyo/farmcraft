using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class SaveManager
{
    #region SaveInfos
    public static readonly (string name, string format) playerData_SaveInfo = ("ocarina_DreamQuiz", ".dat");
    #endregion

#if UNITY_EDITOR
    private static bool SHOW_DEBUG = true;
#endif

    public static T LoadData<T>((string name, string format) saveInfo, System.Action<bool> resultCallback = null) where T : new()
    {
        T dataToLoad = new T();
        bool success = false;
        string path = GetPath(saveInfo);

        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileToLoad = new FileStream(path, FileMode.Open);

            string dataAsJson = "";

            try
            {
                dataAsJson = (string)binaryFormatter.Deserialize(fileToLoad);
                dataToLoad = JsonConvert.DeserializeObject<T>(dataAsJson);
                fileToLoad.Close();
            }
            catch (System.Exception exception)
            {
#if UNITY_EDITOR
                if (SHOW_DEBUG)
                {
                    Debug.LogError($"Exception: {exception}");
                }
#endif
                // Old data is invalid, reset file format in the next save
                // This will happen only one time per device.

                // Closse file to let DeleteData function work on it
                fileToLoad.Close();
                // Delete data
                DeleteData(saveInfo);

                // Create a empty save
                ResetSaveData<T>(saveInfo);

                // Return a new Load with the new save
                return LoadData<T>(saveInfo);
            }

            success = true;

#if UNITY_EDITOR
            if (SHOW_DEBUG)
            {
                Debug.Log($"Loaded {typeof(T)} from {path}.\n{dataAsJson}");
            }
#endif
        }
#if UNITY_EDITOR
        else if (SHOW_DEBUG)
        {
            Debug.Log($"No file found. Returning new {typeof(T)}");
        }
#endif

        resultCallback?.Invoke(success);

        return dataToLoad;
    }

    public static void SaveData<T>((string name, string format) saveInfo, T data, System.Action<bool> resultCallback = null)
    {
        bool success = false;
        string path = GetPath(saveInfo);

        if (data != null)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream savedFile = File.Open(path, FileMode.OpenOrCreate);

            string dataAsJson = JsonConvert.SerializeObject(data);
            binaryFormatter.Serialize(savedFile, dataAsJson);
            savedFile.Close();

            success = true;

#if UNITY_EDITOR
            if (SHOW_DEBUG)
            {
                Debug.Log($"Saved {typeof(T)} at {path}.\n{dataAsJson}");
            }
#endif
        }
#if UNITY_EDITOR
        else if (SHOW_DEBUG)
        {
            Debug.LogError($"data param is null. Nothing to save.");
        }
#endif

        resultCallback?.Invoke(success);
    }

    public static void DeleteData((string name, string format) saveInfo, System.Action<bool> resultCallback = null)
    {
        bool success = false;
        string tempPath = GetPath(saveInfo);
        if (File.Exists(tempPath))
        {
#if UNITY_EDITOR
            if (SHOW_DEBUG)
            {
                Debug.Log($"Deleted {saveInfo.name}{saveInfo.format} at {Application.persistentDataPath}");
            }
#endif
            File.Delete(tempPath);
            success = true;
        }
#if UNITY_EDITOR
        else if (SHOW_DEBUG)
        {
            Debug.Log($"No file named {saveInfo.name}{saveInfo.format} found at {Application.persistentDataPath}");
        }
#endif

        resultCallback?.Invoke(success);
    }

    private static string GetPath((string name, string format) saveInfo)
    {
        return $"{Application.persistentDataPath}/{saveInfo.name}{saveInfo.format}";
    }

    private static void ResetSaveData<T>((string name, string format) saveInfo) where T : new()
    {
        // Force to create a new empty save file.
        SaveData(saveInfo, new T());
    }
}
