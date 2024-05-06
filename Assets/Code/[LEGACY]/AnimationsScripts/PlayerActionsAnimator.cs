using Dimasyechka.Code.BattleSystem.Controllers;
using UnityEngine;

namespace Dimasyechka.Code._LEGACY_.AnimationsScripts
{
    public class PlayerActionsAnimator : MonoBehaviour
    {
        // Конец анимации мечей и начало боя
        public void EndAnimation()
        {
            //BattleController.Instance.StartBattleAfterAnim();
        }

        // Конец битвы и начало анимации монеток как знак победы
        public void EndBattle()
        {
            BattleController.Instance.EndBattle();
        }
    }
}
