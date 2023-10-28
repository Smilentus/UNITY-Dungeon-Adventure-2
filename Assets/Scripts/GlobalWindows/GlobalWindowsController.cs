using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalWindowsController : MonoBehaviour
{
    private static GlobalWindowsController instance;
    public static GlobalWindowsController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<GlobalWindowsController>(); 
            }

            return instance;
        }
    }


    [Tooltip("—юда закидываем все окна, существующие на сцене")]
    [SerializeField]
    private List<GameObject> windowGameObjects = new List<GameObject>();


    private List<IGlobalWindow> globalWindows = new List<IGlobalWindow>();

    private List<BaseGameGlobalWindow> baseGameGlobalWindows = new List<BaseGameGlobalWindow>();


    private void Awake()
    {
        InitializeGlobalWindows();
    }


    private void InitializeGlobalWindows()
    {
        foreach (GameObject gameObject in windowGameObjects)
        {
            IGlobalWindow window;
            if (gameObject.TryGetComponent<IGlobalWindow>(out window))
            {
                globalWindows.Add(window);
            }

            BaseGameGlobalWindow baseGameGlobalWindow;
            if (gameObject.TryGetComponent<BaseGameGlobalWindow>(out baseGameGlobalWindow))
            {
                baseGameGlobalWindows.Add(baseGameGlobalWindow);
            }
        }
    }


    public void TryShowGlobalWindow(Type globalWindowType, IGlobalWindowData globalWindowData = null)
    {
        IGlobalWindow windowToOpen = globalWindows.Find(x => x.GetType().Equals(globalWindowType));

        if (windowToOpen != null)
        {
            windowToOpen.Show(globalWindowData);
        }
    }

    public bool IsWindowShown(Type globalWindowType)
    {
        IGlobalWindow windowToOpen = globalWindows.Find(x => x.GetType().Equals(globalWindowType));

        if (windowToOpen != null)
        {
            return windowToOpen.IsShown;
        }

        return false;
    }

    public void TryHideGlobalWindow(Type globalWindowType)
    {
        IGlobalWindow windowToClose = globalWindows.Find(x => x.GetType().Equals(globalWindowType));

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
        foreach (BaseGameGlobalWindow window in baseGameGlobalWindows)
        {
            window.Hide();
        }
    }

    public void CloseEveryBaseGameGlobalWindowExceptOne(Type globalWindowType)
    {
        foreach (BaseGameGlobalWindow window in baseGameGlobalWindows)
        {
            if (window.GetType().Equals(globalWindowType)) continue;
            
            window.Hide();
        }
    }
}
