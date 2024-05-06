using System;
using System.Collections.Generic;
using Dimasyechka.Code.GlobalWindows.Base;
using Dimasyechka.Code.SkillsSystem.Controllers;
using Dimasyechka.Code.SkillsSystem.Core;
using Dimasyechka.Code.SkillsSystem.Views;
using Dimasyechka.Code.UpgradeableSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Dimasyechka.Code.SkillsSystem.GlobalWindow
{
    public class ObtainSkillGlobalWindow : BaseGameGlobalWindow
    {
        [SerializeField]
        private ObtainSkillDataView m_currentSkillView;

        [SerializeField]
        private ObtainSkillDataView m_nextSkillView;



        [SerializeField]
        private SkillUpgradeDescriptionView m_neededUpgradePrefab;

        [SerializeField]
        private Transform m_neededUpgradesContentParent;


        [SerializeField]
        private Button m_closeButton;

        [SerializeField]
        private Button m_upgradeButton;


        private void OnEnable()
        {
            m_closeButton.onClick.AddListener(Hide);
            m_upgradeButton.onClick.AddListener(OnApplyButtonPressed);
        }

        private void OnDisable()
        {
            m_closeButton.onClick.RemoveListener(Hide);
            m_upgradeButton.onClick.RemoveListener(OnApplyButtonPressed);
        }


        protected override void OnShow()
        {
            DrawSkillData();
        }

        private void DrawSkillData()
        {
            ObtainSkillGlobalWindowData data = GetConvertedWindowData<ObtainSkillGlobalWindowData>();

            PlayerSkill playerSkill = PlayerSkillsController.instance.GetPlayerSkillByGuid(data.Profile.skillGUID);

            int currentSkillLevel = -1;
            bool isMaxUpgraded = false;
            List<string> deltaValues = new List<string>();

            if (playerSkill != null)
            {
                currentSkillLevel = playerSkill.RuntimeSkillCore.UpgradeableComponent.currentLevel;
                deltaValues = playerSkill.RuntimeSkillCore.GetSkillDeltaValues(currentSkillLevel + 1);
                isMaxUpgraded = playerSkill.RuntimeSkillCore.UpgradeableComponent.reachedMaxUpgrades;
            }
            else
            {
                isMaxUpgraded = false;
                deltaValues = data.Profile.skillCorePrefab.GetSkillDeltaValues(1);
            }

            SkillLevelData currentLevelData = currentSkillLevel == -1 ? null : data.Profile.GetLevelData(currentSkillLevel);
            SkillLevelData nextLevelData = data.Profile.GetLevelData(currentSkillLevel + 1);

            ClearUpgradesDescription();

            if (isMaxUpgraded)
            {
                m_upgradeButton.gameObject.SetActive(false);

                m_currentSkillView.SetData(currentLevelData, currentSkillLevel);
                m_nextSkillView.SetData(null, 0, null);

                SkillUpgradeDescriptionView view = Instantiate(m_neededUpgradePrefab, m_neededUpgradesContentParent);
                view.SetData(new SkillUpgradeDescriptionData() 
                {
                    Description = "Достигнут макс. уровень!",
                    IsAccomplished = true
                });
            }
            else
            {
                m_upgradeButton.gameObject.SetActive(true);

                m_currentSkillView.SetData(currentLevelData, currentSkillLevel);
                m_nextSkillView.SetData(nextLevelData, currentSkillLevel + 1, deltaValues);

                DrawUpgradesDescription(playerSkill);
            }
        }

        private void ClearUpgradesDescription()
        {
            m_neededUpgradesContentParent.gameObject.SetActive(true);

            for (int i = m_neededUpgradesContentParent.childCount - 1; i >= 0; i--)
            {
                Destroy(m_neededUpgradesContentParent.GetChild(i).gameObject);
            }
        }

        private void DrawUpgradesDescription(PlayerSkill playerSkill)
        {
            List<SkillUpgradeDescriptionData> skillUpgrades = new List<SkillUpgradeDescriptionData>();

            if (playerSkill == null)
            {
                ObtainSkillGlobalWindowData data = GetConvertedWindowData<ObtainSkillGlobalWindowData>();
                skillUpgrades = data.Profile.skillCorePrefab.UpgradeableComponent.GetUpgradeDescriptions();
            }
            else
            {
                skillUpgrades = playerSkill.RuntimeSkillCore.UpgradeableComponent.GetUpgradeDescriptions();
            }

            foreach (SkillUpgradeDescriptionData upgradeDescription in skillUpgrades)
            {
                SkillUpgradeDescriptionView view = Instantiate(m_neededUpgradePrefab, m_neededUpgradesContentParent);

                view.SetData(upgradeDescription);
            }
        }


        private void OnApplyButtonPressed()
        {
            ObtainSkillGlobalWindowData data = GetConvertedWindowData<ObtainSkillGlobalWindowData>();

            data?.OnApply();

            DrawSkillData();
        }
    }

    [System.Serializable]
    public class ObtainSkillGlobalWindowData : BaseGameGlobalWindowData
    {
        public SkillProfile Profile;

        public Action OnApply;
    }
}