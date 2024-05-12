using Dimasyechka.Code.GlobalWindows;
using Dimasyechka.Code.GlobalWindows.Controllers;
using Dimasyechka.Code.SaveLoadSystem.BetweenScenes;
using Dimasyechka.Code.SaveLoadSystem.Controllers;
using Dimasyechka.Code.SaveLoadSystem.GlobalWindow;
using Dimasyechka.Code.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Dimasyechka.Code.MainMenu
{
    public class MainMenuBootstrapper : MonoBehaviour
    {
        private SaveLoadSlotsController _saveLoadSlotsController;

        [Inject]
        public void Construct(SaveLoadSlotsController saveLoadSlotsController)
        {
            _saveLoadSlotsController = saveLoadSlotsController;
        }


        private void Awake()
        {
            _saveLoadSlotsController.LoadSaveSlots();
            ScreenFader.Instance.FadeOutScreen();
        }


        public void StartNewGame()
        {
            BetweenScenesLoaderAdapter.Instance.LoadableData.SelectedSaveFileFullPath = "";

            SceneManager.LoadScene("GameScene");
        }

        public void ContinueGame()
        {
            // TODO: ...
            // Тут надо получать последнее сделанное сохранение и загружать его 
            BetweenScenesLoaderAdapter.Instance.LoadableData.SelectedSaveFileFullPath = _saveLoadSlotsController.GetAutoSaveFullFilePath();

            SceneManager.LoadScene("GameScene");
        }

        public void LoadSaveGame()
        {
            GlobalWindowsController.Instance.TryShowGlobalWindow(typeof(SaveLoadSlotsGlobalWindow));
        }

        public void ExitGame()
        {
            GlobalWindowsController.Instance.TryShowGlobalWindow(
                typeof(AcceptGlobalWindow),
                new AcceptGlobalWindowData()
                {
                    ApplyButtonText = "Выйти из игры",
                    CancelButtonText = "Остаться в игре",
                    GlobalWindowTitle = "Подтверждение выхода",
                    GlobalWindowDescription = "Уже покидаете нас?",
                    OnApply = OnExitApplied
                });
        }

        private void OnExitApplied()
        {
            Application.Quit();
        }
    }
}
