using Dimasyechka.Code.CinematicSystem.InputHandlers;
using Dimasyechka.Code.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Dimasyechka.Code.CinematicSystem.Views
{
    public class CinematicsKeyboardInputHandlerView : MonoBehaviour
    {
        [SerializeField]
        private FadeEffectController m_fadeEffectController;

        [SerializeField]
        private Image m_fillableImage;


        private CinematicsKeyboardInputHandler _handler;


        private void Awake()
        {
            _handler = FindObjectOfType<CinematicsKeyboardInputHandler>();

            if (_handler != null)
            {
                _handler.onForceSkip += OnForceSkip;
                _handler.onProgress += OnProgress;
                _handler.onPressed += OnPressed;

                _handler.onToggled += OnActiveStatusChanged;
            }

            m_fadeEffectController.ForceFadeOut();
        }

        private void OnActiveStatusChanged(bool status)
        {
            if (!status)
            {
                m_fadeEffectController.ForceFadeOut();
            }
        }

        private void OnDestroy()
        {
            if (_handler != null)
            {
                _handler.onForceSkip -= OnForceSkip;
                _handler.onProgress -= OnProgress;
                _handler.onPressed -= OnPressed;

                _handler.onToggled -= OnActiveStatusChanged;
            }
        }


        private void OnPressed(bool value)
        {
            if (value)
            {
                m_fadeEffectController.FadeIn();
            }
            else
            {
                m_fadeEffectController.ForceFadeOut();
            }
        }

        private void OnProgress(float progressRatio)
        {
            m_fillableImage.fillAmount = progressRatio;
        }

        private void OnForceSkip()
        {
            m_fadeEffectController.FadeOut();
        }
    }
}
