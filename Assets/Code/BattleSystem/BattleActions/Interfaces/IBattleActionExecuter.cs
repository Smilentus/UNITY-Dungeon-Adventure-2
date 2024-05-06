using Dimasyechka.Code.BattleSystem.Controllers;

namespace Dimasyechka.Code.BattleSystem.BattleActions.Interfaces
{
    public interface IBattleActionExecuter
    {
        public IBattleActionInteraction BattleActionInteraction { get; set;  }

        public bool CanExecuteAction();

        public void Initialize();

        public void SetInteraction(IBattleActionInteraction _interaction);

        public void EveryTurnCheck(BattleController.TurnStatus turnStatus);

        public void ExecuteAction();
    }
}
