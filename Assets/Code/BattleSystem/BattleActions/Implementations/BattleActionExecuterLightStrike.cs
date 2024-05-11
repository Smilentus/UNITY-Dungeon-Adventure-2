namespace Dimasyechka.Code.BattleSystem.BattleActions.Implementations
{
    public class BattleActionExecuterLightStrike : BattleActionExecuterBaseActionExecuter
    {
        public override void ExecuteAction()
        {
            base.ExecuteAction();

            if (!_runtimeBattlePlayerController.CriticalStrike())
            {
                // ������� � ��� �� �������� �����
                for (int i = 0; i < _runtimePlayer.RuntimePlayerStats.AttackSpeed.Value; i++)
                {
                    double dmg = _runtimePlayer.RuntimePlayerStats.Damage.Value / 2;

                    // �������� ��������� ����������
                    if (UnityEngine.Random.Range(0, 101) + _runtimePlayer.RuntimePlayerStats.Luck.Value > _battleController.EnemiesInBattle[_battleController.EnemiesInBattle.Count - 1].DodgeChance)
                    {
                        if (UnityEngine.Random.Range(0, 101) <= _runtimePlayer.RuntimePlayerStats.LightStrikeChance.Value + _runtimePlayer.RuntimePlayerStats.Luck.Value)
                        {
                            if (dmg > _battleController.EnemiesInBattle[_battleController.EnemiesInBattle.Count - 1].Armor)
                            {
                                _battleController.EnemiesInBattle[_battleController.EnemiesInBattle.Count - 1].Health -= dmg;
                                // ��������� ����
                                //GameController.Instance.AddEventText(_battleController.CurrentBattleStep + " - �� ������� ���� ������ ������: " + dmg + " ��.");

                                if (_battleController.EnemiesInBattle[_battleController.EnemiesInBattle.Count - 1].Health <= 0)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                //GameController.Instance.AddEventText(_battleController.CurrentBattleStep + " - ����� �������� ����������.");
                            }
                        }
                        else
                        {
                            //GameController.Instance.AddEventText(_battleController.CurrentBattleStep + " - �� ������������.");
                        }
                    }
                    else
                    {
                        //GameController.Instance.AddEventText(_battleController.CurrentBattleStep + " - ��������� ���������.");
                    }
                }
            }
        }
    }
}
