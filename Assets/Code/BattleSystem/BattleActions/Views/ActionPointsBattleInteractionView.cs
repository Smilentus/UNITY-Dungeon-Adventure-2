using Dimasyechka.Code.BattleSystem.BattleActions.Profiles;
using Dimasyechka.Code.BattleSystem.PlayerSystem;
using Dimasyechka.Lubribrary.RxMV.UniRx.Attributes;
using UniRx;

namespace Dimasyechka.Code.BattleSystem.BattleActions.Views
{
    public class ActionPointsBattleInteractionView : BaseBattleInteractionView
    {
        [RxAdaptableProperty]
        public ReactiveProperty<int> SpendableActions = new ReactiveProperty<int>();


        public override void SetData(AvailableBattleActionData availableBattleActionData, int index)
        {
            base.SetData(availableBattleActionData, index);

            BattleActionProfile profile = availableBattleActionData.Interaction as BattleActionProfile;

            if (profile != null)
            {
                SpendableActions.Value = profile.SpendableActions;
            }
        }
    }
}