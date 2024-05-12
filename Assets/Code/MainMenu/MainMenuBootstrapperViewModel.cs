using Dimasyechka.Lubribrary.RxMV.Core;
using Dimasyechka.Lubribrary.RxMV.UniRx.Attributes;
using UniRx;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.MainMenu
{
    public class MainMenuBootstrapperViewModel : MonoViewModel<MainMenuBootstrapper>
    {
        [RxAdaptableProperty]
        public ReactiveProperty<string> GameVersion = new ReactiveProperty<string>();


        [Inject]
        public void Construct(MainMenuBootstrapper mainMenuBootstrapper)
        {
            ZenjectModel(mainMenuBootstrapper);
        }


        protected override void OnSetupModel()
        {
            GameVersion.Value = $"v{Application.version}";
        }

        [RxAdaptableMethod]
        public void OnNewGameButtonClicked()
        {
            Model.StartNewGame();
        }

        [RxAdaptableMethod]
        public void OnContinueButtonClicked()
        {
            Model.ContinueGame();
        }

        [RxAdaptableMethod]
        public void OnLoadSaveButtonClicked()
        {
            Model.LoadSaveGame();
        }

        [RxAdaptableMethod]
        public void OnSettingsButtonClicked()
        {
            // ...
        }

        [RxAdaptableMethod]
        public void OnExitButtonClicked()
        {
            Model.ExitGame();
        }
    }
}
