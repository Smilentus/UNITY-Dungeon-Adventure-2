using Dimasyechka.Code.BattleSystem.BattleActions.Profiles;
using Dimasyechka.Code.BattleSystem.PlayerSystem;
using TMPro;
using UnityEngine;

namespace Dimasyechka.Code.BattleSystem.BattleActions.Views
{
    public class ActionPointsBattleInteractionView : BaseBattleInteractionView
    {
        [SerializeField]
        protected TMP_Text _mActionPointsCostTMP;


        public override void SetData(AvailableBattleActionData availableBattleActionData, int index)
        {
            base.SetData(availableBattleActionData, index);

            BattleActionProfile profile = availableBattleActionData.Interaction as BattleActionProfile;

            _mActionPointsCostTMP.text = $"{profile.SpendableActions} нд";
        }
    }
}