using Dimasyechka.Code.BattleSystem.BattleActions.Interfaces;
using Dimasyechka.Code.BattleSystem.BattleActions.Profiles;
using Dimasyechka.Code.BattleSystem.Controllers;
using Dimasyechka.Code.BattleSystem.PlayerSystem;
using UnityEngine;

namespace Dimasyechka.Code.BattleSystem.BattleActions.Implementations
{
    public class BattleActionExecuter_BaseActionExecuter : MonoBehaviour, IBattleActionExecuter
    {
        protected IBattleActionInteraction battleActionInteraction;
        public IBattleActionInteraction BattleActionInteraction { get => battleActionInteraction; set => battleActionInteraction = value; }


        protected BattleActionProfile battleActionProfile;


        public virtual bool CanExecuteAction()
        {
            return RuntimeBattlePlayerController.Instance.PlayerActionPoints >= battleActionProfile.SpendableActions;
        }

        public virtual void Initialize() 
        {

        }

        public virtual void SetInteraction(IBattleActionInteraction _interaction)
        {
            if (battleActionInteraction == null)
            {
                battleActionInteraction = _interaction;
            }

            battleActionProfile = _interaction as BattleActionProfile;
        }

        public virtual void EveryTurnCheck(BattleController.TurnStatus turnStatus) { }

        public virtual void ExecuteAction() 
        {
            RuntimeBattlePlayerController.Instance.PlayerActionPoints -= battleActionProfile.SpendableActions;
        }
    }
}
