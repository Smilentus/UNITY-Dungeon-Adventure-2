using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionsAnimator : MonoBehaviour
{
    // Конец анимации мечей и начало боя
    public void EndAnimation()
    {
        BattleHelper._BH.StartBattleAfterAnim();
    }

    // Конец битвы и начало анимации монеток как знак победы
    public void EndBattle()
    {
        BattleHelper._BH.EndBattle();
    }
}
