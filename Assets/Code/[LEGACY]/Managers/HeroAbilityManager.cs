using UnityEngine;

namespace Dimasyechka.Code._LEGACY_.Managers
{
    public class HeroAbilityManager : MonoBehaviour
    {
        [Header("Панель использования абилки")]
        public GameObject abilityPanel;

        private void Start()
        {
            //if (RuntimePlayer.HALvl > 0)
            //    abilityPanel.SetActive(true);
            //else
            //    abilityPanel.SetActive(false);
        }

        // Сброс абилки 
        public void ResetHeroAbility()
        {
            //RuntimePlayer.HALvl = 0;
            //RuntimePlayer.HAMaxLvl = 10;
            //RuntimePlayer.HAChargePower = 0;
            //RuntimePlayer.HACharge = 0;
            //RuntimePlayer.HAMaxCharge = 0;
            //RuntimePlayer.Ability = RuntimePlayer.HeroAbility.None;
            //abilityPanel.SetActive(false);
        }

        // Улучшение абилки
        public void UpgradeHeroAbility()
        {
            //if (RuntimePlayer.HALvl < RuntimePlayer.HAMaxLvl && RuntimePlayer.HACharge >= RuntimePlayer.HAMaxCharge)
            //{
            //    if (RuntimePlayer.HALvl == 0)
            //    {
            //        RuntimePlayer.HALvl++;
            //        switch (RuntimePlayer.Ability)
            //        {
            //            case RuntimePlayer.HeroAbility.Archer:
            //                RuntimePlayer.HAChargePower = 1;
            //                RuntimePlayer.HAMaxCharge = 325;
            //                break;
            //            case RuntimePlayer.HeroAbility.Explosioner:
            //                RuntimePlayer.HAChargePower = 3;
            //                RuntimePlayer.HAMaxCharge = 250;
            //                break;
            //            case RuntimePlayer.HeroAbility.Mage:
            //                RuntimePlayer.HAChargePower = 4;
            //                RuntimePlayer.HAMaxCharge = 150;
            //                break;
            //            case RuntimePlayer.HeroAbility.Ninja:
            //                RuntimePlayer.HAChargePower = 3;
            //                RuntimePlayer.HAMaxCharge = 500;
            //                break;
            //            case RuntimePlayer.HeroAbility.Warrior:
            //                RuntimePlayer.HAChargePower = 2;
            //                RuntimePlayer.HAMaxCharge = 400;
            //                break;
            //            default:
            //                break;
            //        }
            //        abilityPanel.SetActive(true);
            //    }
            //    else
            //    {
            //        RuntimePlayer.HALvl++;
            //        switch (RuntimePlayer.Ability)
            //        {
            //            case RuntimePlayer.HeroAbility.Archer:
            //                RuntimePlayer.HAChargePower += 1;
            //                RuntimePlayer.HAMaxCharge += RuntimePlayer.HAMaxCharge * 0.40;
            //                break;
            //            case RuntimePlayer.HeroAbility.Explosioner:
            //                RuntimePlayer.HAChargePower += 6;
            //                RuntimePlayer.HAMaxCharge += RuntimePlayer.HAMaxCharge * 0.25;
            //                break;
            //            case RuntimePlayer.HeroAbility.Warrior:
            //                RuntimePlayer.HAChargePower += 3;
            //                RuntimePlayer.HAMaxCharge += RuntimePlayer.HAMaxCharge * 0.30;
            //                break;
            //            case RuntimePlayer.HeroAbility.Mage:
            //                RuntimePlayer.HAChargePower += 4;
            //                RuntimePlayer.HAMaxCharge += RuntimePlayer.HAMaxCharge * 0.25;
            //                break;
            //            case RuntimePlayer.HeroAbility.Ninja:
            //                RuntimePlayer.HAChargePower += 5;
            //                RuntimePlayer.HAMaxCharge += RuntimePlayer.HAMaxCharge * 0.35;
            //                break;
            //            default:
            //                // Пусто :P
            //                break;
            //        }
            //    }
            //}
        }

