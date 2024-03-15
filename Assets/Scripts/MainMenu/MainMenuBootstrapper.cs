using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;


public class MainMenuBootstrapper : MonoBehaviour
{
    private static MainMenuBootstrapper _instance;
    public static MainMenuBootstrapper Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MainMenuBootstrapper>();
            }

            return _instance;
        }
    }



    [SerializeField]
    private BetweenScenesLoadableData m_loadableData;
    public BetweenScenesLoadableData LoadableData => m_loadableData;


    private void Awake()
    {
        SaveLoadSlotsController.Instance.LoadSaveSlots();
    }


    public void StartNewGame()
    {
        m_loadableData.SelectedSaveFileFullPath = "";

        SceneManager.LoadScene("GameScene");
    }

    public void ContinueGame()
    {
        // TODO: ...
        // Тут надо получать последнее сделанное сохранение и загружать его 
        m_loadableData.SelectedSaveFileFullPath = SaveLoadSlotsController.Instance.GetAutoSaveFullFilePath();

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
            new AcceptGlobalWindowData() { 
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
