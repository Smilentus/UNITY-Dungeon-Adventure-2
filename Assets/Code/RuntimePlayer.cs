using System;
using UniRx;
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


        private RuntimePlayerStats _runtimePlayerStats = new RuntimePlayerStats();
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


        // TODO: Временное решение для переработки
        private void Update()
        {
            if (_runtimePlayerStats.Health.Value > _runtimePlayerStats.MaxHealth.Value)
            {
                _runtimePlayerStats.Health.Value = _runtimePlayerStats.MaxHealth.Value;
            }
            if (_runtimePlayerStats.Health.Value < 0)
            {
                _runtimePlayerStats.Health.Value = 0;
            }


            if (_runtimePlayerStats.Mana.Value >= _runtimePlayerStats.MaxMana.Value)
            {
                _runtimePlayerStats.Mana.Value = _runtimePlayerStats.MaxMana.Value;
            }
            if (_runtimePlayerStats.Mana.Value < 0)
            {
                _runtimePlayerStats.Mana.Value = 0;
            }
        }


        public void SetDefaultPlayerStats()
        {
            //_runtimePlayerStats.IsStun = false;
            //_runtimePlayerStats.IsFirstEnter = true;
            //_runtimePlayerStats.AntiHole = false;
            //_runtimePlayerStats.ChanceNotToDelete = 0;
            //_runtimePlayerStats.ChanceToCraftTwice = 0;
            //_runtimePlayerStats.OpenedInvCases = 19;
            //_runtimePlayerStats.OpenedRuneCases = 0;
            //_runtimePlayerStats.IsHadVillage = false;
            ////_runtimePlayerStats.AttackType.Value = CharacterAttackType.Melee;
            //_runtimePlayerStats.ArmorType.Value = CharacterArmorType.None;
            //_runtimePlayerStats.Element.Value = CharacterElement.None;
            
            _runtimePlayerStats.Health.Value = 100;
            _runtimePlayerStats.MaxHealth.Value = 100;
            _runtimePlayerStats.HealthRegen.Value = 0;

            _runtimePlayerStats.Mana.Value = 0;
            _runtimePlayerStats.MaxMana.Value = 0;
            _runtimePlayerStats.ManaRegen.Value = 0;

            _runtimePlayerStats.Armor.Value = 0;
            _runtimePlayerStats.Damage.Value = 10;
            _runtimePlayerStats.AttackSpeed.Value = 1;

            _runtimePlayerStats.Luck.Value = 0;
            _runtimePlayerStats.SkillPoints.Value = 0;


            _runtimePlayerStats.Lvl.Value = 1;
            _runtimePlayerStats.Exp.Value = 0;
            _runtimePlayerStats.MaxExp.Value = 50;
            _runtimePlayerStats.ExpMulty.Value = 50;
            _runtimePlayerStats.Money.Value = 0;

            _runtimePlayerStats.ExtraExpMultiplier.Value = 0;
            _runtimePlayerStats.ExtraMoneyMultiplier.Value = 0;

            _runtimePlayerStats.DodgeChance.Value = 10;
            _runtimePlayerStats.LightStrikeChance.Value = 95;
            _runtimePlayerStats.MediumStrikeChance.Value = 65;
            _runtimePlayerStats.HeavyStrikeChance.Value = 30;
            _runtimePlayerStats.CriticalStrikeChance.Value = 1;
            _runtimePlayerStats.CriticalStrikeDamageMultiplier.Value = 2;
        }


        public void DealDamage(double damage, bool ignoreArmor)
        {
            if (ignoreArmor)
            {
                _runtimePlayer.RuntimePlayerStats.Health.Value -= damage;
                //GameController.Instance.AddEventText("Вы получили урон через броню: " + damage.ToString("f2"));
            }
            else
            {
                if (damage > _runtimePlayer.RuntimePlayerStats.Armor.Value)
                {
                    _runtimePlayer.RuntimePlayerStats.Health.Value -= (damage - _runtimePlayer.RuntimePlayerStats.Armor.Value);
                    //GameController.Instance.AddEventText("Вы получили урон: " + (damage - _runtimePlayer.RuntimePlayerStats.Armor.Value).ToString("f2"));
                }
                else
                {
                    //GameController.Instance.AddEventText("Броня заблокировала урон.");
                }
            }
        }

        public void GiveExperience(double experience)
        {
            // Подсчёт доп. процентного опыта
            double extraExp = (experience * _runtimePlayer.RuntimePlayerStats.ExtraExpMultiplier.Value);

            // Опыт, который дадим
            experience += extraExp;
            _runtimePlayer.RuntimePlayerStats.Exp.Value += experience;

            string info = "Получено: " + experience.ToString("f2") + " ед. опыта!";
            if (_runtimePlayer.RuntimePlayerStats.ExtraExpMultiplier.Value > 0)
            {
                info = "Получено: " + experience.ToString("f2") + " + (" + extraExp.ToString("f2") + ") ед. опыта.";
            }

            //GameController.Instance.AddEventText(info);

            while (_runtimePlayer.RuntimePlayerStats.Exp.Value >= _runtimePlayer.RuntimePlayerStats.MaxExp.Value)
            {
                _runtimePlayer.RuntimePlayerStats.Lvl.Value++;
                _runtimePlayer.RuntimePlayerStats.SkillPoints.Value += 5;
                _runtimePlayer.RuntimePlayerStats.Exp.Value -= _runtimePlayer.RuntimePlayerStats.MaxExp.Value;
                _runtimePlayer.RuntimePlayerStats.MaxExp.Value += _runtimePlayer.RuntimePlayerStats.ExpMulty.Value;
                _runtimePlayer.RuntimePlayerStats.ExpMulty.Value += 1;
                //GameController.Instance.AddEventText("Новый уровень - " + _runtimePlayer.RuntimePlayerStats.Lvl.Value + "!");
            }
        }

        public void GiveMoney(double money)
        {
            double extraMoney = money * _runtimePlayer.RuntimePlayerStats.ExtraMoneyMultiplier.Value;

            money += extraMoney;
            _runtimePlayer.RuntimePlayerStats.Money.Value += (int)money;

            string info = "Получено: " + money.ToString("f2") + " ед. золота!";
            if (_runtimePlayer.RuntimePlayerStats.ExtraMoneyMultiplier.Value > 0)
            {
                info = "Получено: " + money.ToString("f2") + " + (" + extraMoney.ToString("f2") + ") ед. золота.";
            }

            //GameController.Instance.AddEventText(info);
        }


        public void PerformHealthRegeneration()
        {
            _runtimePlayerStats.Health.Value += _runtimePlayerStats.HealthRegen.Value;
        }

        public void PerformManaRegeneration()
        {
            _runtimePlayerStats.Mana.Value += _runtimePlayerStats.ManaRegen.Value;
        }
    }


    [System.Serializable]
    public class RuntimePlayerStats
    {
        public ReactiveProperty<double> Health = new ReactiveProperty<double>();
        public ReactiveProperty<double> HealthRegen = new ReactiveProperty<double>();
        public ReactiveProperty<double> MaxHealth = new ReactiveProperty<double>();

        public double HealthRatio => MaxHealth.Value != 0 ? Health.Value / MaxHealth.Value : 0;

        public ReactiveProperty<double> Mana = new ReactiveProperty<double>();
        public ReactiveProperty<double> ManaRegen = new ReactiveProperty<double>();
        public ReactiveProperty<double> MaxMana = new ReactiveProperty<double>();

        public double ManaRatio => MaxMana.Value != 0 ? Mana.Value / MaxMana.Value : 0;

        public ReactiveProperty<double> Damage = new ReactiveProperty<double>();
        public ReactiveProperty<double> Armor = new ReactiveProperty<double>();

        public ReactiveProperty<double> AttackSpeed = new ReactiveProperty<double>();

        public ReactiveProperty<double> Lvl = new ReactiveProperty<double>();
        public ReactiveProperty<double> Exp = new ReactiveProperty<double>();
        public ReactiveProperty<double> MaxExp = new ReactiveProperty<double>();

        public double ExpRatio => MaxExp.Value != 0 ? Exp.Value / MaxExp.Value : 0;

        public ReactiveProperty<double> ExpMulty = new ReactiveProperty<double>();
        public ReactiveProperty<double> ExtraExpMultiplier = new ReactiveProperty<double>();

        public ReactiveProperty<double> Luck = new ReactiveProperty<double>();

        public ReactiveProperty<double> Money = new ReactiveProperty<double>();
        public ReactiveProperty<double> ExtraMoneyMultiplier = new ReactiveProperty<double>();

        public ReactiveProperty<int> SkillPoints = new ReactiveProperty<int>();

        public ReactiveProperty<double> DodgeChance = new ReactiveProperty<double>();

        public ReactiveProperty<double> LightStrikeChance = new ReactiveProperty<double>();
        public ReactiveProperty<double> MediumStrikeChance = new ReactiveProperty<double>();
        public ReactiveProperty<double> HeavyStrikeChance = new ReactiveProperty<double>();

        public ReactiveProperty<double> CriticalStrikeChance = new ReactiveProperty<double>();
        public ReactiveProperty<double> CriticalStrikeDamageMultiplier = new ReactiveProperty<double>();
    }
}