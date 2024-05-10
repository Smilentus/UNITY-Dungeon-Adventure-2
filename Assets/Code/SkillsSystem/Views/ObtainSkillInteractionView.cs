using Dimasyechka.Code.SkillsSystem.Controllers;
using Dimasyechka.Code.SkillsSystem.Core;
using Dimasyechka.Code.SkillsSystem.Interactions;
using Dimasyechka.Lubribrary.RxMV.Core;
using Dimasyechka.Lubribrary.RxMV.UniRx.Attributes;
using UniRx;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.SkillsSystem.Views
{
    public class ObtainSkillInteractionView : MonoViewModel<PlayerSkillsController>
    {
        [SerializeField]
        private ObtainSkillInteraction _obtainSkillInteraction;


        [RxAdaptableProperty]
        public ReactiveProperty<string> SkillTitle = new ReactiveProperty<string>();

        [RxAdaptableProperty]
        public ReactiveProperty<Sprite> SkillIcon = new ReactiveProperty<Sprite>();


        [Inject]
        public void Construct(PlayerSkillsController playerSkillsController)
        {
            ZenjectModel(playerSkillsController);
        }


        protected override void OnSetupModel()
        {
            if (_obtainSkillInteraction == null)
            {
                _obtainSkillInteraction = GetComponent<ObtainSkillInteraction>();
            }

            Model.onSkillUpgraded += OnSkillUpgraded;

            PlayerSkill playerSkill = Model.GetPlayerSkillByGuid(_obtainSkillInteraction.SkillProfile.skillGUID);

            if (playerSkill == null)
            {
                UpdateSkillData(_obtainSkillInteraction.SkillProfile, 0);
            }
            else
            {
                UpdateSkillData(_obtainSkillInteraction.SkillProfile, playerSkill.RuntimeSkillCore.UpgradeableComponent.currentLevel);
            }
        }

        protected override void OnRemoveModel()
        {
            if (Model != null)
            {
                Model.onSkillUpgraded -= OnSkillUpgraded;
            }
        }


        // TODO: Переделать по другой архитектуре
        private void OnSkillUpgraded(PlayerSkill upgradedSkill)
        {
            if (upgradedSkill.SkillProfile.skillGUID == _obtainSkillInteraction.SkillProfile.skillGUID)
            {
                UpdateSkillData(upgradedSkill.SkillProfile, upgradedSkill.RuntimeSkillCore.UpgradeableComponent.currentLevel);
            }
        }

        private void UpdateSkillData(SkillProfile skillProfile, int skillLevel)
        {
            SkillLevelData levelData = skillProfile.GetLevelData(skillLevel);

            if (levelData != null)
            {
                SkillTitle.Value = skillLevel.ToString();
                SkillIcon.Value = levelData.skillLevelIcon;
            }
        }
    }
}
