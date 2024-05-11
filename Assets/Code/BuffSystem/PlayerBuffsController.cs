using Dimasyechka.Code.BuffSystem.Containers;
using Dimasyechka.Code.BuffSystem.Profiles;
using Dimasyechka.Code.GameTimeFlowSystem.Controllers;
using Dimasyechka.Code.SaveLoadSystem.SaveLoadConverters;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.BuffSystem
{
    public class PlayerBuffsController : MonoBehaviour
    {
        [SerializeField]
        private BuffsContainer _playerBuffsContainer;
        public BuffsContainer PlayerBuffsContainer => _playerBuffsContainer;


        private GameTimeFlowController _gameTimeFlowController;

        [Inject]
        public void Construct(GameTimeFlowController gameTimeFlowController)
        {
            _gameTimeFlowController = gameTimeFlowController;
        }


        private void Start()
        {
            _gameTimeFlowController.onTimeHoursPassed += OnTimeHoursPassed;
        }

        private void OnDestroy()
        {
            _gameTimeFlowController.onTimeHoursPassed -= OnTimeHoursPassed;
        }


        private void OnTimeHoursPassed(int passedHours)
        {
            // За каждый прошедший час - обновляем действия баффов
            for (int i = 0; i < passedHours; i++)
            {
                UpdatePlayerBuffs();
            }
        }


        public void DisableAndRemoveAllBuffs()
        {
            _playerBuffsContainer.DisableAndRemoveAllBuffs();
        }

        public void LoadSaveBuffData(BuffSaveLoadData saveData)
        {
            foreach (RuntimeBuffSaveData buffSaveData in saveData.RuntimeBuffsSaveData)
            {
                _playerBuffsContainer.LoadBuff(BuffsWarehouse.Instance.GetBuffProfileByUid(buffSaveData.BuffUID), buffSaveData.BuffDurationHours);
            }
        }


        public void AddPlayerBuff(BuffProfile profile)
        {
            _playerBuffsContainer.AddBuff(profile);
        }

        public void RemovePlayerBuff(BuffProfile profile)
        {
            _playerBuffsContainer.RemoveBuff(profile);
        }

        public void UpdatePlayerBuffs()
        {
            _playerBuffsContainer.UpdateContainedBuffs();
        }
    }
}