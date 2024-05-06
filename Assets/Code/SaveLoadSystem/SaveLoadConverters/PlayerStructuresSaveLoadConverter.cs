using Dimasyechka.Code.SaveLoadSystem.Controllers;
using Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters.Base;

namespace Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters
{
    public class PlayerStructuresSaveLoadConverter : SaveLoadBaseConverter<PlayerStructuresSaveLoadData>
    {
        public override PlayerStructuresSaveLoadData GetConverterData(string saveFileName)
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
}