using Dimasyechka.Lubribrary.RxMV.Core;
using Dimasyechka.Lubribrary.RxMV.UniRx.Attributes;
using Newtonsoft.Json.Linq;
using UniRx;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.BattleSystem.PlayerSystem
{
    public class RuntimeBattlePlayerControllerViewModel : MonoViewModel<RuntimeBattlePlayerController>
    {
        [RxAdaptableProperty]
        public ReactiveProperty<int> PlayerActionPoints = new ReactiveProperty<int>();

        [RxAdaptableProperty]
        public ReactiveProperty<int> TotalPlayerActionPoints = new ReactiveProperty<int>();


        [SerializeField]
        private BattleActionsListView _battleActionsView;


        [Inject]
        public void Construct(RuntimeBattlePlayerController controller)
        {
            ZenjectModel(controller);
        }

        protected override void OnSetupModel()
        {
            Model.onPlayerActionPointsChanged += OnPlayerActionsChanged;
            Model.onBattleActionsUpdated += OnBattleActionsUpdated;

            OnBattleActionsUpdated();
        }

        protected override void OnRemoveModel()
        {
            Model.onPlayerActionPointsChanged -= OnPlayerActionsChanged;
            Model.onBattleActionsUpdated -= OnBattleActionsUpdated;
        }

        private void OnBattleActionsUpdated()
        {
            _battleActionsView.SetPressedCallback(Model.ExecuteAction);
            _battleActionsView.UpdateData(Model.AvailablePlayerBattleActions);

            PlayerActionPoints.Value = Model.PlayerActionPoints;
            TotalPlayerActionPoints.Value = Model.PlayerActionPointsTotalPerRound;
        }

        private void OnPlayerActionsChanged(int value)
        {
            PlayerActionPoints.Value = value;
            TotalPlayerActionPoints.Value = Model.PlayerActionPointsTotalPerRound;
        }
    }
}
