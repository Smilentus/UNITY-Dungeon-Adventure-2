using System.Collections.Generic;
using Dimasyechka.Code.SkillsSystem.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dimasyechka.Code.SkillsSystem.Views
{
    public class ObtainSkillDataView : MonoBehaviour
    {
        [SerializeField]
        private Image m_skillPreview;

        [SerializeField]
        private TMP_Text m_skillLevel;

        [SerializeField]
        private TMP_Text m_skillTitle;

        [SerializeField]
        private TMP_Text m_skillDescription;


        // Почему так некрасиво? =/
        public void SetData(SkillLevelData skillLevelData, int skillLevel, List<string> deltaValues = null) 
        {
            if (skillLevelData == null)
            {
                if (m_skillPreview != null) m_skillPreview.enabled = false;
                if (m_skillLevel != null) m_skillLevel.enabled = false;
                if (m_skillTitle != null) m_skillTitle.enabled = false;
                if (m_skillDescription != null) m_skillDescription.enabled = false;
            }
            else
            {
                if (m_skillPreview != null) m_skillPreview.enabled = true;
                if (m_skillLevel != null) m_skillLevel.enabled = true;
                if (m_skillTitle != null) m_skillTitle.enabled = true;
                if (m_skillDescription != null) m_skillDescription.enabled = true;

                if (m_skillPreview != null) m_skillPreview.sprite = skillLevelData.skillLevelIcon;
                if (m_skillLevel != null) m_skillLevel.text = $"{skillLevel}";
                if (m_skillTitle != null) m_skillTitle.text = $"{skillLevelData.skillLevelTitle}";
                if (m_skillDescription != null)
                {
                    m_skillDescription.text = $"{skillLevelData.skillLevelDescription}";

                    if (deltaValues != null)
                    {
                        m_skillDescription.text += $"\n\n";

                        foreach (string deltaValue in deltaValues)
                        {
                            m_skillDescription.text += $"{deltaValue}\n";
                        }
                    }
                }
            }
        }
    }
}