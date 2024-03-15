using System;
using UnityEngine;


public class SaveLoadSlotDataConverter : SaveLoadBaseConverter<SaveLoadSlotData>
{
    private string GetRandomVisibleSaveFileName => "����� ����������";

    public override SaveLoadSlotData GetConverterData(string saveFileName)
    {
        SaveLoadSlotData saveLoadSlotData = new SaveLoadSlotData();

        // ��� ��� ����� �������� ����� ������������, ����� �� ����� ��� �������� ����� ���������� ������?
        saveLoadSlotData.VisibleSaveFileName = saveFileName == "AutoSave" ? "��������������" : GetRandomVisibleSaveFileName;
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