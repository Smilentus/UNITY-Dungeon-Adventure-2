using Dimasyechka.Code.SkillsSystem.Controllers;
using Dimasyechka.Code.SkillsSystem.Core;
using Dimasyechka.Code.SkillsSystem.Interactions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dimasyechka.Code.SkillsSystem.Views
{
    public class ObtainSkillInteractionView : MonoBehaviour
    {
        [SerializeField]
        private ObtainSkillInteraction m_obtainSkillInteraction;


        [SerializeField]
        private TMP_Text m_skillTitle;

        [SerializeField]
        private Image m_skillIcon;


        private void Awake()
        {
            if (m_obtainSkillInteraction == null)
            {
                m_obtainSkillInteraction = GetComponent<ObtainSkillInteraction>();
            }
        }


        private void Start()
        {
            PlayerSkillsController.instance.onSkillUpgraded += OnSkillUpgraded;

            PlayerSkill playerSkill = PlayerSkillsController.instance.GetPlayerSkillByGuid(m_obtainSkillInteraction.SkillProfile.skillGUID);

            if (playerSkill == null)
            {
                UpdateSkillData(m_obtainSkillInteraction.SkillProfile, 0);
            }
            else
            {
                UpdateSkillData(m_obtainSkillInteraction.SkillProfile, playerSkill.RuntimeSkillCore.UpgradeableComponent.currentLevel);
            }
        }

        private void OnDestroy()
        {
            if (PlayerSkillsController.instance != null)
            {
                PlayerSkillsController.instance.onSkillUpgraded -= OnSkillUpgraded;
            }
        }


        // TODO: Переделать по другой архитектуре
        private void OnSkillUpgraded(PlayerSkill upgradedSkill)
        {
            if (upgradedSkill.SkillProfile.skillGUID == m_obtainSkillInteraction.SkillProfile.skillGUID)
            {
                UpdateSkillData(upgradedSkill.SkillProfile, upgradedSkill.RuntimeSkillCore.UpgradeableComponent.currentLevel);
            }
        }

        private void UpdateSkillData(SkillProfile skillProfile, int skillLevel)
        {
            SkillLevelData levelData = skillProfile.GetLevelData(skillLevel);

            if (levelData != null)
            {
                m_skillTitle.text = skillLevel.ToString();
                m_skillIcon.sprite = levelData.skillLevelIcon;
            }
        }
    }
}
