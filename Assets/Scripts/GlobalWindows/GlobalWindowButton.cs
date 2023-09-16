using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GlobalWindowButton : MonoBehaviour
{
    public UnityEvent onButtonClicked = new UnityEvent();


    [SerializeField]
    private TMP_Text m_buttonTitle;

    [SerializeField]
    private Button m_buttonReference;


    private void OnEnable()
    {
        m_buttonReference.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        m_buttonReference.onClick.RemoveListener(OnButtonClicked);
    }


    public void SetButtonTitle(string buttonTitle)
    {
        m_buttonTitle.text = buttonTitle;
    }

    private void OnButtonClicked()
    {
        onButtonClicked?.Invoke();
    }
}
