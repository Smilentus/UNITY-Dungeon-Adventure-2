using Dimasyechka.Code.BattleSystem.Controllers;
using Dimasyechka.Code.BattleSystem.PlayerSystem;

namespace Dimasyechka.Code.BattleSystem.BattleActions.Implementations
{
    public class BattleActionExecuter_LightStrike : BattleActionExecuter_BaseActionExecuter
    {
        public override void ExecuteAction()
        {
            base.ExecuteAction();

            if (!RuntimeBattlePlayerController.Instance.CriticalStrike())
            {
                // ������� � ��� �� �������� �����
                for (int i = 0; i < RuntimePlayer.Instance.RuntimePlayerStats.AttackSpeed; i++)
                {
                    double dmg = RuntimePlayer.Instance.RuntimePlayerStats.Damage / 2;

                    // �������� ��������� ����������
                    if (UnityEngine.Random.Range(0, 101) + RuntimePlayer.Instance.RuntimePlayerStats.Luck > BattleController.Instance.EnemiesInBattle[BattleController.Instance.EnemiesInBattle.Count - 1].DodgeChance)
                    {
                        if (UnityEngine.Random.Range(0, 101) <= RuntimePlayer.Instance.RuntimePlayerStats.LightStrikeChance + RuntimePlayer.Instance.RuntimePlayerStats.Luck)
                        {
                            if (dmg > BattleController.Instance.EnemiesInBattle[BattleController.Instance.EnemiesInBattle.Count - 1].Armor)
                            {
                                BattleController.Instance.EnemiesInBattle[BattleController.Instance.EnemiesInBattle.Count - 1].Health -= dmg;
                                // ��������� ����
                                GameController.Instance.AddEventText(BattleController.Instance.CurrentBattleStep + " - �� ������� ���� ������ ������: " + dmg + " ��.");

                                if (BattleController.Instance.EnemiesInBattle[BattleController.Instance.EnemiesInBattle.Count - 1].Health <= 0)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                GameController.Instance.AddEventText(BattleController.Instance.CurrentBattleStep + " - ����� �������� ����������.");
                            }
                        }
                        else
                            GameController.Instance.AddEventText(BattleController.Instance.CurrentBattleStep + " - �� ������������.");
                    }
                    else
                        GameController.Instance.AddEventText(BattleController.Instance.CurrentBattleStep + " - ��������� ���������.");
                }
            }
        }
    }
}
