using UnityEngine;


public class BaseGameGlobalWindow : MonoBehaviour, IGlobalWindow
{
    private BaseGameGlobalWindowData windowData;
    public IGlobalWindowData globalWindowData => windowData;


    private bool isShown;
    public bool IsShown => isShown;


    public void Hide()
    {
        isShown = false;
        this.gameObject.SetActive(false);
    }

    public void SetWindowData(IGlobalWindowData globalWindowData)
    {
        windowData = globalWindowData as BaseGameGlobalWindowData;
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
    }
}

public class BaseGameGlobalWindowData : IGlobalWindowData
{
    public string GlobalWindowTitle { get; set; } = "Базовое игровое окно";
}