using Dimasyechka.Code.BattleSystem.Controllers;
using Dimasyechka.Code.GameTimeFlowSystem.Controllers;
using Dimasyechka.Code.GlobalWindows;
using Dimasyechka.Code.GlobalWindows.Controllers;
using Dimasyechka.Code.LocationSystem.Controllers;
using Dimasyechka.Code.LocationSystem.Profiles;
using Dimasyechka.Code.SaveLoadSystem.BetweenScenes;
using Dimasyechka.Code.SaveLoadSystem.Controllers;
using Dimasyechka.Code.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


// TODO: Refactoring
namespace Dimasyechka.Code
{
    public class GameController : MonoBehaviour
    {
        private static GameController instance;
        public static GameController Instance
        { 
            get 
            { 
                if (instance == null)
                {
                    instance = FindObjectOfType<GameController>();
                }

                return instance; 
            }
        }


        [Header("Префаб информационного текста")]
        public GameObject EventText;
        [Header("Родитель для евентов")]
        public Transform EventParent;
        [Header("Панель скролов")]
        public ScrollRect eventScroll;


        [Header("Окно информации")]
        public GameObject Blocker;
        public GameObject DeathBox;

        // Куда-то вынести отсюдАВА
        private void Awake()
        {
            instance = this;


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
                    Blocker.SetActive(false);
                    GlobalWindowsController.Instance.TryHideGlobalWindow(typeof(AcceptGlobalWindow));
                }
                else
                {
                    Blocker.SetActive(true);
                    GlobalWindowsController.Instance.TryShowGlobalWindow(typeof(AcceptGlobalWindow), new AcceptGlobalWindowData() {
                        GlobalWindowTitle = "Информация",
                        GlobalWindowDescription = "Вы действительно хотите выйти в меню?",
                        ApplyButtonText = "Принять",
                        CancelButtonText = "Отменить",
                        OnApply = ExitToMainMenu
                    });
                }
            }
        }


        // Добавление текста в панель ивентов
        public void AddEventText(string text)
        {
            // Если текстовых полей >= Х, то удаляем
            if (EventParent.childCount >= 512)
            {
                for (int i = 512; i < EventParent.childCount; i++)
                {
                    Destroy(EventParent.GetChild(i).gameObject);
                }
            }

            GameObject newText = Instantiate(EventText, EventParent);
            newText.GetComponent<Text>().text = "[" + GameTimeFlowController.Instance.DateNow() + "]\n" + text + "\n";
            newText.transform.SetAsFirstSibling();

            eventScroll.normalizedPosition = new Vector2(eventScroll.normalizedPosition.x, 1);
        }

        // Отображение информационного окна
        /// <summary>
        /// 0 - Событие. 1 - Информация.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="action"></param>
        public void ShowMessageText(string message, string title = "[Информация]")
        {
            Blocker.SetActive(true);

            GlobalWindowsController.Instance.TryShowGlobalWindow(typeof(InfoGlobalWindow), new InfoGlobalWindowData() { 
                GlobalWindowTitle = title,
                InfoMessage = message,
                ApplyButtonText = "Принять",
                OnApply = HideMessageText
            });
        }
        // Закрытие информационного окна
        public void HideMessageText()
        {
            Blocker.SetActive(false);
        }

        // DeathBox открытие
        public void ShowDeathBox(string deathText)
        {
            RuntimePlayer.Instance.RuntimePlayerStats.isDeath = true;
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

        // Действие при нажатии на кнопку локации
        public void PressDirectionButton(LocationProfile locationToTravel)
        {
            LocationsController.Instance.TravelToLocation(locationToTravel, 0);
        }

        // Ожидание X часов
        public void WaitSomeTime(int timeToWait)
        {
            if (!BattleController.Instance.IsBattle)
            {
                // Время баффов
                //FindObjectOfType<BuffManager>().ChangeBuffActionTime(timeToWait);

                // Добавляем время
                if (timeToWait < 0)
                    timeToWait *= -1;

                GameTimeFlowController.Instance.AddTime(timeToWait);
            }
        }
        // --------------
    }
}