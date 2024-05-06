using System;
using Dimasyechka.Code.GameTimeFlowSystem.Controllers;
using Dimasyechka.Code.SaveLoadSystem.Controllers;
using Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters.Base;
using UnityEngine;

namespace Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters
{
    public class SaveLoadSlotDataConverter : SaveLoadBaseConverter<SaveLoadSlotData>
    {
        private const string AutoSaveFileName = "AutoSave"; // Не нравится по названию определять, надо булку добавить наверно внутрь файла :c
        private const string AutoSaveName = "Автосохранение";
        private const string RandomVisibleSaveFileName = "Новое сохранение";

        public override SaveLoadSlotData GetConverterData(string saveFileName)
        {
            SaveLoadSlotData saveLoadSlotData = new SaveLoadSlotData();

            // Тут ещё можем добавить опрос пользователя, может он хочет сам название файлу сохранения давать?
            saveLoadSlotData.VisibleSaveFileName = saveFileName == AutoSaveFileName ? AutoSaveName : RandomVisibleSaveFileName; // Тоже не нравится :(
            saveLoadSlotData.GameVersion = Application.version.ToString();
            saveLoadSlotData.SaveDateTimeStamp = DateTime.Now.Ticks;
            saveLoadSlotData.GameSaveDateTimeStamp = GameTimeFlowController.Instance.DateNow();

            return saveLoadSlotData;
        }

        public override void ParseGeneralSaveData(GeneralSaveData generalSaveData) { }

        public override void SetDefaultData() { }
    }

    [System.Serializable]
    public class SaveLoadSlotData
    {
        /// <summary>
        ///      Отображаемое время файла сохранения в игре
        /// </summary>
        public string VisibleSaveFileName;

        /// <summary>
        ///     Реальное время на компьютере во время создания сохранения
        /// </summary>
        public long SaveDateTimeStamp;

        /// <summary>
        ///     Время в игре на момент сохранения
        /// </summary>
        public string GameSaveDateTimeStamp;

        /// <summary>
        ///     Версия игры, в которой было создано это сохранение
        /// </summary>
        public string GameVersion;
    }
}