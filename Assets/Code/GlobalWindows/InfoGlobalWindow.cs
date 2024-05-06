using System;
using Dimasyechka.Code.GlobalWindows.Base;
using Dimasyechka.Code.GlobalWindows.Interfaces;
using TMPro;
using UnityEngine;

namespace Dimasyechka.Code.GlobalWindows
{
    public class InfoGlobalWindow : BaseGameGlobalWindow
    {
        [SerializeField]
        private TMP_Text m_windowTitle;

        [SerializeField]
        private TMP_Text m_windowDescription;


        [SerializeField]
        private BaseGlobalWindowButton m_applyButton;


        private void OnEnable()
        {
            m_applyButton.onButtonClicked.AddListener(Apply);
        }

        private void OnDisable()
        {
            m_applyButton.onButtonClicked.RemoveListener(Apply);
        }

        protected override void OnShow()
        {
            InfoGlobalWindowData data = GetConvertedWindowData<InfoGlobalWindowData>();

            m_windowTitle.text = data.GlobalWindowTitle;
            m_windowDescription.text = data.InfoMessage;

            m_applyButton.SetButtonTitle(data.ApplyButtonText);
        }

        public void Apply()
        {
            if (GlobalWindowData != null)
            {
                GetConvertedWindowData<InfoGlobalWindowData>()?.OnApply();
            }

            Hide();
        }
    }

    public class InfoGlobalWindowData : IGlobalWindowData
    {
        public string GlobalWindowTitle { get; set; } = "Информация";

        public string InfoMessage = "Произошло какое-то событие";

        public Action OnApply = null;

        public string ApplyButtonText = "Принять";
    }
}