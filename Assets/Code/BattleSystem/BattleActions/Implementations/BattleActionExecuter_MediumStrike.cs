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
                // ������� � ��� �� �������� �����
                for (int i = 0; i < _runtimePlayer.RuntimePlayerStats.AttackSpeed; i++)
                {
                    double dmg = _runtimePlayer.RuntimePlayerStats.Damage;

                    if (UnityEngine.Random.Range(0, 101) + _runtimePlayer.RuntimePlayerStats.Luck > _battleController.EnemiesInBattle[_battleController.EnemiesInBattle.Count - 1].DodgeChance)
                    {
                        // �������� ��������� ����������
                        if (UnityEngine.Random.Range(0, 101) <= _runtimePlayer.RuntimePlayerStats.MediumStrikeChance + _runtimePlayer.RuntimePlayerStats.Luck)
                        {
                            if (dmg > _battleController.EnemiesInBattle[_battleController.EnemiesInBattle.Count - 1].Armor)
                            {
                                _battleController.EnemiesInBattle[_battleController.EnemiesInBattle.Count - 1].Health -= dmg;
                                // ��������� ����
                                GameController.Instance.AddEventText(_battleController.CurrentBattleStep + " - �� ������� ���� ���������� ������: " + dmg + " ��.");

                                if (_battleController.EnemiesInBattle[_battleController.EnemiesInBattle.Count - 1].Health <= 0)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                GameController.Instance.AddEventText(_battleController.CurrentBattleStep + " - ����� �������� ����������.");
                            }
                        }
                        else
                            GameController.Instance.AddEventText(_battleController.CurrentBattleStep + " - �� ������������.");
                    }
                    else
                        GameController.Instance.AddEventText(_battleController.CurrentBattleStep + " - ��������� ���������.");
                }
            }
        }
    }
}
