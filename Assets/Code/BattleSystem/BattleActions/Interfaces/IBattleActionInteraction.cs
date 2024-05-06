using Dimasyechka.Code.BattleSystem.BattleActions.Views;
using Dimasyechka.Code.Utilities;

namespace Dimasyechka.Code.BattleSystem.BattleActions.Interfaces
{
    public interface IBattleActionInteraction
    {
        public string InteractionTitle { get; }

        public BaseBattleInteractionView ActionProfileViewPrefab { get; }
        public SerializableMonoScript<IBattleActionExecuter> ActionExecuter { get; }
    }
}