using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters;
using UnityEngine;

namespace Dimasyechka.Code.SaveLoadSystem.Controllers
{
    public class SaveLoadSlotsController : MonoBehaviour
    {
        private static SaveLoadSlotsController instance;
        public static SaveLoadSlotsController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<SaveLoadSlotsController>();
                }

                return instance;
            }
        }


        public event Action onSavedSlotsUpdated;


        private List<RuntimeSaveLoadSlotData> savedSlots = new List<RuntimeSaveLoadSlotData>();
        public List<RuntimeSaveLoadSlotData> SavedSlots => savedSlots;


        public bool TryGetSaveFiles(out string[] saveFiles)
        {
            saveFiles = new string[0];

            try
            {
                if (Directory.Exists(SaveLoadSystemController.Instance.SaveFolderPath))
                {
                    saveFiles = Directory.GetFiles(SaveLoadSystemController.Instance.SaveFolderPath);
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
                if (SaveLoadSystemController.Instance.TryReadFileJSON(filePath, out string fileJsonData))
                {
                    GeneralSaveData saveData = SaveLoadSystemController.Instance.DeserializeJSON(fileJsonData);

                    if (saveData != null)
                    {
                        foreach (object obj in saveData.savedObjects)
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
            SaveLoadSystemController.Instance.TrySaveGameState();
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