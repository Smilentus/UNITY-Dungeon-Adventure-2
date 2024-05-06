using System;
using Dimasyechka.Code.GameTimeFlowSystem.Controllers;
using Dimasyechka.Code.SaveLoadSystem.Controllers;
using Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters.Base;
using UnityEngine;

namespace Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters
{
    public class SaveLoadSlotDataConverter : SaveLoadBaseConverter<SaveLoadSlotData>
    {
        private const string AutoSaveFileName = "AutoSave"; // �� �������� �� �������� ����������, ���� ����� �������� ������� ������ ����� :c
        private const string AutoSaveName = "��������������";
        private const string RandomVisibleSaveFileName = "����� ����������";

        public override SaveLoadSlotData GetConverterData(string saveFileName)
        {
            SaveLoadSlotData saveLoadSlotData = new SaveLoadSlotData();

            // ��� ��� ����� �������� ����� ������������, ����� �� ����� ��� �������� ����� ���������� ������?
            saveLoadSlotData.VisibleSaveFileName = saveFileName == AutoSaveFileName ? AutoSaveName : RandomVisibleSaveFileName; // ���� �� �������� :(
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
        ///      ������������ ����� ����� ���������� � ����
        /// </summary>
        public string VisibleSaveFileName;

        /// <summary>
        ///     �������� ����� �� ���������� �� ����� �������� ����������
        /// </summary>
        public long SaveDateTimeStamp;

        /// <summary>
        ///     ����� � ���� �� ������ ����������
        /// </summary>
        public string GameSaveDateTimeStamp;

        /// <summary>
        ///     ������ ����, � ������� ���� ������� ��� ����������
        /// </summary>
        public string GameVersion;
    }
}