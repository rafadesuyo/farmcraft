using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

public class JSONFileHandler
{
    private readonly string _filePath;

    private readonly string _fileName;

    private readonly string _fullPath;

    private readonly bool _useEncryption;

    private readonly string _encryptionKey = "farm";

    private string EncryptDecrypt(string data)
    {
        string modifiedData = string.Empty;

        for (int i = 0; i < data.Length; ++i)
        {
            modifiedData += (char)(data[i] ^ _encryptionKey[i % _encryptionKey.Length]);
        }

        return modifiedData;
    }

    public JSONFileHandler(string filePath, string fileName, bool useEncryption)
    {
        _filePath = filePath;
        _fileName = fileName;
        _fullPath = Path.Combine(_filePath, _fileName);
        _useEncryption = useEncryption;

        if (!_useEncryption)
        {
            _fullPath += ".json";
        }
    }

    public void Save(GameData gameData)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_fullPath));
            string dataToSave = JsonConvert.SerializeObject(gameData, Formatting.Indented);

            if (_useEncryption)
            {
                dataToSave = EncryptDecrypt(dataToSave);
            }

            using (FileStream fileStream = new(_fullPath, FileMode.Create))
            {
                using StreamWriter streamWriter = new(fileStream);
                streamWriter.Write(dataToSave);
            }

            Debug.Log($"File saved at: {_fullPath}.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error occured when trying to save data to file: {_fullPath}\n{e}");
        }
    }

    public GameData Load()
    {
        GameData gameData = null;

        if (File.Exists(_fullPath))
        {
            try
            {
                string dataToLoad = string.Empty;

                using (FileStream fileStream = new(_fullPath, FileMode.Open))
                {
                    using StreamReader streamReader = new(fileStream);
                    dataToLoad = streamReader.ReadToEnd();
                }

                if (_useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                gameData = JsonConvert.DeserializeObject<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error occured when trying to load data from file: {_fullPath}\n{e}");
            }
        }

        return gameData;
    }
}