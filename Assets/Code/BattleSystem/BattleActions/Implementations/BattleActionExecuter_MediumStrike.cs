using Dimasyechka.Code.BattleSystem.Controllers;

namespace Dimasyechka.Code.BattleSystem.BattleActions.Implementations
{
    public class BattleActionExecuterMediumStrike : BattleActionExecuterBaseActionExecuter
    {
        public override void ExecuteAction()
        {
            base.ExecuteAction();

            if (!_runtimeBattlePlayerController.CriticalStrike())
            {
                // Атакуем Х раз от скорости атаки
                for (int i = 0; i < _runtimePlayer.RuntimePlayerStats.AttackSpeed; i++)
                {
                    double dmg = _runtimePlayer.RuntimePlayerStats.Damage;

                    if (UnityEngine.Random.Range(0, 101) + _runtimePlayer.RuntimePlayerStats.Luck > _battleController.EnemiesInBattle[_battleController.EnemiesInBattle.Count - 1].DodgeChance)
                    {
                        // Проверка уклонения противника
                        if (UnityEngine.Random.Range(0, 101) <= _runtimePlayer.RuntimePlayerStats.MediumStrikeChance + _runtimePlayer.RuntimePlayerStats.Luck)
                        {
                            if (dmg > _battleController.EnemiesInBattle[_battleController.EnemiesInBattle.Count - 1].Armor)
                            {
                                _battleController.EnemiesInBattle[_battleController.EnemiesInBattle.Count - 1].Health -= dmg;
                                // Анимируем удар
                                GameController.Instance.AddEventText(_battleController.CurrentBattleStep + " - Вы нанесли урон нормальным ударом: " + dmg + " ед.");

                                if (_battleController.EnemiesInBattle[_battleController.EnemiesInBattle.Count - 1].Health <= 0)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                GameController.Instance.AddEventText(_battleController.CurrentBattleStep + " - Броня защитила противника.");
                            }
                        }
                        else
                            GameController.Instance.AddEventText(_battleController.CurrentBattleStep + " - Вы промахнулись.");
                    }
                    else
                        GameController.Instance.AddEventText(_battleController.CurrentBattleStep + " - Противник увернулся.");
                }
            }
        }
    }
}
