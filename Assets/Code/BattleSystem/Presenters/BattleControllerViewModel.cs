using Dimasyechka.Code.BattleSystem.Controllers;
using Dimasyechka.Lubribrary.RxMV.UniRx.Attributes;
using Dimasyechka.Lubribrary.RxMV.UniRx.RxLink;
using UniRx;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.BattleSystem.Presenters
{
    public class BattleControllerViewModel : MonoBehaviour, IRxLinkable
    {
        [RxAdaptableProperty]
        public ReactiveProperty<string> TurnStatus = new ReactiveProperty<string>();


        private BattleController _battleController;


        [Inject]
        public void Construct(BattleController battleController)
        {
            _battleController = battleController;
        }


        private void Awake()
        {
            _battleController.onBattleStatusChanged += OnBattleStatusChanged;
            _battleController.onBattleTurnStatusChanged += OnBattleTurnStatusChanged;
        }

        private void OnDestroy()
        {
            if (_battleController != null)
            {
                _battleController.onBattleStatusChanged += OnBattleStatusChanged;
                _battleController.onBattleTurnStatusChanged += OnBattleTurnStatusChanged;
            }
        }


        private void OnBattleTurnStatusChanged(BattleController.TurnStatus turnStatus)
        {
            if (turnStatus == BattleController.TurnStatus.PlayerTurn)
            {
                TurnStatus.Value = "Ход игрока";
            }
            else if (turnStatus == BattleController.TurnStatus.EnemiesTurn)
            {
                TurnStatus.Value = "Ход противника";
            }
            else
            {
                TurnStatus.Value = "Неизвестно чей ход???";
            }
        }

        private void OnBattleStatusChanged(bool value)
        {

        }
    }
}
