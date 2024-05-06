using Dimasyechka.Code.BattleSystem.BattleActions.Interfaces;
using Dimasyechka.Code.BattleSystem.BattleActions.Profiles;
using Dimasyechka.Code.BattleSystem.Controllers;
using Dimasyechka.Code.BattleSystem.PlayerSystem;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.BattleSystem.BattleActions.Implementations
{
    public class BattleActionExecuterBaseActionExecuter : MonoBehaviour, IBattleActionExecuter
    {
        protected IBattleActionInteraction _battleActionInteraction;
        public IBattleActionInteraction BattleActionInteraction { get => _battleActionInteraction; set => _battleActionInteraction = value; }


        protected BattleActionProfile _battleActionProfile;

        protected RuntimeBattlePlayerController _runtimeBattlePlayerController;
        protected BattleController _battleController;
        protected RuntimePlayer _runtimePlayer;

        [Inject]
        public void Construct(RuntimeBattlePlayerController runtimeBattlePlayerController, RuntimePlayer runtimePlayer, BattleController battleController)
        {
            _runtimeBattlePlayerController = runtimeBattlePlayerController;
            _runtimePlayer = runtimePlayer;
            _battleController = battleController;
        }


        public virtual bool CanExecuteAction()
        {
            return _runtimeBattlePlayerController.PlayerActionPoints >= _battleActionProfile.SpendableActions;
        }

        public virtual void Initialize() 
        {

        }

        public virtual void SetInteraction(IBattleActionInteraction _interaction)
        {
            if (_battleActionInteraction == null)
            {
                _battleActionInteraction = _interaction;
            }

            _battleActionProfile = _interaction as BattleActionProfile;
        }

        public virtual void EveryTurnCheck(BattleController.TurnStatus turnStatus) { }

        public virtual void ExecuteAction() 
        {
            _runtimeBattlePlayerController.PlayerActionPoints -= _battleActionProfile.SpendableActions;
        }
    }
}
