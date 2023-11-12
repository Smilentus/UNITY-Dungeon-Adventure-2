public class PlayerStructuresSaveLoadConverter : SaveLoadBaseConverter<PlayerStructuresSaveLoadData>
{
    public override PlayerStructuresSaveLoadData GetConverterData()
    {
        PlayerStructuresSaveLoadData saveLoadData = new PlayerStructuresSaveLoadData();

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
public class PlayerStructuresSaveLoadData
{

}
