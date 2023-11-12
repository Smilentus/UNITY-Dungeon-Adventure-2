public class WorldMapSaveLoadConverter : SaveLoadBaseConverter<WorldMapSaveLoadData>
{
    public override WorldMapSaveLoadData GetConverterData()
    {
        WorldMapSaveLoadData saveLoadData = new WorldMapSaveLoadData();

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
public class WorldMapSaveLoadData
{

}