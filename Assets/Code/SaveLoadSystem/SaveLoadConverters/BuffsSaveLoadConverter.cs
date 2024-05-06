using System.Collections.Generic;
using Dimasyechka.Code.BuffSystem;
using Dimasyechka.Code.SaveLoadSystem.Controllers;
using Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters.Base;
using Zenject;

namespace Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters
{
    public class BuffsSaveLoadConverter : SaveLoadBaseConverter<BuffSaveLoadData>
    {
        private PlayerBuffsController _playerBuffsController;

        [Inject]
        public void Construct(PlayerBuffsController playerBuffsController)
        {
            _playerBuffsController = playerBuffsController;
        }


        public override BuffSaveLoadData GetConverterData(string saveFileName)
        {
            BuffSaveLoadData saveLoadData = new BuffSaveLoadData();

            saveLoadData.RuntimeBuffsSaveData = new List<RuntimeBuffSaveData>();

            for (int i = 0; i < _playerBuffsController.PlayerBuffsContainer.RuntimeBuffs.Count; i++)
            {
                saveLoadData.RuntimeBuffsSaveData.Add(_playerBuffsController.PlayerBuffsContainer.RuntimeBuffs[i].GetSaveBuffData());
            }

            return saveLoadData;
        }

        public override void ParseGeneralSaveData(GeneralSaveData generalSaveData)
        {
            BuffSaveLoadData buffSaveLoadData = ExtractDataType(generalSaveData);

            if (buffSaveLoadData != null)
            {
                _playerBuffsController.DisableAndRemoveAllBuffs();
                _playerBuffsController.LoadSaveBuffData(buffSaveLoadData);
            }
            else
            {
                SetDefaultData();
            }
        }

        public override void SetDefaultData()
        {
            _playerBuffsController.DisableAndRemoveAllBuffs();
        }
    }

    [System.Serializable]
    public class BuffSaveLoadData
    {
        public List<RuntimeBuffSaveData> RuntimeBuffsSaveData = new List<RuntimeBuffSaveData>();
    }
}