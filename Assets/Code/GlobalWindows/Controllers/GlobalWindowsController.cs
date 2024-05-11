using Dimasyechka.Code.GlobalWindows.Base;
using Dimasyechka.Code.GlobalWindows.Interfaces;
using System;
using System.Collections.Generic;
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


        [Tooltip("—юда закидываем все окна, существующие на сцене")]
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


        public void CloseEveryWindow()
        {
            foreach (BaseGameGlobalWindow window in _baseGameGlobalWindows)
            {
                window.Hide();
            }
        }

        public void CloseEveryWindowAmongThoseTypes(Type[] globalWindowTypes)
        {
            foreach (BaseGameGlobalWindow window in _baseGameGlobalWindows)
            {
                foreach (Type windowType in globalWindowTypes)
                {
                    if (IsWindowShown(windowType))
                    {
                        TryHideGlobalWindow(windowType);
                    }
                }
            }
        }

        public void CloseEveryWindowAmongThoseTypesExceptOne(Type[] globalWindowTypes, Type exceptWindowType)
        {
            foreach (BaseGameGlobalWindow window in _baseGameGlobalWindows)
            {
                foreach (Type windowType in globalWindowTypes)
                {
                    if (windowType == exceptWindowType) continue;

                    if (IsWindowShown(windowType))
                    {
                        TryHideGlobalWindow(windowType);
                    }
                }
            }
        }

        public void CloseEveryWindowExceptOne(Type globalWindowType)
        {
            foreach (BaseGameGlobalWindow window in _baseGameGlobalWindows)
            {
                if (window.GetType().Equals(globalWindowType)) continue;

                window.Hide();
            }
        }


        [ContextMenu("CollectAllGlobalWindowsAtScene")]
        public void CollectAllGlobalWindowsAtScene()
        {
            _windowGameObjects.Clear();

            BaseGameGlobalWindow[] globalWindows = FindObjectsOfType<BaseGameGlobalWindow>(true);

            for (int i = 0; i < globalWindows.Length; i++)
            {
                _windowGameObjects.Add(globalWindows[i].gameObject);
            }
        }
    }
}
