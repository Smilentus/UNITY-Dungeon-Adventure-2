using Dimasyechka.Code.BattleSystem.Controllers;
using Dimasyechka.Code.BattleSystem.PlayerSystem;
using Dimasyechka.Code.BuffSystem;
using Dimasyechka.Code.CinematicSystem;
using Dimasyechka.Code.CraftingSystem.Recipes;
using Dimasyechka.Code.GameTimeFlowSystem.Controllers;
using Dimasyechka.Code.InventorySystem;
using Dimasyechka.Code.LocationSystem.Controllers;
using Dimasyechka.Code.SaveLoadSystem.Controllers;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.MonoInstallers
{
    public class ServicesMonoInstaller : MonoInstaller
    {
        [Header("Instances")]
        [SerializeField]
        private GameController _gameController;

        [SerializeField]
        private BattleController _battleController;

        [SerializeField]
        private RuntimeBattlePlayerController _runtimeBattlePlayerController;

        [SerializeField]
        private RuntimePlayer _runtimePlayer;

        [SerializeField]
        private GameTimeFlowController _gameTimeFlowController;

        [SerializeField]
        private PlayerBuffsController _playerBuffsController;

        [SerializeField]
        private Cinematics _cinematics;

        [SerializeField]
        private InventoryController _inventoryController;

        [SerializeField]
        private CraftSystem _craftSystem;

        [SerializeField]
        private SaveLoadSystemController _saveLoadSystemController;

        [SerializeField]
        private LocationsController _locationsController;


        public override void InstallBindings()
        {
            BindSaveLoadSystem();
            BindCraftSystem();
            BindInventoryController();
            BindCinematics();
            BindPlayerBuffsController();
            BindGameTimeFlowController();
            BindRuntimePlayer();
            BindRuntimeBattlePlayerController();
            BindBattleController();
            BindGameController();
            BindLocationsController();
        }

        private void BindLocationsController()
        {
            Container.Bind<LocationsController>().FromInstance(_locationsController).AsSingle();
        }

        private void BindGameController()
        {
            Container.Bind<GameController>().FromInstance(_gameController).AsSingle();
        }

        private void BindBattleController()
        {
            Container.Bind<BattleController>().FromInstance(_battleController).AsSingle();
            Container.Bind<RuntimeBattleCharacterFactory>().FromNew().AsSingle();
            Container.Bind<RuntimeBattleActionFactory>().FromNew().AsSingle();
        }

        private void BindRuntimeBattlePlayerController()
        {
            Container.Bind<RuntimeBattlePlayerController>().FromInstance(_runtimeBattlePlayerController).AsSingle();
        }

        private void BindRuntimePlayer()
        {
            Container.Bind<RuntimePlayer>().FromInstance(_runtimePlayer).AsSingle();
        }

        private void BindGameTimeFlowController()
        {
            Container.Bind<GameTimeFlowController>().FromInstance(_gameTimeFlowController).AsSingle();
        }

        private void BindPlayerBuffsController()
        {
            Container.Bind<PlayerBuffsController>().FromInstance(_playerBuffsController).AsSingle();
        }

        private void BindCinematics()
        {
            Container.Bind<Cinematics>().FromInstance(_cinematics).AsSingle();
        }

        private void BindInventoryController()
        {
            Container.Bind<InventoryController>().FromInstance(_inventoryController).AsSingle();
        }

        private void BindCraftSystem()
        {
            Container.Bind<CraftSystem>().FromInstance(_craftSystem).AsSingle();
        }

        private void BindSaveLoadSystem()
        {
            Container.Bind<SaveLoadSystemController>().FromInstance(_saveLoadSystemController).AsSingle();
        }
    }
}
