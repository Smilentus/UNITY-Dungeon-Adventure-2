using System;
using System.Collections.Generic;
using Dimasyechka.Code.GlobalWindows.Base;
using Dimasyechka.Code.GlobalWindows.Interfaces;
using UnityEngine;

namespace Dimasyechka.Code.GlobalWindows.Controllers
{
    public class GlobalWindowsController : MonoBehaviour
    {
        private static GlobalWindowsController _instance;
        public static GlobalWindowsController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<GlobalWindowsController>(); 
                }

                return _instance;
            }
        }


        [Tooltip("���� ���������� ��� ����, ������������ �� �����")]
        [SerializeField]
        private List<GameObject> _windowGameObjects = new List<GameObject>();


        private List<IGlobalWindow> _globalWindows = new List<IGlobalWindow>();

        private List<BaseGameGlobalWindow> _baseGameGlobalWindows = new List<BaseGameGlobalWindow>();


        private void Awake()
        {
            InitializeGlobalWindows();
        }


        private void InitializeGlobalWindows()
        {
            foreach (GameObject gameObject in _windowGameObjects)
            {
                IGlobalWindow window;
                if (gameObject.TryGetComponent<IGlobalWindow>(out window))
                {
                    _globalWindows.Add(window);
                }

                BaseGameGlobalWindow baseGameGlobalWindow;
                if (gameObject.TryGetComponent<BaseGameGlobalWindow>(out baseGameGlobalWindow))
                {
                    _baseGameGlobalWindows.Add(baseGameGlobalWindow);
                }
            }
        }


        public void TryShowGlobalWindow(Type globalWindowType, IGlobalWindowData globalWindowData = null)
        {
            IGlobalWindow windowToOpen = _globalWindows.Find(x => x.GetType().Equals(globalWindowType));

            if (windowToOpen != null)
            {
                windowToOpen.Show(globalWindowData);
            }
        }

        public bool IsWindowShown(Type globalWindowType)
        {
            IGlobalWindow windowToOpen = _globalWindows.Find(x => x.GetType().Equals(globalWindowType));

            if (windowToOpen != null)
            {
                return windowToOpen.IsShown;
            }

            return false;
        }

        public void TryHideGlobalWindow(Type globalWindowType)
        {
            IGlobalWindow windowToClose = _globalWindows.Find(x => x.GetType().Equals(globalWindowType));

            if (windowToClose != null)
            {
                windowToClose.Hide();
            }
        }


        public void TryToggleGlobalWindow(Type globalWindowType, IGlobalWindowData globalWindowData = null)
        {
            if (IsWindowShown(globalWindowType))
            {
                TryHideGlobalWindow(globalWindowType);  
            }
            else
            {
                TryShowGlobalWindow(globalWindowType, globalWindowData);
            }
        }


        public void CloseEveryBaseGameGlobalWindow()
        {
            foreach (BaseGameGlobalWindow window in _baseGameGlobalWindows)
            {
                window.Hide();
            }
        }

        public void CloseEveryBaseGameGlobalWindowExceptOne(Type globalWindowType)
        {
            foreach (BaseGameGlobalWindow window in _baseGameGlobalWindows)
            {
                if (window.GetType().Equals(globalWindowType)) continue;
            
                window.Hide();
            }
        }
    }
}