        // Заряд индивидуальной способности персонажа в зависимости от класса
        public void ChargeHeroAbility(RuntimePlayerStats.HeroAbility ability)
        {
            //if (RuntimePlayer.HACharge < RuntimePlayer.HAMaxCharge)
            //{
            //    switch (ability)
            //    {
            //        case RuntimePlayer.HeroAbility.Archer:
            //            RuntimePlayer.HACharge += RuntimePlayer.HAChargePower + RuntimePlayer.Damage * 0.10;
            //            break;
            //        case RuntimePlayer.HeroAbility.Explosioner:
            //            RuntimePlayer.HACharge += RuntimePlayer.HAChargePower + RuntimePlayer.CriticalStrikeChance + 5;
            //            break;
            //        case RuntimePlayer.HeroAbility.Warrior:
            //            RuntimePlayer.HACharge += RuntimePlayer.HAChargePower + RuntimePlayer.MaxHealth * 0.01;
            //            break;
            //        case RuntimePlayer.HeroAbility.Mage:
            //            RuntimePlayer.HACharge += RuntimePlayer.HAChargePower + RuntimePlayer.MaxMana * 0.05;
            //            break;
            //        case RuntimePlayer.HeroAbility.Ninja:
            //            RuntimePlayer.HACharge += RuntimePlayer.HAChargePower + RuntimePlayer.DodgeChance;
            //            break;
            //        default:
            //            RuntimePlayer.HACharge += RuntimePlayer.HAChargePower;
            //            break;
            //    }
            //    if (RuntimePlayer.HACharge >= RuntimePlayer.HAMaxCharge)
            //    {
            //        RuntimePlayer.HACharge = RuntimePlayer.HAMaxCharge;
            //        FindObjectOfType<AbilityButton>().AnimateButton();
            //    }
            //}
        }

        // Использование индивидуальной способности персонажа
        public void UseHeroAbility()
        {
            //if (Player.HACharge >= Player.HAMaxCharge)
            //{
            //    switch (Player.Ability)
            //    {
            //        case Player.HeroAbility.Archer:
            //            if (BattleController.Instance.IsBattle)
            //            {
            //                Player.HACharge = 0;
            //                //BattleController.Instance.allEnemies[0].Health = BattleController.Instance.allEnemies[0].Health * 0.01;
            //                BattleController.Instance.CheckEnemyDeath();
            //            }
            //            break;
            //        case Player.HeroAbility.Explosioner:
            //            if (BattleController.Instance.isBattle)
            //            {
            //                Player.HACharge = 0;
            //                for (int i = 0; i < BattleController.Instance.allEnemies.Count; i++)
            //                {
            //                    BattleController.Instance.allEnemies[i].Health -= 1000 * Player.HALvl;
            //                }
            //            }
            //            break;
            //        case Player.HeroAbility.Warrior:
            //            Player.HACharge = 0;
            //            FindObjectOfType<BuffManager>().SetBuff(Buff.BuffType.WarriorAbility);
            //            break;
            //        case Player.HeroAbility.Mage:
            //            Player.HACharge = 0;
            //            FindObjectOfType<BuffManager>().SetBuff(Buff.BuffType.MageAbility);
            //            if(BattleController.Instance.isBattle)
            //            {
            //                // Если в битве, то пускаем X фаерболлов в противника
            //                for(int i = 0; i < Player.HALvl; i++)
            //                    BattleController.Instance.allEnemies[0].Health -= 100 * Player.HALvl;
            //            }
            //            break;
            //        case Player.HeroAbility.Ninja:
            //            Player.HACharge = 0;
            //            Player.MaxHealth += 50 * Player.HALvl;
            //            Player.Health += 50 * Player.HALvl;
            //            Player.Damage += 5 * Player.HALvl;
            //            Player.AttackSpeed += 1;
            //            break;
            //        default:
            //            // Пусто :P
            //            break;
            //    }
            //    FindObjectOfType<AbilityButton>().StopAnimateButton();
            //}
        }
    }
}
