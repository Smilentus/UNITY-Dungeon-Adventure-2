using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AcceptGlobalWindow : MonoBehaviour, IGlobalWindow
{
    private AcceptGlobalWindowData windowData;
    public IGlobalWindowData globalWindowData { get => windowData; }


    private bool m_isShown;
    public bool IsShown => m_isShown;


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

    public void SetWindowData(IGlobalWindowData globalWindowData)
    {
        if (globalWindowData.GetType().Equals(typeof(AcceptGlobalWindowData)))
        {
            windowData = globalWindowData as AcceptGlobalWindowData;

            m_windowTitle.text = windowData.GlobalWindowTitle;
            m_windowDescription.text = windowData.GlobalWindowDescription;

            m_applyButton.SetButtonTitle(windowData.ApplyButtonText);
            m_closeButton.SetButtonTitle(windowData.CancelButtonText);
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

    public void Close()
    {
        windowData.OnCancel();
        Hide();
    }
}

public class AcceptGlobalWindowData : IGlobalWindowData
{
    public string GlobalWindowTitle { get; set; } = "Подтверждение";
    public string GlobalWindowDescription { get; set; } = "Подтвердить действие?";

    public Action OnApply;
    public Action OnCancel;

    public string ApplyButtonText = "Подтвердить";
    public string CancelButtonText = "Отменить";
}
