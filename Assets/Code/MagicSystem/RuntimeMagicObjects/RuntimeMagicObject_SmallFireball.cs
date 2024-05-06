using Dimasyechka.Code.BattleSystem.BattleActions.Implementations;
using Dimasyechka.Code.BattleSystem.Controllers;

namespace Dimasyechka.Code.MagicSystem.RuntimeMagicObjects
{
    public class RuntimeMagicObjectSmallFireball : BattleMagicExecuterBaseActionExecuter
    {
        private float damage = 5f;


        public override void ExecuteAction()
        {
            base.ExecuteAction();

            _battleController.EnemiesInBattle[_battleController.EnemiesInBattle.Count - 1].Health -= damage;
            GameController.Instance.AddEventText(_battleController.CurrentBattleStep + " - Вы нанесли урон магией '" + _magicProfile.InteractionTitle + "' " + damage + " ед.");
        }
    }
}
