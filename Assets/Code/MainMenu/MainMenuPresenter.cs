using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dimasyechka.Code.MainMenu
{
    public class MainMenuPresenter : MonoBehaviour
    {
        [SerializeField]
        private MainMenuBootstrapper _bootstrapper;


        [SerializeField]
        private TMP_Text m_gameVersionTMP;


        [SerializeField]
        private Button m_newGameButton;

        [SerializeField]
        private Button m_continueButton;

        [SerializeField]
        private Button m_loadSaveButton;

        [SerializeField]
        private Button m_settingsButton;

        [SerializeField]
        private Button m_exitGameButton;


        private void Awake()
        {
            m_newGameButton?.onClick.AddListener(OnNewGameButtonClicked);
            m_continueButton?.onClick.AddListener(OnContinueButtonClicked);
            m_loadSaveButton?.onClick.AddListener(OnLoadSaveButtonClicked);
            m_settingsButton?.onClick.AddListener(OnSettingsButtonClicked);
            m_exitGameButton.onClick.AddListener(OnExitButtonClicked);

            if (m_gameVersionTMP != null)
            {
                m_gameVersionTMP.text = $"v{Application.version}";
            }
        }


        private void OnNewGameButtonClicked()
        {
            _bootstrapper.StartNewGame();
        }

        private void OnContinueButtonClicked()
        {
            _bootstrapper.ContinueGame();
        }

        private void OnLoadSaveButtonClicked()
        {
            _bootstrapper.LoadSaveGame();
        }

        private void OnSettingsButtonClicked()
        {
            // ...
        }

        private void OnExitButtonClicked()
        {
            _bootstrapper.ExitGame();
        }
    }
}
