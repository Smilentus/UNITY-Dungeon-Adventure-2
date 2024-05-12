using Dimasyechka.Code.SaveLoadSystem.Controllers;
using Dimasyechka.Code.SaveLoadSystem.GlobalWindow;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.MonoInstallers
{
    public class SaveSystemMonoInstaller : MonoInstaller
    {
        [SerializeField]
        private SaveLoadSystemController _saveLoadSystemController;

        [SerializeField]
        private SaveLoadSlotsController _saveLoadSlotsController;


        public override void InstallBindings()
        {
            BindSaveLoadSystem();
        }


        private void BindSaveLoadSystem()
        {
            Container.Bind<SaveLoadSystemController>().FromInstance(_saveLoadSystemController).AsSingle();
            Container.Bind<SaveLoadSlotsController>().FromInstance(_saveLoadSlotsController).AsSingle();

            Container.Bind<SaveLoadSlotViewFactory>().FromNew().AsSingle();
        }
    }
}
