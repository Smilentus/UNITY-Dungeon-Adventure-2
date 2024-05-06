using Dimasyechka.Code.GameTimeFlowSystem.Controllers;
using Dimasyechka.Code.SaveLoadSystem.Controllers;
using Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters.Base;
using Zenject;

namespace Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters
{
    public class GameTimeFlowEventsSaveLoadConverter : SaveLoadBaseConverter<GameTimeFlowEventsSaveData>
    {
        private GameTimeFlowController _gameTimeFlowController;

        [Inject]
        public void Construct(GameTimeFlowController gameTimeFlowController)
        {
            _gameTimeFlowController = gameTimeFlowController;
        }


        public override void ParseGeneralSaveData(GeneralSaveData generalSaveData)
        {
            GameTimeFlowEventsSaveData saveData = ExtractDataType(generalSaveData);

            if (saveData != null)
            {
                _gameTimeFlowController.SetSaveMaskData(saveData.SaveMaskData);
            }
        }

        public override GameTimeFlowEventsSaveData GetConverterData(string saveFileName)
        {
            GameTimeFlowEventsSaveData saveData = new GameTimeFlowEventsSaveData();

            saveData.SaveMaskData = _gameTimeFlowController.GetSaveMaskData();

            return saveData;
        }

        public override void SetDefaultData()
        {
            _gameTimeFlowController.CurrentDay = 1;
            _gameTimeFlowController.CurrentHour = 10;
            _gameTimeFlowController.CurrentMonth = 1;
            _gameTimeFlowController.CurrentYear = 1;
        }
    }

    [System.Serializable]
    public class GameTimeFlowEventsSaveData
    {
        public GameTimeFlowEventSaveMaskData SaveMaskData;
    }
}