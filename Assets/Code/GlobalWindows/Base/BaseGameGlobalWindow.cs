using Dimasyechka.Code.GlobalWindows.Interfaces;
using UnityEngine;

namespace Dimasyechka.Code.GlobalWindows.Base
{
    public class BaseGameGlobalWindow : MonoBehaviour, IGlobalWindow
    {
        private IGlobalWindowData _globalWindowData;
        public IGlobalWindowData GlobalWindowData => _globalWindowData;


        public T GetConvertedWindowData<T>() => (T)GlobalWindowData;


        private bool isShown;
        public bool IsShown => isShown;


        public void Hide()
        {
            OnHide();

            isShown = false;
            this.gameObject.SetActive(false);
        }
        protected virtual void OnHide() { }

        public void SetWindowData(IGlobalWindowData globalWindowData)
        {
            _globalWindowData = globalWindowData;
        }

        public void Show(IGlobalWindowData globalWindowData)
        {
            SetWindowData(globalWindowData);
            Show();
        }

        public void Show()
        {
            isShown = true;
            this.gameObject.SetActive(true);

            OnShow();
        }
        protected virtual void OnShow() { }
    }

    public class BaseGameGlobalWindowData : IGlobalWindowData
    {
        public string GlobalWindowTitle { get; set; } = "Базовое игровое окно";
    }
}