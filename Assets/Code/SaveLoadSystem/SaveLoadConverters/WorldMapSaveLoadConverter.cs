using Dimasyechka.Code.SaveLoadSystem.Controllers;
using Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters.Base;

namespace Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters
{
    public class WorldMapSaveLoadConverter : SaveLoadBaseConverter<WorldMapSaveLoadData>
    {
        public override WorldMapSaveLoadData GetConverterData(string saveFileName)
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
}