using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleActionExecuter_MediumStrike : MonoBehaviour, IBattleActionExecuter
{
    public void Initialize()
    {
        Debug.Log($"BattleActionExecuter_MediumStrike initialized!");
    }

    public void EveryTurnCheck(BattleController.TurnStatus turnStatus)
    {

    }

    public void ExecuteAction()
    {
        if (!RuntimeBattlePlayerController.Instance.CriticalStrike())
        {
            // ������� � ��� �� �������� �����
            for (int i = 0; i < RuntimePlayer.Instance.RuntimePlayerStats.AttackSpeed; i++)
            {
                double dmg = RuntimePlayer.Instance.RuntimePlayerStats.Damage;

                if (UnityEngine.Random.Range(0, 101) + RuntimePlayer.Instance.RuntimePlayerStats.Luck > BattleController.Instance.EnemiesInBattle[BattleController.Instance.EnemiesInBattle.Count - 1].DodgeChance)
                {
                    // �������� ��������� ����������
                    if (UnityEngine.Random.Range(0, 101) <= RuntimePlayer.Instance.RuntimePlayerStats.MediumStrikeChance + RuntimePlayer.Instance.RuntimePlayerStats.Luck)
                    {
                        if (dmg > BattleController.Instance.EnemiesInBattle[BattleController.Instance.EnemiesInBattle.Count - 1].Armor)
                        {
                            BattleController.Instance.EnemiesInBattle[BattleController.Instance.EnemiesInBattle.Count - 1].Health -= dmg;
                            // ��������� ����
                            GameController.Instance.AddEventText(BattleController.Instance.CurrentBattleStep + " - �� ������� ���� ���������� ������: " + dmg + " ��.");

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
