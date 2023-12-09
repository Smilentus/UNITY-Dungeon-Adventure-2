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
            // Атакуем Х раз от скорости атаки
            for (int i = 0; i < RuntimePlayer.Instance.RuntimePlayerStats.AttackSpeed; i++)
            {
                double dmg = RuntimePlayer.Instance.RuntimePlayerStats.Damage;

                if (UnityEngine.Random.Range(0, 101) + RuntimePlayer.Instance.RuntimePlayerStats.Luck > BattleController.Instance.EnemiesInBattle[BattleController.Instance.EnemiesInBattle.Count - 1].DodgeChance)
                {
                    // Проверка уклонения противника
                    if (UnityEngine.Random.Range(0, 101) <= RuntimePlayer.Instance.RuntimePlayerStats.MediumStrikeChance + RuntimePlayer.Instance.RuntimePlayerStats.Luck)
                    {
                        if (dmg > BattleController.Instance.EnemiesInBattle[BattleController.Instance.EnemiesInBattle.Count - 1].Armor)
                        {
                            BattleController.Instance.EnemiesInBattle[BattleController.Instance.EnemiesInBattle.Count - 1].Health -= dmg;
                            // Анимируем удар
                            GameController.Instance.AddEventText(BattleController.Instance.CurrentBattleStep + " - Вы нанесли урон нормальным ударом: " + dmg + " ед.");

                            if (BattleController.Instance.EnemiesInBattle[BattleController.Instance.EnemiesInBattle.Count - 1].Health <= 0)
                            {
                                break;
                            }
                        }
                        else
                        {
                            GameController.Instance.AddEventText(BattleController.Instance.CurrentBattleStep + " - Броня защитила противника.");
                        }
                    }
                    else
                        GameController.Instance.AddEventText(BattleController.Instance.CurrentBattleStep + " - Вы промахнулись.");
                }
                else
                    GameController.Instance.AddEventText(BattleController.Instance.CurrentBattleStep + " - Противник увернулся.");
            }
        }
    }
}
