using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGlobalWindow
{
    public IGlobalWindowData globalWindowData { get; }

    public bool IsShown { get; }

    public void SetWindowData(IGlobalWindowData globalWindowData);

    public void Show(IGlobalWindowData globalWindowData);
    public void Show();
    public void Hide();
}

public interface IGlobalWindowData 
{
    public string GlobalWindowTitle { get; set; }
}