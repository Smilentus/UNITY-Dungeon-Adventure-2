using TMPro;
using UnityEngine;


public class SkillUpgradeDescriptionView : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_descriptionTMP;

    [SerializeField]
    private Color m_notEnoughColor;

    [SerializeField]
    private Color m_accomplishedColor;


    public void SetData(SkillUpgradeDescriptionData description)
    {
        m_descriptionTMP.text = description.Description;
    
        m_descriptionTMP.color = description.IsAccomplished ? m_accomplishedColor : m_notEnoughColor;
    }
}
