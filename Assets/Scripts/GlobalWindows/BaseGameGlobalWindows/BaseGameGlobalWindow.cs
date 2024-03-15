using UnityEngine;


public class BaseGameGlobalWindow : MonoBehaviour, IGlobalWindow
{
    private BaseGameGlobalWindowData windowData;
    public IGlobalWindowData globalWindowData => windowData;


    public T GetConvertedWindowData<T>() => (T)globalWindowData;


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

        OnShow();
    }
    protected virtual void OnShow() { }
}

public class BaseGameGlobalWindowData : IGlobalWindowData
{
    public string GlobalWindowTitle { get; set; } = "Базовое игровое окно";
}