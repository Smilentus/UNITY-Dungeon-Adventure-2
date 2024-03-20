using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class RuntimeBuffView : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_buffTitleTMP;

    [SerializeField]
    private Image m_buffIcon;

    [SerializeField]
    private TMP_Text m_buffDurationTMP;


    private int maxDurationHours;


    public void SetTitleAndIcon(string _title, Sprite _icon)
    {
        if (m_buffTitleTMP != null)
        {
            m_buffTitleTMP.text = $"{_title}";
        }

        if (m_buffIcon != null)
        {
            m_buffIcon.sprite = _icon;
        }
    }

    public void SetDuration(int _durationHours)
    {
        if (m_buffDurationTMP != null)
        {
            m_buffDurationTMP.text = $"{_durationHours}";
        }
    }

    public void SetMaxDuration(int _maxDurationHours)
    {
        maxDurationHours = _maxDurationHours;
    }
}
