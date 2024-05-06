using System;
using UnityEngine;
using UnityEngine.UI;

namespace Dimasyechka.Code.GlobalWindows.Invokers
{
    [RequireComponent(typeof(Button), typeof(Image))]
    public class GameGlobalWindowInvoker : MonoBehaviour
    {
        [SerializeField]
        private Color defaultColor;

        [SerializeField]
        private Color highlightedColor;


        private Button buttonReference;
        private Image imageReference;


        private Action pressCallback;


        private void Awake()
        {
            buttonReference = GetComponent<Button>();
            buttonReference.onClick.AddListener(OnPressed);

            imageReference = GetComponent<Image>();
            imageReference.color = defaultColor;
        }

        private void OnDestroy()
        {
            buttonReference.onClick.RemoveListener(OnPressed);
        }


        public void SetPressCallback(Action _callback)
        {
            pressCallback = _callback;
        }

        public void SetHighlighted(bool value)
        {
            if (value)
            {
                imageReference.color = highlightedColor;
            }
            else
            {
                imageReference.color = defaultColor;
            }
        }

        private void OnPressed()
        {
            if (pressCallback != null)
                pressCallback();
        }
    }
}
