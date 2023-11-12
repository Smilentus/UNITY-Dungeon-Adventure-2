public class BuffsSaveLoadConverter : SaveLoadBaseConverter<BuffSaveLoadData>
{
    public override BuffSaveLoadData GetConverterData()
    {
        BuffSaveLoadData saveLoadData = new BuffSaveLoadData();

        return saveLoadData;
    }

    public override void ParseGeneralSaveData(GeneralSaveData generalSaveData)
    {

    }

    public override void SetDefaultData()
    {

    }
}

[System.Serializable]
public class BuffSaveLoadData
{

}