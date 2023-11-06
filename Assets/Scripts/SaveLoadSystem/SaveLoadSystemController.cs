using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;


public class SaveLoadSystemController : MonoBehaviour
{
    private static SaveLoadSystemController instance;
    public static SaveLoadSystemController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SaveLoadSystemController>();    
            }

            return instance;
        }
    }


    private string generatorReferenceABC = "abcdefghijklmnopqrstuvwxyz";


    private string saveFolderPath;
    public string SaveFolderPath => saveFolderPath;


    private List<string> PermittedExtensions = new List<string>() { ".savedata", ".json" };

    private JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
    {
        TypeNameHandling = TypeNameHandling.All,
        NullValueHandling = NullValueHandling.Ignore
    };


    public List<ISaveLoadConverter> SaveLoadConverters = new List<ISaveLoadConverter>();


    private void Awake()
    {
        saveFolderPath = Path.Combine(Application.persistentDataPath, "Saves");
    }


    public void AddSaveLoadConverter(ISaveLoadConverter converter)
    {
        if (SaveLoadConverters.Contains(converter)) return;
        
        SaveLoadConverters.Add(converter);
    }


    private bool IsExtensionPermitted(string fileName)
    {
        return PermittedExtensions.Contains(Path.GetExtension(fileName));
    }
    private string ConvertFileExtension(string fileName)
    {
        return Path.ChangeExtension(fileName, PermittedExtensions[0]);
    }
    private string CheckExtensions(string filePath)
    {
        if (!IsExtensionPermitted(filePath))
        {
            return ConvertFileExtension(filePath);
        }
        else
        {
            return filePath;
        }
    }
    

    private string GenerateRandomSaveFileName()
    {
        StringBuilder fileName = new StringBuilder();

        for (int i = 0; i < 16; i++)
        {
            fileName.Append(generatorReferenceABC[UnityEngine.Random.Range(0, generatorReferenceABC.Length)]);
        }

        return fileName.ToString();
    }


    public bool TrySaveGameState()
    {
        string fullFilePath = Path.Combine(saveFolderPath, GenerateRandomSaveFileName());
        return TryCollectAndSaveDataToFile(fullFilePath);
    }


    public GeneralSaveData CollectSaveData()
    {
        // Набрасываем все данные со всех контроллеров
        GeneralSaveData saveData = new GeneralSaveData();

        saveData.savedObjects = new List<object>();

        foreach (ISaveLoadConverter converter in SaveLoadConverters)
        {
            saveData.savedObjects.Add(converter.GetConverterData());
        }

        return saveData;
    }
    public void ParseLoadedData(GeneralSaveData generalSaveData)
    {
        foreach (ISaveLoadConverter converter in SaveLoadConverters)
        {
            converter.ParseGeneralSaveData(generalSaveData);
        }
    }


    public string SerializeJSON(GeneralSaveData saveData)
    {
        try
        {
            string outputJSON = JsonConvert.SerializeObject(saveData, serializerSettings);
            return outputJSON;
        }
        catch (Exception ex)
        {
            Debug.LogError("Произошла ошибка во время сериализации данных!\nError: " + ex.Message);
        }

        return "";
    }
    public GeneralSaveData DeserializeJSON(string json)
    {
        GeneralSaveData generalSaveData = null;

        try
        {
            generalSaveData = JsonConvert.DeserializeObject<GeneralSaveData>(json, serializerSettings);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Произошла ошибка во время десериализации данных!\nError: " + ex.Message);
            return null;
        }

        return generalSaveData;
    }


    public bool TryCollectAndSaveDataToFile(string fullFilePath)
    {
        GeneralSaveData saveData = CollectSaveData();
        string json = SerializeJSON(saveData);

        fullFilePath = CheckExtensions(fullFilePath);

        return TryWriteFileJSON(fullFilePath, json);
    }
    public bool TryLoadAndParseDataFromFile(string fullFilePath)
    {
        fullFilePath = CheckExtensions(fullFilePath);

        if (TryReadFileJSON(fullFilePath, out string fileText))
        {
            GeneralSaveData loadedData = DeserializeJSON(fileText);
            ParseLoadedData(loadedData);
            return true;
        }
        else
        {
            return false;
        }
    }


    public bool TryWriteFileJSON(string fullFilePath, string json)
    {
        try
        {
            if (!Directory.Exists(saveFolderPath))
            {
                Directory.CreateDirectory(saveFolderPath);
            }

            File.WriteAllText(fullFilePath, json, Encoding.UTF8);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Произошла ошибка во время сохранения файла! \n" + ex.Message);
            return false;
        }

        return true;
    }
    public bool TryReadFileJSON(string fullFilePath, out string outputFileText)
    {
        fullFilePath = CheckExtensions(fullFilePath);
        outputFileText = "";

        try
        {
            outputFileText = File.ReadAllText(fullFilePath, Encoding.UTF8);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Произошла ошибка во время загрузки файла! \n" + ex.Message);
            outputFileText = "";
            return false;
        }

        return true;
    }
}


[System.Serializable]
public class GeneralSaveData
{
    public List<object> savedObjects = new List<object>();
}