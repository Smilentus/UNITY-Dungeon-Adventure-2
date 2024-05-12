using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters.Interfaces;
using Newtonsoft.Json;
using UnityEngine;

namespace Dimasyechka.Code.SaveLoadSystem.Controllers
{
    public class SaveLoadSystemController : MonoBehaviour
    {
        private readonly string _generatorReferenceAbc = "abcdefghijklmnopqrstuvwxyz";


        private string _saveFolderPath;
        public string SaveFolderPath => _saveFolderPath;


        [SerializeField]
        private List<string> _permittedExtensions = new List<string>() { ".savedata", ".json" };


        private JsonSerializerSettings _serializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
            NullValueHandling = NullValueHandling.Ignore
        };


        public List<ISaveLoadConverter> SaveLoadConverters = new List<ISaveLoadConverter>();


        private void Awake()
        {
            _saveFolderPath = Path.Combine(Application.persistentDataPath, "Saves");
        }


        public void AddSaveLoadConverter(ISaveLoadConverter converter)
        {
            if (SaveLoadConverters.Contains(converter)) return;
        
            SaveLoadConverters.Add(converter);
        }


        private bool IsExtensionPermitted(string fileName)
        {
            return _permittedExtensions.Contains(Path.GetExtension(fileName));
        }
        private string ConvertFileExtension(string fileName)
        {
            return Path.ChangeExtension(fileName, _permittedExtensions[0]);
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
                fileName.Append(_generatorReferenceAbc[UnityEngine.Random.Range(0, _generatorReferenceAbc.Length)]);
            }

            return fileName.ToString();
        }


        public bool TrySaveGameState(string saveFileName = "")
        {
            string fullFilePath;

            if (saveFileName == "")
            {
                fullFilePath = Path.Combine(_saveFolderPath, GenerateRandomSaveFileName());
            }
            else
            {
                fullFilePath = Path.Combine(_saveFolderPath, saveFileName);
            }

            return TryCollectAndSaveDataToFile(fullFilePath);
        }
        public bool TryLoadGameState(string loadFileName)
        {
            string fullFilePath = Path.Combine(_saveFolderPath, loadFileName);

            return TryLoadAndParseDataFromFile(fullFilePath);
        }

        public GeneralSaveData CollectSaveData(string saveFileName)
        {
            // Набрасываем все данные со всех контроллеров
            GeneralSaveData saveData = new GeneralSaveData();

            saveData.SavedObjects = new List<object>();

            foreach (ISaveLoadConverter converter in SaveLoadConverters)
            {
                saveData.SavedObjects.Add(converter.GetConverterData(saveFileName));
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
                string outputJSON = JsonConvert.SerializeObject(saveData, _serializerSettings);
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
                generalSaveData = JsonConvert.DeserializeObject<GeneralSaveData>(json, _serializerSettings);
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
            GeneralSaveData saveData = CollectSaveData(Path.GetFileNameWithoutExtension(fullFilePath));
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
                if (!Directory.Exists(_saveFolderPath))
                {
                    Directory.CreateDirectory(_saveFolderPath);
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

        public bool TryDeleteSaveFile(string fullFilePath)
        {
            if (File.Exists(fullFilePath))
            {
                File.Delete(fullFilePath);

                return true;
            }
            else
            {
                return false;
            }
        }


        [ContextMenu("DEBUG_Save_AutoSave")]
        private void DEBUG_SaveAutoSave()
        {
            TrySaveGameState("AutoSave");
        }
        [ContextMenu("DEBUG_Load_AutoSave")]
        private void DEBUG_LoadAutoSave()
        {
            TryLoadGameState("AutoSave");
        }
    }


    [System.Serializable]
    public class GeneralSaveData
    {
        public List<object> SavedObjects = new List<object>();
    }
}