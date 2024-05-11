using Dimasyechka.Code.BattleSystem.Controllers;
using Dimasyechka.Code.GameTimeFlowSystem.Controllers;
using Dimasyechka.Code.GlobalWindows;
using Dimasyechka.Code.GlobalWindows.Controllers;
using Dimasyechka.Code.SaveLoadSystem.BetweenScenes;
using Dimasyechka.Code.SaveLoadSystem.Controllers;
using Dimasyechka.Code.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;


// TODO: Refactoring
namespace Dimasyechka.Code
{
    public class GameController : MonoBehaviour
    {
        private static GameController _instance;
        public static GameController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameController>();
                }

                return _instance;
            }
        }


        public GameObject DeathBox;


        private BattleController _battleController;
        private GameTimeFlowController _gameTimeFlowController;


        [Inject]
        public void Construct(
            BattleController battleController,
            GameTimeFlowController gameTimeFlowController)
        {
            _battleController = battleController;
            _gameTimeFlowController = gameTimeFlowController;
        }


        // Куда-то вынести отсюдАВА
        private void Awake()
        {
            _instance = this;


            BetweenScenesLoadableData data = BetweenScenesLoaderAdapter.Instance.LoadableData;

            if (data == null ||
                string.IsNullOrEmpty(data.SelectedSaveFileFullPath)
               ) return;

            Debug.Log($"[BetweenScenesLoaderAdapter] --> Попытка загрузить сохранение: '{data.SelectedSaveFileFullPath}'");

            SaveLoadSystemController.Instance.TryLoadAndParseDataFromFile(data.SelectedSaveFileFullPath);
        }

        private void Start()
        {
            ScreenFader.Instance.FadeOutScreen();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GlobalWindowsController.Instance.IsWindowShown(typeof(AcceptGlobalWindow)))
                {
                    GlobalWindowsController.Instance.TryHideGlobalWindow(typeof(AcceptGlobalWindow));
                }
                else
                {
                    GlobalWindowsController.Instance.TryShowGlobalWindow(typeof(AcceptGlobalWindow), new AcceptGlobalWindowData()
                    {
                        GlobalWindowTitle = "Информация",
                        GlobalWindowDescription = "Вы действительно хотите выйти в меню?",
                        ApplyButtonText = "Принять",
                        CancelButtonText = "Отменить",
                        OnApply = ExitToMainMenu
                    });
                }
            }
        }


        public void ShowMessageText(string message, string title = "[Информация]")
        {
            GlobalWindowsController.Instance.TryShowGlobalWindow(typeof(InfoGlobalWindow), new InfoGlobalWindowData()
            {
                GlobalWindowTitle = title,
                InfoMessage = message,
                ApplyButtonText = "Принять",
                OnApply = HideMessageText
            });
        }

        public void HideMessageText()
        {
            // ...
        }

        public void ShowDeathBox(string deathText)
        {
            DeathBox.SetActive(true);
            DeathBox.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = deathText;
        }

        public void ExitToMainMenu()
        {
            GlobalWindowsController.Instance.TryShowGlobalWindow(typeof(AcceptGlobalWindow), new AcceptGlobalWindowData()
            {
                GlobalWindowTitle = "Выход в меню",
                GlobalWindowDescription = "Вы действительно хотите покинуть этот прекрасный игровой мир и вернуться в меню?",
                ApplyButtonText = "Выйти в меню",
                CancelButtonText = "Вернуться в игру",
                OnApply = OnExitApplied
            });

            void OnExitApplied()
            {
                SaveLoadSystemController.Instance.TrySaveGameState("AutoSave");
                ScreenFader.Instance.FadeInScreen();

                ScreenFader.Instance.FadeEffectController.onFadeIn += OnFadeIn;
            }

            // Ладно пускай будет так...
            void OnFadeIn()
            {
                ScreenFader.Instance.FadeEffectController.onFadeIn -= OnFadeIn;

                SceneManager.LoadScene("MenuScene");
            }
        }

        public void LoadAutoSave()
        {
            BetweenScenesLoaderAdapter.Instance.LoadableData.SelectedSaveFileFullPath = SaveLoadSlotsController.Instance.GetAutoSaveFullFilePath();

            SceneManager.LoadScene("GameScene");
        }

        public void WaitSomeTime(int timeToWait)
        {
            if (!_battleController.IsBattle)
            {
                // Время баффов
                //FindObjectOfType<BuffManager>().ChangeBuffActionTime(timeToWait);

                // Добавляем время
                if (timeToWait < 0)
                    timeToWait *= -1;

                _gameTimeFlowController.AddTime(timeToWait);
            }
        }
    }
}