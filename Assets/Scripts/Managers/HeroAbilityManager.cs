using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroAbilityManager : MonoBehaviour
{
    [Header("Панель использования абилки")]
    public GameObject abilityPanel;

    private void Start()
    {
        if (Player.HALvl > 0)
            abilityPanel.SetActive(true);
        else
            abilityPanel.SetActive(false);
    }

    // Сброс абилки 
    public void ResetHeroAbility()
    {
        Player.HALvl = 0;
        Player.HAMaxLvl = 10;
        Player.HAChargePower = 0;
        Player.HACharge = 0;
        Player.HAMaxCharge = 0;
        Player.Ability = Player.HeroAbility.None;
        abilityPanel.SetActive(false);
    }

    // Улучшение абилки
    public void UpgradeHeroAbility()
    {
        if (Player.HALvl < Player.HAMaxLvl && Player.HACharge >= Player.HAMaxCharge)
        {
            if (Player.HALvl == 0)
            {
                Player.HALvl++;
                switch (Player.Ability)
                {
                    case Player.HeroAbility.Archer:
                        Player.HAChargePower = 1;
                        Player.HAMaxCharge = 325;
                        break;
                    case Player.HeroAbility.Explosioner:
                        Player.HAChargePower = 3;
                        Player.HAMaxCharge = 250;
                        break;
                    case Player.HeroAbility.Mage:
                        Player.HAChargePower = 4;
                        Player.HAMaxCharge = 150;
                        break;
                    case Player.HeroAbility.Ninja:
                        Player.HAChargePower = 3;
                        Player.HAMaxCharge = 500;
                        break;
                    case Player.HeroAbility.Warrior:
                        Player.HAChargePower = 2;
                        Player.HAMaxCharge = 400;
                        break;
                    default:
                        break;
                }
                abilityPanel.SetActive(true);
            }
            else
            {
                Player.HALvl++;
                switch (Player.Ability)
                {
                    case Player.HeroAbility.Archer:
                        Player.HAChargePower += 1;
                        Player.HAMaxCharge += Player.HAMaxCharge * 0.40;
                        break;
                    case Player.HeroAbility.Explosioner:
                        Player.HAChargePower += 6;
                        Player.HAMaxCharge += Player.HAMaxCharge * 0.25;
                        break;
                    case Player.HeroAbility.Warrior:
                        Player.HAChargePower += 3;
                        Player.HAMaxCharge += Player.HAMaxCharge * 0.30;
                        break;
                    case Player.HeroAbility.Mage:
                        Player.HAChargePower += 4;
                        Player.HAMaxCharge += Player.HAMaxCharge * 0.25;
                        break;
                    case Player.HeroAbility.Ninja:
                        Player.HAChargePower += 5;
                        Player.HAMaxCharge += Player.HAMaxCharge * 0.35;
                        break;
                    default:
                        // Пусто :P
                        break;
                }
            }
        }
    }

    // Заряд индивидуальной способности персонажа в зависимости от класса
    public void ChargeHeroAbility(Player.HeroAbility ability)
    {
        if (Player.HACharge < Player.HAMaxCharge)
        {
            switch (ability)
            {
                case Player.HeroAbility.Archer:
                    Player.HACharge += Player.HAChargePower + Player.Damage * 0.10;
                    break;
                case Player.HeroAbility.Explosioner:
                    Player.HACharge += Player.HAChargePower + Player.CriticalStrikeChance + 5;
                    break;
                case Player.HeroAbility.Warrior:
                    Player.HACharge += Player.HAChargePower + Player.MaxHealth * 0.01;
                    break;
                case Player.HeroAbility.Mage:
                    Player.HACharge += Player.HAChargePower + Player.MaxMana * 0.05;
                    break;
                case Player.HeroAbility.Ninja:
                    Player.HACharge += Player.HAChargePower + Player.DodgeChance;
                    break;
                default:
                    Player.HACharge += Player.HAChargePower;
                    break;
            }
            if (Player.HACharge >= Player.HAMaxCharge)
            {
                Player.HACharge = Player.HAMaxCharge;
                FindObjectOfType<AbilityButton>().AnimateButton();
            }
        }
    }

    // Использование индивидуальной способности персонажа
    public void UseHeroAbility()
    {
        if (Player.HACharge >= Player.HAMaxCharge)
        {
            switch (Player.Ability)
            {
                case Player.HeroAbility.Archer:
                    if (BattleHelper._BH.isBattle)
                    {
                        Player.HACharge = 0;
                        BattleHelper._BH.allEnemies[0].Health = BattleHelper._BH.allEnemies[0].Health * 0.01;
                        FindObjectOfType<BattleHelper>().CheckEnemyDeath();
                    }
                    break;
                case Player.HeroAbility.Explosioner:
                    if (BattleHelper._BH.isBattle)
                    {
                        Player.HACharge = 0;
                        var helper = BattleHelper._BH;
                        for (int i = 0; i < helper.allEnemies.Count; i++)
                        {
                            helper.allEnemies[i].Health -= 1000 * Player.HALvl;
                        }
                    }
                    break;
                case Player.HeroAbility.Warrior:
                    Player.HACharge = 0;
                    FindObjectOfType<BuffManager>().SetBuff(Buff.BuffType.WarriorAbility);
                    break;
                case Player.HeroAbility.Mage:
                    Player.HACharge = 0;
                    FindObjectOfType<BuffManager>().SetBuff(Buff.BuffType.MageAbility);
                    if(BattleHelper._BH.isBattle)
                    {
                        // Если в битве, то пускаем X фаерболлов в противника
                        for(int i = 0; i < Player.HALvl; i++)
                            BattleHelper._BH.allEnemies[0].Health -= 100 * Player.HALvl;
                    }
                    break;
                case Player.HeroAbility.Ninja:
                    Player.HACharge = 0;
                    Player.MaxHealth += 50 * Player.HALvl;
                    Player.Health += 50 * Player.HALvl;
                    Player.Damage += 5 * Player.HALvl;
                    Player.AttackSpeed += 1;
                    break;
                default:
                    // Пусто :P
                    break;
            }
            FindObjectOfType<AbilityButton>().StopAnimateButton();
        }
    }
}
