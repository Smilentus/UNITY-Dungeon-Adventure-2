using System;
using UnityEngine;


public class SaveLoadSlotDataConverter : SaveLoadBaseConverter<SaveLoadSlotData>
{
    private string GetRandomVisibleSaveFileName => "Новое сохранение";

    public override SaveLoadSlotData GetConverterData(string saveFileName)
    {
        SaveLoadSlotData saveLoadSlotData = new SaveLoadSlotData();

        // Тут ещё можем добавить опрос пользователя, может он хочет сам название файлу сохранения давать?
        saveLoadSlotData.VisibleSaveFileName = saveFileName == "AutoSave" ? "Автосохранение" : GetRandomVisibleSaveFileName;
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