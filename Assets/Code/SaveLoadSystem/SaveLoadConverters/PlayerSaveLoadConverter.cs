using Dimasyechka.Code.SaveLoadSystem.Controllers;
using Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters.Base;
using Zenject;

namespace Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters
{
    public class PlayerSaveLoadConverter : SaveLoadBaseConverter<PlayerSaveData>
    {
        private RuntimePlayer _runtimePlayer;

        [Inject]
        public void Construct(RuntimePlayer runtimePlayer)
        {
            _runtimePlayer = runtimePlayer;
        }

        public override PlayerSaveData GetConverterData(string saveFileName)
        {
            PlayerSaveData playerData = new PlayerSaveData();

            // Сохраняем игрока
            playerData.RuntimePlayerStatsSnapshot = _runtimePlayer.RuntimePlayerStats;

            return playerData;
        }

        public override void ParseGeneralSaveData(GeneralSaveData generalSaveData)
        {
            PlayerSaveData playerSaveData = ExtractDataType(generalSaveData);

            if (playerSaveData != null)
            {
                _runtimePlayer.RuntimePlayerStats = playerSaveData.RuntimePlayerStatsSnapshot;
            }
            else
            {
                SetDefaultData();
            }
        }

        public override void SetDefaultData()
        {
            _runtimePlayer.SetDefaultPlayerStats();
        }
    }

    [System.Serializable]
    public class PlayerSaveData
    {
        public RuntimePlayerStats RuntimePlayerStatsSnapshot;
    }
}