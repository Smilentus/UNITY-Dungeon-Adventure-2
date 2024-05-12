using Dimasyechka.Code.MainMenu;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.MonoInstallers
{
    public class MainMenuMonoInstaller : MonoInstaller
    {
        [SerializeField]
        private MainMenuBootstrapper _bootstrapper;


        public override void InstallBindings()
        {
            BindMainMenuBootstrapper();
        }

        private void BindMainMenuBootstrapper()
        {
            Container.Bind<MainMenuBootstrapper>().FromInstance(_bootstrapper).AsSingle();
        }
    }
}
