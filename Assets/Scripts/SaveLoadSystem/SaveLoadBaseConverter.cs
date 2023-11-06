using UnityEngine;


public abstract class SaveLoadBaseConverter<T> : MonoBehaviour, ISaveLoadConverter
{
    object ISaveLoadConverter.GetConverterData() => GetConverterData();


    public abstract T GetConverterData();
    public abstract void ParseGeneralSaveData(GeneralSaveData generalSaveData);
}