using Dimasyechka.Code.BattleSystem.BattleActions.Implementations;
using Dimasyechka.Code.BattleSystem.PlayerSystem;
using Dimasyechka.Code.MagicSystem.Profiles;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dimasyechka.Code.BattleSystem.BattleActions.Views
{
    public class MagicBattleInteractionView : BaseBattleInteractionView
    {
        [SerializeField]
        protected TMP_Text m_manaCostTMP;


        [SerializeField]
        protected Image m_FillableImage;

        [SerializeField]
        protected TMP_Text m_CooldownTMP;


        private BattleMagicExecuterBaseActionExecuter magicExecuter;


        public void SetFillAmountRatio(int _cooldown, float _ratio)
        {
            if (m_CooldownTMP != null)
            {
                m_CooldownTMP.text = _cooldown.ToString("f0");
            }

            if (m_FillableImage != null)
            {
                m_FillableImage.fillAmount = Mathf.Clamp01(_ratio);
            }
        }


        public override void SetData(AvailableBattleActionData availableBattleActionData, int index)
        {
            base.SetData(availableBattleActionData, index);

            BaseMagicProfile profile = availableBattleActionData.Interaction as BaseMagicProfile;

            m_manaCostTMP.text = $"{profile.DefaultManaPointsCost} ÎÌ";

            magicExecuter = availableBattleActionData.Executer as BattleMagicExecuterBaseActionExecuter;
            magicExecuter.OnCooldownPassed += OnCooldownPassed;

            SetFillAmountRatio(magicExecuter.CooldownValue, magicExecuter.CooldownRatio);
        }

        private void OnCooldownPassed(int cooldown)
        {
            SetFillAmountRatio(magicExecuter.CooldownValue, magicExecuter.CooldownRatio);
        }
    }
}
