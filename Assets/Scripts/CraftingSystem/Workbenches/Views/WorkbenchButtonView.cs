using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class WorkbenchButtonView : MonoBehaviour, IPointerClickHandler
{
    [Header("Colorizer")]
    [SerializeField]
    private Image m_containerHighlighter;

    [SerializeField]
    private Color m_defaultColor;

    [SerializeField]
    private Color m_selectedColor;


    [Header("Inner References")]
    [SerializeField]
    protected Image m_containerIcon;

    [SerializeField]
    protected TMP_Text m_containerTitle;


    private CraftingWorkbenchProfile _profile;
    private Action<CraftingWorkbenchProfile> _handler;


    private bool isOpened;
    public bool IsOpened => isOpened;


    public void SetData(CraftingWorkbenchProfile profile, Action<CraftingWorkbenchProfile> callback)
    {
        _handler = callback;

        _profile = profile;

        m_containerIcon.sprite = _profile.WorkbenchMiniSprite;
    }

    public void SetOpenStatus(bool _isOpened)
    {
        isOpened = _isOpened;

        if (isOpened)
        {
            m_containerHighlighter.color = m_selectedColor;
        }
        else
        {
            m_containerHighlighter.color = m_defaultColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _handler?.Invoke(_profile);
    }
}
