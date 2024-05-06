using Dimasyechka.Code.SaveLoadSystem.Controllers;
using Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters.Base;
using Dimasyechka.Code.SkillsSystem.Controllers;

namespace Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters
{
    public class PlayerSkillsSaveLoadConverter : SaveLoadBaseConverter<PlayerSkillSaveLoadData>
    {
        public override PlayerSkillSaveLoadData GetConverterData(string saveFileName)
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
                    skillGUID = playerSkills[i].SkillProfile.skillGUID,
                    obtainedSkillLevel = playerSkills[i].RuntimeSkillCore.UpgradeableComponent.currentLevel
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
            // �� ����� ���� ��� ��������� ������� � ��������� ��� - ������ ������ � �� ���� �� ������� ������
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
}