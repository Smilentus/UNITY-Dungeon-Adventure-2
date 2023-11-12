public class PlayerSkillsSaveLoadConverter : SaveLoadBaseConverter<PlayerSkillSaveLoadData>
{
    public override PlayerSkillSaveLoadData GetConverterData()
    {
        PlayerSkillSaveLoadData saveLoadData = new PlayerSkillSaveLoadData();

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
public class PlayerSkillSaveLoadData
{

}
