using System;
using UnityEngine;


public class SaveLoadSlotDataConverter : SaveLoadBaseConverter<SaveLoadSlotData>
{
    private string GetRandomVisibleSaveFileName => "����� ����������";


    private void Awake()
    {
        SaveLoadSystemController.Instance.AddSaveLoadConverter(this);
    }

    public SaveLoadSlotData GetData()
    {
        SaveLoadSlotData saveLoadSlotData = new SaveLoadSlotData();

        // ��� ��� ����� �������� ����� ������������, ����� �� ����� ��� �������� ����� ���������� ������?
        saveLoadSlotData.VisibleSaveFileName = GetRandomVisibleSaveFileName;
        saveLoadSlotData.GameVersion = Application.version.ToString();
        saveLoadSlotData.SaveDateTimeStamp = DateTime.Now.Ticks.ToString();

        return saveLoadSlotData;
    }

    public override SaveLoadSlotData GetConverterData()
    {
        SaveLoadSlotData saveLoadSlotData = new SaveLoadSlotData();

        // ��� ��� ����� �������� ����� ������������, ����� �� ����� ��� �������� ����� ���������� ������?
        saveLoadSlotData.VisibleSaveFileName = GetRandomVisibleSaveFileName;
        saveLoadSlotData.GameVersion = Application.version.ToString();
        saveLoadSlotData.SaveDateTimeStamp = DateTime.Now.Ticks.ToString();

        return saveLoadSlotData;
    }

    public override void ParseGeneralSaveData(GeneralSaveData generalSaveData) { }
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
    public string SaveDateTimeStamp;

    /// <summary>
    ///     ������ ����, � ������� ���� ������� ��� ����������
    /// </summary>
    public string GameVersion;
}