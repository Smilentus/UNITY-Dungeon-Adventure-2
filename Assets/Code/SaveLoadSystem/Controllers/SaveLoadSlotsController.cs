using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.SaveLoadSystem.Controllers
{
    public class SaveLoadSlotsController : MonoBehaviour
    {
        public event Action onSavedSlotsUpdated;


        private List<RuntimeSaveLoadSlotData> savedSlots = new List<RuntimeSaveLoadSlotData>();
        public List<RuntimeSaveLoadSlotData> SavedSlots => savedSlots;


        private SaveLoadSystemController _saveLoadSystemController;

        [Inject]
        public void Construct(SaveLoadSystemController saveLoadSystemController)
        {
            _saveLoadSystemController = saveLoadSystemController;
        }


        public bool TryGetSaveFiles(out string[] saveFiles)
        {
            saveFiles = new string[0];

            try
            {
                if (Directory.Exists(_saveLoadSystemController.SaveFolderPath))
                {
                    saveFiles = Directory.GetFiles(_saveLoadSystemController.SaveFolderPath);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Произошла ошибка при попытке получить доступные файлы сохранения! \n" + ex.Message);
            }

            // Не смогли получить файлы либо сохранений не обнаружено
            if (saveFiles.Length == 0)
            {
                return false;
            }

            return true;
        }

        public void LoadSaveSlots()
        {
            string[] saveFiles;

            bool isSavesExists = TryGetSaveFiles(out saveFiles);

            //if (isSavesExists)
            //{
            savedSlots.Clear();

            foreach (string filePath in saveFiles)
            {
                if (_saveLoadSystemController.TryReadFileJSON(filePath, out string fileJsonData))
                {
                    GeneralSaveData saveData = _saveLoadSystemController.DeserializeJSON(fileJsonData);

                    if (saveData != null)
                    {
                        foreach (object obj in saveData.SavedObjects)
                        {
                            if (obj is SaveLoadSlotData)
                            {
                                SaveLoadSlotData saveLoadSlotData = obj as SaveLoadSlotData;
                                savedSlots.Add(new RuntimeSaveLoadSlotData()
                                {
                                    SaveFilePath = filePath,
                                    SlotData = saveLoadSlotData
                                });
                            }
                        }
                    }
                }
            }

            savedSlots = savedSlots.OrderByDescending(x => x.SlotData.SaveDateTimeStamp).ToList();

            onSavedSlotsUpdated?.Invoke();
            //}
        }


        public void CreateNewSaveSlot()
        {
            _saveLoadSystemController.TrySaveGameState();
        }

    
        public string GetAutoSaveFullFilePath()
        {
            LoadSaveSlots();

            RuntimeSaveLoadSlotData slotData = savedSlots.Find(x => Path.GetFileNameWithoutExtension(x.SaveFilePath).Equals("AutoSave"));

            if (slotData == null) return "";
            else return slotData.SaveFilePath;
        }
    }

    [System.Serializable]
    public class RuntimeSaveLoadSlotData
    {
        public string SaveFilePath;
        public SaveLoadSlotData SlotData;
    }
}