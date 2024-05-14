using Dimasyechka.Code.BattleSystem.Controllers;
using Dimasyechka.Code.BattleSystem.PlayerSystem;
using Dimasyechka.Code.BuffSystem;
using Dimasyechka.Code.BuffSystem.Views;
using Dimasyechka.Code.CinematicSystem;
using Dimasyechka.Code.CraftingSystem.Recipes;
using Dimasyechka.Code.CraftingSystem.Workbenches.Containers;
using Dimasyechka.Code.CraftingSystem.Workbenches.Views;
using Dimasyechka.Code.GameTimeFlowSystem.Controllers;
using Dimasyechka.Code.InventorySystem;
using Dimasyechka.Code.InventorySystem.BaseInventoryContainer;
using Dimasyechka.Code.InventorySystem.BaseMouse;
using Dimasyechka.Code.LocationSystem.Controllers;
using Dimasyechka.Code.SkillsSystem.Controllers;
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
        private LocationsController _locationsController;

        [SerializeField]
        private PlayerSkillsController _playerSkillsController;


        public override void InstallBindings()
        {
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
            BindPlayerSkills();
        }

        private void BindPlayerSkills()
        {
            Container.Bind<PlayerSkillsController>().FromInstance(_playerSkillsController).AsSingle();
            Container.Bind<SkillCoreFactory>().FromNew().AsSingle();
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
            Container.Bind<RuntimeBuffViewFactory>().FromNew().AsSingle();
        }

        private void BindCinematics()
        {
            Container.Bind<Cinematics>().FromInstance(_cinematics).AsSingle();
        }

        private void BindInventoryController()
        {
            Container.Bind<InventoryController>().FromInstance(_inventoryController).AsSingle();
            Container.Bind<BaseMouseItemController>().FromNew().AsSingle();

            Container.Bind<BaseInventoryContainerViewFactory>().FromNew().AsSingle();
            Container.Bind<BaseInventoryContainerSlotViewFactory>().FromNew().AsSingle();
            Container.Bind<BaseInventoryContainerButtonViewFactory>().FromNew().AsSingle();
        }

        private void BindCraftSystem()
        {
            Container.Bind<CraftSystem>().FromInstance(_craftSystem).AsSingle();
            Container.Bind<CraftInputItemViewFactory>().FromNew().AsSingle();
            Container.Bind<WorkbenchButtonViewFactory>().FromNew().AsSingle();
            Container.Bind<BaseWorkbenchRecipeViewFactory>().FromNew().AsSingle();
        }
    }
}
