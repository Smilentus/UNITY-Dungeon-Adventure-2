using UnityEngine;


public abstract class SaveLoadBaseConverter<T> : MonoBehaviour, ISaveLoadConverter
{
    public virtual void Awake()
    {
        SaveLoadSystemController.Instance.AddSaveLoadConverter(this);

        SetDefaultData();
    }


    object ISaveLoadConverter.GetConverterData() => GetConverterData();

    public abstract T GetConverterData();
    public abstract void ParseGeneralSaveData(GeneralSaveData generalSaveData);

    public abstract void SetDefaultData();

    public T ExtractDataType(GeneralSaveData generalSaveData)
    {
        foreach (object savedObject in generalSaveData.savedObjects)
        {
            if (savedObject.GetType().Equals(typeof(T)))
            {
                return (T)savedObject;
            }
        }

        return default(T);
    }
}