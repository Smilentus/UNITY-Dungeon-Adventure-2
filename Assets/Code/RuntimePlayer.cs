using Dimasyechka.Code.BattleSystem;
using Newtonsoft.Json;
using System;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code
{
    /// <summary>
    ///     Здесь хранятся все основные переменные, относящиеся к нашему игроку!
    ///     Конкретно к игровому персонажу, которым мы управляем по факту
    /// </summary>
    public class RuntimePlayer : MonoBehaviour
    {
        public event Action onRuntimePlayerStatsUpdated;


        private RuntimePlayerStats _runtimePlayerStats;
        /// <summary>
        ///     Текущие характеристики игрового персонажа
        /// </summary>
        public RuntimePlayerStats RuntimePlayerStats
        {
            get
            {
                return _runtimePlayerStats;
            }
            set
            {
                _runtimePlayerStats = value;
                onRuntimePlayerStatsUpdated?.Invoke();
            }
        }


        private RuntimePlayer _runtimePlayer;

        [Inject]
        public void Construct(RuntimePlayer runtimePlayer)
        {
            _runtimePlayer = runtimePlayer;
        }


        private void Awake()
        {
            _runtimePlayerStats = new RuntimePlayerStats();
        }


        public void SetDefaultPlayerStats()
        {
            _runtimePlayerStats.Health = 100;
            _runtimePlayerStats.MaxHealth = 100;

            _runtimePlayerStats.PlayerName = "";
            _runtimePlayerStats.IsFirstEnter = true;
            _runtimePlayerStats.MaxHealth = 100;
            _runtimePlayerStats.Health = 100;
            _runtimePlayerStats.HealthRegen = 0;
            _runtimePlayerStats.MaxMana = 0;
            _runtimePlayerStats.ManaRegen = 0;
            _runtimePlayerStats.Mana = 0;
            _runtimePlayerStats.Armor = 0;
            _runtimePlayerStats.Damage = 10;
            _runtimePlayerStats.AttackSpeed = 1;
            _runtimePlayerStats.IsStun = false;

            _runtimePlayerStats.TempDamage = 0;
            _runtimePlayerStats.TempHealth = 0;
            _runtimePlayerStats.TempMaxHealth = 0;

            _runtimePlayerStats.AntiHole = false;
            _runtimePlayerStats.Luck = 0;
            _runtimePlayerStats.SkillPoints = 0;
            _runtimePlayerStats.ChanceNotToDelete = 0;
            _runtimePlayerStats.ChanceToCraftTwice = 0;
            _runtimePlayerStats.OpenedInvCases = 19;
            _runtimePlayerStats.OpenedRuneCases = 0;

            _runtimePlayerStats.Lvl = 1;
            _runtimePlayerStats.Exp = 0;
            _runtimePlayerStats.MaxExp = 50;
            _runtimePlayerStats.ExpMulty = 50;
            _runtimePlayerStats.Money = 0;

            _runtimePlayerStats.ExtraExpMultiplier = 0;
            _runtimePlayerStats.ExtraMoneyMultiplier = 0;

            // Различные шансы
            _runtimePlayerStats.DodgeChance = 10;
            _runtimePlayerStats.LightStrikeChance = 95;
            _runtimePlayerStats.MediumStrikeChance = 65;
            _runtimePlayerStats.HeavyStrikeChance = 30;
            _runtimePlayerStats.CriticalStrikeChance = 1;
            _runtimePlayerStats.CriticalStrikeDamageMultiplier = 2;

            _runtimePlayerStats.IsHadVillage = false;

            _runtimePlayerStats.AttackType = CharacterAttackType.Melee;
            _runtimePlayerStats.ArmorType = CharacterArmorType.None;
            _runtimePlayerStats.Element = CharacterElement.None;
        }


        // TODO: Временное решение для переработки
        private void Update()
        {
            if (_runtimePlayerStats.Health > _runtimePlayerStats.MaxHealth)
            {
                _runtimePlayerStats.Health = _runtimePlayerStats.MaxHealth;
            }
            if (_runtimePlayerStats.Health < 0)
            {
                _runtimePlayerStats.Health = 0;
            }


            if (_runtimePlayerStats.Mana >= _runtimePlayerStats.MaxMana)
            {
                _runtimePlayerStats.Mana = _runtimePlayerStats.MaxMana;
            }
            if (_runtimePlayerStats.Mana < 0)
            {
                _runtimePlayerStats.Mana = 0;
            }
        }


        public void DealDamage(double damage, bool ignoreArmor)
        {
            if (ignoreArmor)
            {
                _runtimePlayer.RuntimePlayerStats.Health -= damage;
                GameController.Instance.AddEventText("Вы получили урон через броню: " + damage.ToString("f2"));
            }
            else
            {
                if (damage > _runtimePlayer.RuntimePlayerStats.Armor)
                {
                    _runtimePlayer.RuntimePlayerStats.Health -= (damage - _runtimePlayer.RuntimePlayerStats.Armor);
                    GameController.Instance.AddEventText("Вы получили урон: " + (damage - _runtimePlayer.RuntimePlayerStats.Armor).ToString("f2"));
                }
                else
                    GameController.Instance.AddEventText("Броня заблокировала урон.");
            }
        }

        public void GiveExperience(double experience)
        {
            // Подсчёт доп. процентного опыта
            double extraExp = (experience * _runtimePlayer.RuntimePlayerStats.ExtraExpMultiplier);

            // Опыт, который дадим
            experience += extraExp;
            _runtimePlayer.RuntimePlayerStats.Exp += experience;

            string info = "Получено: " + experience.ToString("f2") + " ед. опыта!";
            if (_runtimePlayer.RuntimePlayerStats.ExtraExpMultiplier > 0)
            {
                info = "Получено: " + experience.ToString("f2") + " + (" + extraExp.ToString("f2") + ") ед. опыта.";
            }

            GameController.Instance.AddEventText(info);

            while (_runtimePlayer.RuntimePlayerStats.Exp >= _runtimePlayer.RuntimePlayerStats.MaxExp)
            {
                _runtimePlayer.RuntimePlayerStats.Lvl++;
                _runtimePlayer.RuntimePlayerStats.SkillPoints += 5;
                _runtimePlayer.RuntimePlayerStats.Exp -= _runtimePlayer.RuntimePlayerStats.MaxExp;
                _runtimePlayer.RuntimePlayerStats.MaxExp += _runtimePlayer.RuntimePlayerStats.ExpMulty;
                _runtimePlayer.RuntimePlayerStats.ExpMulty += 1;
                GameController.Instance.AddEventText("Новый уровень - " + _runtimePlayer.RuntimePlayerStats.Lvl + "!");
            }
        }

        public void GiveMoney(double money)
        {
            double extraMoney = money * _runtimePlayer.RuntimePlayerStats.ExtraMoneyMultiplier;

            money += extraMoney;
            _runtimePlayer.RuntimePlayerStats.Money += (int)money;

            string info = "Получено: " + money.ToString("f2") + " ед. золота!";
            if (_runtimePlayer.RuntimePlayerStats.ExtraMoneyMultiplier > 0)
            {
                info = "Получено: " + money.ToString("f2") + " + (" + extraMoney.ToString("f2") + ") ед. золота.";
            }

            GameController.Instance.AddEventText(info);
        }


        public void PerformHealthRegeneration()
        {
            _runtimePlayerStats.Health += _runtimePlayerStats.HealthRegen;
        }

        public void PerformManaRegeneration()
        {
            _runtimePlayerStats.Mana += _runtimePlayerStats.ManaRegen;
        }
    }

    [System.Serializable]
    public class RuntimePlayerStats
    {
        [JsonIgnore]
        public string elementStr
        {
            get
            {
                string str = "";
                switch (Element)
                {
                    case CharacterElement.Dark:
                        str = "Тьма";
                        break;
                    case CharacterElement.Earth:
                        str = "Земля";
                        break;
                    case CharacterElement.Fire:
                        str = "Огонь";
                        break;
                    case CharacterElement.Light:
                        str = "Свет";
                        break;
                    case CharacterElement.None:
                        str = "Нет стихии";
                        break;
                    case CharacterElement.Water:
                        str = "Вода";
                        break;
                    case CharacterElement.Wind:
                        str = "Воздух";
                        break;
                }
                return str;
            }
        }

        public bool IsFirstEnter;

        public enum HeroAbility
        {
            None,
            Warrior,
            Mage,
            Ninja,
            Archer,
            Explosioner
        }

        public string PlayerName;

        public double Health;
        public double MaxHealth;
        public double HealthRegen;

        public double Armor;

        public double Mana;
        public double MaxMana;
        public double ManaRegen;

        public double AttackSpeed;
        public double Damage;


        public double Lvl;
        public double Exp;
        public double MaxExp;
        public double ExpMulty;

        public double ExtraExpMultiplier;
        public double ExtraMoneyMultiplier;
        public int SkillPoints;


        public bool AntiHole;
        public double Luck;
        public double Money;
        public double ChanceNotToDelete;
        public double ChanceToCraftTwice;
        public int OpenedInvCases;
        public int OpenedRuneCases;


        public double DodgeChance;
        public double LightStrikeChance;
        public double MediumStrikeChance;
        public double HeavyStrikeChance;
        public double CriticalStrikeChance;
        public double CriticalStrikeDamageMultiplier;

        public bool IsDeath;
        public bool IsHadVillage;

        [JsonIgnore]
        public string GetAbility
        {
            get
            {
                switch (Ability)
                {
                    case HeroAbility.Archer:
                        return "Лучник";
                    case HeroAbility.Warrior:
                        return "Воин";
                    case HeroAbility.Explosioner:
                        return "Подрывник";
                    case HeroAbility.Mage:
                        return "Маг";
                    case HeroAbility.Ninja:
                        return "Ниндзя";
                    default:
                        return "";
                }
            }
        }

        public bool IsStun;
        public double HaChargePower;
        public double HaCharge;
        public double HaMaxCharge;
        public int HaLvl;
        public int HaMaxLvl;
        public HeroAbility Ability;

        public CharacterElement Element;
        public CharacterArmorType ArmorType;
        public CharacterAttackType AttackType;


        public double TempHealth, TempMaxHealth, TempDamage, TempMaxMana, TempMana, TempAttackSpeed, TempArmor;
    }
}