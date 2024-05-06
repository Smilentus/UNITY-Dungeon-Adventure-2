using Dimasyechka.Code.SaveLoadSystem.Controllers;
using Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters.Base;

namespace Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters
{
    public class CraftingSystemSaveLoadConverter : SaveLoadBaseConverter<CraftingSystemSaveLoadData>
    {
        public override CraftingSystemSaveLoadData GetConverterData(string saveFileName)
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
}