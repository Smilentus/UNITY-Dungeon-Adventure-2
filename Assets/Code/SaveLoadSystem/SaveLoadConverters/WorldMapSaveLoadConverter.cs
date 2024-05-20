using Dimasyechka.Code.LocationSystem.Controllers;
using Dimasyechka.Code.LocationSystem.Profiles;
using Dimasyechka.Code.SaveLoadSystem.Controllers;
using Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters.Base;
using Dimasyechka.Code.SkillsSystem.Controllers;
using System.Collections.Generic;
using Zenject;

namespace Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters
{
    public class WorldMapSaveLoadConverter : SaveLoadBaseConverter<WorldMapSaveLoadData>
    {
        private LocationsController _locationsController;

        [Inject]
        public void Construct(LocationsController locationsController)
        {
            _locationsController = locationsController;
        }


        public override WorldMapSaveLoadData GetConverterData(string saveFileName)
        {
            WorldMapSaveLoadData saveLoadData = new WorldMapSaveLoadData();

            if (_locationsController.CurrentLocation != null)
            {
                saveLoadData.LocationGuid = _locationsController.CurrentLocation.LocationGuid;

                saveLoadData.ExploredLocationsGuids = new List<string>();
                
                foreach (LocationProfile locationProfile in _locationsController.ExploredLocations)
                {
                    saveLoadData.ExploredLocationsGuids.Add(locationProfile.LocationGuid);
                }
            }

            return saveLoadData;
        }

        public override void ParseGeneralSaveData(GeneralSaveData generalSaveData)
        {
            WorldMapSaveLoadData worldMapSaveLoadData = ExtractDataType(generalSaveData);

            if (worldMapSaveLoadData != null)
            {
                _locationsController.SetLocationAfterLoading(worldMapSaveLoadData.LocationGuid);

                _locationsController.LoadExploredLocations(worldMapSaveLoadData.ExploredLocationsGuids);
            }
            else
            {
                SetDefaultData();
            }
        }

        public override void SetDefaultData()
        {
            _locationsController.SetLocationAfterLoading("green_forest");
        }
    }

    [System.Serializable]
    public class WorldMapSaveLoadData
    {
        public string LocationGuid;

        public List<string> ExploredLocationsGuids = new List<string>();
    }
}