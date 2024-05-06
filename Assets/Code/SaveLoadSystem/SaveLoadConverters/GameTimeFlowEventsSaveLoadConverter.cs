using Dimasyechka.Code.GameTimeFlowSystem.Controllers;
using Dimasyechka.Code.SaveLoadSystem.Controllers;
using Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters.Base;

namespace Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters
{
    public class GameTimeFlowEventsSaveLoadConverter : SaveLoadBaseConverter<GameTimeFlowEventsSaveData>
    {
        public override void ParseGeneralSaveData(GeneralSaveData generalSaveData)
        {
            GameTimeFlowEventsSaveData saveData = ExtractDataType(generalSaveData);

            if (saveData != null)
            {
                GameTimeFlowController.Instance.SetSaveMaskData(saveData.SaveMaskData);
            }
        }

        public override GameTimeFlowEventsSaveData GetConverterData(string saveFileName)
        {
            GameTimeFlowEventsSaveData saveData = new GameTimeFlowEventsSaveData();

            saveData.SaveMaskData = GameTimeFlowController.Instance.GetSaveMaskData();

            return saveData;
        }

        public override void SetDefaultData()
        {
            GameTimeFlowController.Instance.CurrentDay = 1;
            GameTimeFlowController.Instance.CurrentHour = 10;
            GameTimeFlowController.Instance.CurrentMonth = 1;
            GameTimeFlowController.Instance.CurrentYear = 1;
        }
    }

    [System.Serializable]
    public class GameTimeFlowEventsSaveData
    {
        public GameTimeFlowEventSaveMaskData SaveMaskData;
    }
}