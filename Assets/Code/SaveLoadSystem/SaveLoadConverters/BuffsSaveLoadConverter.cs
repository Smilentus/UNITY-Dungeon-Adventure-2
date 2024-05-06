using System.Collections.Generic;
using Dimasyechka.Code.BuffSystem;
using Dimasyechka.Code.SaveLoadSystem.Controllers;
using Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters.Base;

namespace Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters
{
    public class BuffsSaveLoadConverter : SaveLoadBaseConverter<BuffSaveLoadData>
    {
        public override BuffSaveLoadData GetConverterData(string saveFileName)
        {
            BuffSaveLoadData saveLoadData = new BuffSaveLoadData();

            saveLoadData.RuntimeBuffsSaveData = new List<RuntimeBuffSaveData>();

            for (int i = 0; i < PlayerBuffsController.Instance.PlayerBuffsContainer.RuntimeBuffs.Count; i++)
            {
                saveLoadData.RuntimeBuffsSaveData.Add(PlayerBuffsController.Instance.PlayerBuffsContainer.RuntimeBuffs[i].GetSaveBuffData());
            }

            return saveLoadData;
        }

        public override void ParseGeneralSaveData(GeneralSaveData generalSaveData)
        {
            BuffSaveLoadData buffSaveLoadData = ExtractDataType(generalSaveData);

            if (buffSaveLoadData != null)
            {
                PlayerBuffsController.Instance.DisableAndRemoveAllBuffs();

                PlayerBuffsController.Instance.LoadSaveBuffData(buffSaveLoadData);
            }
            else
            {
                SetDefaultData();
            }
        }

        public override void SetDefaultData()
        {
            PlayerBuffsController.Instance.DisableAndRemoveAllBuffs();
        }
    }

    [System.Serializable]
    public class BuffSaveLoadData
    {
        public List<RuntimeBuffSaveData> RuntimeBuffsSaveData = new List<RuntimeBuffSaveData>();
    }
}