public class CraftingSystemSaveLoadConverter : SaveLoadBaseConverter<CraftingSystemSaveLoadData>
{
    public override CraftingSystemSaveLoadData GetConverterData()
    {
        CraftingSystemSaveLoadData saveLoadData = new CraftingSystemSaveLoadData();

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
public class CraftingSystemSaveLoadData
{

}
