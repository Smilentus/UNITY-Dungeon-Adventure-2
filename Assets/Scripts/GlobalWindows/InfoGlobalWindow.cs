using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoGlobalWindow : MonoBehaviour, IGlobalWindow
{
    private InfoGlobalWindowData windowData;
    public IGlobalWindowData globalWindowData { get => windowData; }


    private bool m_isShown;
    public bool IsShown => m_isShown;


    [SerializeField]
    private TMP_Text m_windowTitle;

    [SerializeField]
    private TMP_Text m_windowDescription;


    [SerializeField]
    private GlobalWindowButton m_applyButton;


    private void OnEnable()
    {
        m_applyButton.onButtonClicked.AddListener(Apply);
    }

    private void OnDisable()
    {
        m_applyButton.onButtonClicked.RemoveListener(Apply);
    }


    public void SetWindowData(IGlobalWindowData globalWindowData)
    {
        if (globalWindowData.GetType().Equals(typeof(InfoGlobalWindowData)))
        {
            windowData = globalWindowData as InfoGlobalWindowData;

            m_windowTitle.text = windowData.GlobalWindowTitle;
            m_windowDescription.text = windowData.InfoMessage;

            m_applyButton.SetButtonTitle(windowData.ApplyButtonText);
        }
    }

    public void Show(IGlobalWindowData globalWindowData)
    {
        SetWindowData(globalWindowData);
        Show();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Apply()
    {
        windowData.OnApply();
        Hide();
    }
}

public class InfoGlobalWindowData : IGlobalWindowData
{
    public string GlobalWindowTitle { get; set; } = "Информация";

    public string InfoMessage = "Произошло какое-то событие";

    public Action OnApply;

    public string ApplyButtonText = "Принять";
}
