using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleActionExecuter_HeavyStrike : MonoBehaviour, IBattleActionExecuter
{
    public void Initialize()
    {
        Debug.Log($"BattleActionExecuter_HeavyStrike initialized!");
    }

    public void EveryTurnCheck(BattleController.TurnStatus turnStatus)
    {

    }

    public void ExecuteAction()
    {
        if (!RuntimeBattlePlayerController.Instance.CriticalStrike())
        {
            // Атакуем Х раз от скорости атаки
            for (int i = 0; i < Player.AttackSpeed; i++)
            {
                double dmg = Player.Damage * 2;

                // Проверка уклонения противника       
                if (UnityEngine.Random.Range(0, 101) + Player.Luck > BattleController.Instance.EnemiesInBattle[BattleController.Instance.EnemiesInBattle.Count - 1].DodgeChance)
                {
                    if (UnityEngine.Random.Range(0, 101) <= Player.HeavyStrikeChance + Player.Luck)
                    {
                        if (dmg > BattleController.Instance.EnemiesInBattle[BattleController.Instance.EnemiesInBattle.Count - 1].Armor)
                        {
                            BattleController.Instance.EnemiesInBattle[BattleController.Instance.EnemiesInBattle.Count - 1].Health -= dmg;
                            // Анимируем удар
                            GameController.Instance.AddEventText(BattleController.Instance.CurrentBattleStep + " - Вы нанесли урон сильным ударом: " + dmg + " ед.");
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
