using System;
using Dimasyechka.Code.GlobalWindows.Base;
using Dimasyechka.Code.GlobalWindows.Interfaces;
using TMPro;
using UnityEngine;

namespace Dimasyechka.Code.GlobalWindows
{
    public class AcceptGlobalWindow : BaseGameGlobalWindow
    {
        [SerializeField]
        private TMP_Text m_windowTitle;

        [SerializeField]
        private TMP_Text m_windowDescription;


        [SerializeField]
        private BaseGlobalWindowButton m_applyButton;

        [SerializeField]
        private BaseGlobalWindowButton m_closeButton;


        private void OnEnable()
        {
            m_applyButton.onButtonClicked.AddListener(Apply);
            m_closeButton.onButtonClicked.AddListener(Close);
        }

        private void OnDisable()
        {
            m_applyButton.onButtonClicked.RemoveListener(Apply);
            m_closeButton.onButtonClicked.RemoveListener(Close);
        }


        protected override void OnShow()
        {
            AcceptGlobalWindowData data = GetConvertedWindowData<AcceptGlobalWindowData>();

            m_windowTitle.text = data.GlobalWindowTitle;
            m_windowDescription.text = data.GlobalWindowDescription;

            m_applyButton.SetButtonTitle(data.ApplyButtonText);
            m_closeButton.SetButtonTitle(data.CancelButtonText);
        }


        public void Apply()
        {
            AcceptGlobalWindowData data = GetConvertedWindowData<AcceptGlobalWindowData>();
            if (data != null)
            {
                data?.OnApply?.Invoke();
            }

            Hide();
        }

        public void Close()
        {
            AcceptGlobalWindowData data = GetConvertedWindowData<AcceptGlobalWindowData>();
            if (data != null)
            {
                data?.OnCancel?.Invoke();
            }

            Hide();
        }
    }

    public class AcceptGlobalWindowData : IGlobalWindowData
    {
        public string GlobalWindowTitle { get; set; } = "Подтверждение";
        public string GlobalWindowDescription { get; set; } = "Подтвердить действие?";

        public Action OnApply = null;
        public Action OnCancel = null;

        public string ApplyButtonText = "Подтвердить";
        public string CancelButtonText = "Отменить";
    }
}