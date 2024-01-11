public class PlayerSkillsSaveLoadConverter : SaveLoadBaseConverter<PlayerSkillSaveLoadData>
{
    public override PlayerSkillSaveLoadData GetConverterData()
    {
        PlayerSkillSaveLoadData saveLoadData = new PlayerSkillSaveLoadData();

        PlayerSkill[] playerSkills = PlayerSkillsController.instance.GetPlayerSkills();

        SaveableSkillData[] saveableSkillData = ConvertToSaveableSkillData(playerSkills);

        saveLoadData.saveableSkills = saveableSkillData;

        return saveLoadData;
    }


    private SaveableSkillData[] ConvertToSaveableSkillData(PlayerSkill[] playerSkills)
    {
        SaveableSkillData[] saveableSkills = new SaveableSkillData[playerSkills.Length];

        for (int i = 0; i < playerSkills.Length; i++)
        {
            saveableSkills[i] = new SaveableSkillData() {
                skillGUID = playerSkills[i].skillProfile.skillGUID,
                obtainedSkillLevel = playerSkills[i].runtimeSkillCore.UpgradeableComponent.currentLevel
            };
        }

        return saveableSkills;
    }


    public override void ParseGeneralSaveData(GeneralSaveData generalSaveData)
    {
        PlayerSkillSaveLoadData playerSkillSaveLoadData = ExtractDataType(generalSaveData);

        if (playerSkillSaveLoadData != null)
        {
            foreach (SaveableSkillData saveableSkillData in playerSkillSaveLoadData.saveableSkills)
            {
                PlayerSkillsController.instance.LoadSkill(saveableSkillData.skillGUID, saveableSkillData.obtainedSkillLevel);
            }
        }
        else
        {
            SetDefaultData();
        }
    }

    public override void SetDefaultData()
    {
        // По факту пока что начальных навыков у персонажа нет - значит ничего и не надо по дефолту делать
    }
}

[System.Serializable]
public class PlayerSkillSaveLoadData
{
    public SaveableSkillData[] saveableSkills;
}

[System.Serializable]
public class SaveableSkillData
{
    public string skillGUID;
    public int obtainedSkillLevel;
}