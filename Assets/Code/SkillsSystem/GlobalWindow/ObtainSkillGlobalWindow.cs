using Dimasyechka.Code.GlobalWindows.Base;
using Dimasyechka.Code.SkillsSystem.Controllers;
using Dimasyechka.Code.SkillsSystem.Core;
using Dimasyechka.Code.SkillsSystem.Views;
using Dimasyechka.Code.UpgradeableSystem;
using Dimasyechka.Lubribrary.RxMV.UniRx.Attributes;
using System;
using System.Collections.Generic;
using Dimasyechka.Lubribrary.RxMV.UniRx.RxLink;
using UniRx;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.SkillsSystem.GlobalWindow
{
    public class ObtainSkillGlobalWindow : BaseGameGlobalWindow, IRxLinkable
    {
        [SerializeField]
        private ObtainSkillDataView _currentSkillView;

        [SerializeField]
        private ObtainSkillDataView _nextSkillView;


        [SerializeField]
        private SkillUpgradeDescriptionView _neededUpgradePrefab;

        [SerializeField]
        private Transform _neededUpgradesContentParent;


        [RxAdaptableProperty]
        public ReactiveProperty<bool> IsUpgradeButtonAvailable = new ReactiveProperty<bool>();


        private PlayerSkillsController _playerSkillsController;

        [Inject]
        public void Construct(PlayerSkillsController playerSkillsController)
        {
            _playerSkillsController = playerSkillsController;
        }


        protected override void OnShow()
        {
            DrawSkillData();
        }

        private void DrawSkillData()
        {
            ObtainSkillGlobalWindowData data = GetConvertedWindowData<ObtainSkillGlobalWindowData>();

            SkillCore upgradeableSkillCore = _playerSkillsController.GetUpgradeableSkillCore(data.SkillProfile);
            
            int currentSkillLevel = -1;
            bool isMaxUpgraded = false;
            List<string> deltaValues = new List<string>();

            if (upgradeableSkillCore != null)
            {
                currentSkillLevel = upgradeableSkillCore.UpgradeableComponent.currentLevel;
                deltaValues = upgradeableSkillCore.GetSkillDeltaValues(currentSkillLevel + 1);
                isMaxUpgraded = upgradeableSkillCore.UpgradeableComponent.reachedMaxUpgrades;
            }
            else
            {
                isMaxUpgraded = false;
                deltaValues = upgradeableSkillCore.GetSkillDeltaValues(1);
            }

            SkillLevelData currentLevelData = currentSkillLevel == -1 ? null : upgradeableSkillCore.SkillProfile.GetLevelData(currentSkillLevel);
            SkillLevelData nextLevelData = upgradeableSkillCore.SkillProfile.GetLevelData(currentSkillLevel + 1);

            ClearUpgradesDescription();

            if (isMaxUpgraded)
            {
                IsUpgradeButtonAvailable.Value = false;

                _currentSkillView.SetupModel(new ObtainSkillData()
                {
                    SkillLevelData = currentLevelData,
                    SkillLevel = currentSkillLevel
                });


                _nextSkillView.SetupModel(new ObtainSkillData()
                {
                    SkillLevelData = null,
                    SkillLevel = 0,
                    DeltaValues = null
                });

                SkillUpgradeDescriptionView view = Instantiate(_neededUpgradePrefab, _neededUpgradesContentParent);
                view.SetupModel(new SkillUpgradeDescriptionData()
                {
                    Description = "Достигнут макс. уровень!",
                    IsAccomplished = true
                });
            }
            else
            {
                IsUpgradeButtonAvailable.Value = true;

                _currentSkillView.SetupModel(new ObtainSkillData()
                {
                    SkillLevelData = currentLevelData,
                    SkillLevel = currentSkillLevel
                });

                _nextSkillView.SetupModel(new ObtainSkillData()
                {
                    SkillLevelData = nextLevelData,
                    SkillLevel = currentSkillLevel + 1,
                    DeltaValues = deltaValues
                });

                DrawUpgradesDescription(upgradeableSkillCore);
            }
        }

        private void ClearUpgradesDescription()
        {
            _neededUpgradesContentParent.gameObject.SetActive(true);

            for (int i = _neededUpgradesContentParent.childCount - 1; i >= 0; i--)
            {
                Destroy(_neededUpgradesContentParent.GetChild(i).gameObject);
            }
        }

        private void DrawUpgradesDescription(SkillCore skillCore)
        {
            List<SkillUpgradeDescriptionData> skillUpgrades = new List<SkillUpgradeDescriptionData>();

            if (skillCore == null)
            {
                ObtainSkillGlobalWindowData data = GetConvertedWindowData<ObtainSkillGlobalWindowData>();
                skillUpgrades = skillCore.UpgradeableComponent.GetUpgradeDescriptions();
            }
            else
            {
                skillUpgrades = skillCore.UpgradeableComponent.GetUpgradeDescriptions();
            }

            foreach (SkillUpgradeDescriptionData upgradeDescription in skillUpgrades)
            {
                SkillUpgradeDescriptionView view = Instantiate(_neededUpgradePrefab, _neededUpgradesContentParent);

                view.SetupModel(upgradeDescription);
            }
        }


        [RxAdaptableMethod]
        public void OnApplyButtonPressed()
        {
            ObtainSkillGlobalWindowData data = GetConvertedWindowData<ObtainSkillGlobalWindowData>();

            data?.OnApply();

            DrawSkillData();
        }

        [RxAdaptableMethod]
        public void OnHidePressed()
        {
            Hide();
        }

        protected override void OnHide()
        {
            _playerSkillsController.DestroyTemporalUpgradeable();
        }
    }

    [System.Serializable]
    public class ObtainSkillGlobalWindowData : BaseGameGlobalWindowData
    {
        public SkillProfile SkillProfile;
        
        public Action OnApply;
    }
}