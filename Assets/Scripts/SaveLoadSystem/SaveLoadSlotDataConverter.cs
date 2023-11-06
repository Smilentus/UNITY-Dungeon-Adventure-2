using System;
using UnityEngine;


public class SaveLoadSlotDataConverter : SaveLoadBaseConverter<SaveLoadSlotData>
{
    private string GetRandomVisibleSaveFileName => "Новое сохранение";


    private void Awake()
    {
        SaveLoadSystemController.Instance.AddSaveLoadConverter(this);
    }

    public SaveLoadSlotData GetData()
    {
        SaveLoadSlotData saveLoadSlotData = new SaveLoadSlotData();

        // Тут ещё можем добавить опрос пользователя, может он хочет сам название файлу сохранения давать?
        saveLoadSlotData.VisibleSaveFileName = GetRandomVisibleSaveFileName;
        saveLoadSlotData.GameVersion = Application.version.ToString();
        saveLoadSlotData.SaveDateTimeStamp = DateTime.Now.Ticks.ToString();

        return saveLoadSlotData;
    }

    public override SaveLoadSlotData GetConverterData()
    {
        SaveLoadSlotData saveLoadSlotData = new SaveLoadSlotData();

        // Тут ещё можем добавить опрос пользователя, может он хочет сам название файлу сохранения давать?
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
    ///      Отображаемое время файла сохранения в игре
    /// </summary>
    public string VisibleSaveFileName;

    /// <summary>
    ///     Реальное время на компьютере во время создания сохранения
    /// </summary>
    public string SaveDateTimeStamp;

    /// <summary>
    ///     Версия игры, в которой было создано это сохранение
    /// </summary>
    public string GameVersion;
}