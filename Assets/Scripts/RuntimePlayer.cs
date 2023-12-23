// Пасхалка разрабов! Ы, Гусь Шында ХунГра-Да! <-- Под кайфом наверно был, когда писал ...
using Newtonsoft.Json;
using System;
using UnityEngine;


/// <summary>
///     Здесь хранятся все основные переменные, относящиеся к нашему игроку!
///     Конкретно к игровому персонажу, которым мы управляем по факту
/// </summary>
public class RuntimePlayer : MonoBehaviour
{
    private static RuntimePlayer instance;
    public static RuntimePlayer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<RuntimePlayer>(true);   
            }

            return instance;
        }
    }


    public event Action onRuntimePlayerStatsUpdated;


    private RuntimePlayerStats runtimePlayerStats;
    /// <summary>
    ///     Текущие характеристики игрового персонажа
    /// </summary>
    public RuntimePlayerStats RuntimePlayerStats { 
        get
        {
            return runtimePlayerStats;
        }
        set
        {
            runtimePlayerStats = value;
            onRuntimePlayerStatsUpdated?.Invoke();
        }
    }


    private void Awake()
    {
        runtimePlayerStats = new RuntimePlayerStats();
    }


    public void SetDefaultPlayerStats()
    {
        runtimePlayerStats.Health = 100;
        runtimePlayerStats.MaxHealth = 100;

        runtimePlayerStats.PlayerName = "";
        runtimePlayerStats.isFirstEnter = true;
        runtimePlayerStats.MaxHealth = 100;
        runtimePlayerStats.Health = 100;
        runtimePlayerStats.HealthRegen = 0;
        runtimePlayerStats.MaxMana = 0;
        runtimePlayerStats.ManaRegen = 0;
        runtimePlayerStats.Mana = 0;
        runtimePlayerStats.Armor = 0;
        runtimePlayerStats.Damage = 10;
        runtimePlayerStats.AttackSpeed = 1;
        runtimePlayerStats.isStun = false;

        runtimePlayerStats.tempDamage = 0;
        runtimePlayerStats.tempHealth = 0;
        runtimePlayerStats.tempMaxHealth = 0;

        runtimePlayerStats.AntiHole = false;
        runtimePlayerStats.Luck = 0;
        runtimePlayerStats.SkillPoints = 0;
        runtimePlayerStats.ChanceNotToDelete = 0;
        runtimePlayerStats.ChanceToCraftTwice = 0;
        runtimePlayerStats.openedInvCases = 19;
        runtimePlayerStats.openedRuneCases = 0;

        runtimePlayerStats.Lvl = 1;
        runtimePlayerStats.Exp = 0;
        runtimePlayerStats.MaxExp = 50;
        runtimePlayerStats.ExpMulty = 50;
        runtimePlayerStats.Money = 0;

        runtimePlayerStats.ExtraExpMod = 0;
        runtimePlayerStats.ExtraMoneyMod = 0;

        // Различные шансы
        runtimePlayerStats.DodgeChance = 10;
        runtimePlayerStats.LightStrikeChance = 95;
        runtimePlayerStats.MediumStrikeChance = 65;
        runtimePlayerStats.HeavyStrikeChance = 30;
        runtimePlayerStats.CriticalStrikeChance = 1;
        runtimePlayerStats.CriticalStrikeMulty = 2;

        runtimePlayerStats.isHadVillage = false;

        runtimePlayerStats.AttackType = CharactersLibrary.CharacterAttackType.Melee;
        runtimePlayerStats.ArmorType = CharactersLibrary.CharacterArmorType.None;
        runtimePlayerStats.Element = CharactersLibrary.CharacterElement.None;
    }


    // TODO: Временное решение для переработки
    private void Update()
    {
        if (runtimePlayerStats.Health > runtimePlayerStats.MaxHealth)
        {
            runtimePlayerStats.Health = runtimePlayerStats.MaxHealth;
        }
        if (runtimePlayerStats.Health < 0)
        {
            runtimePlayerStats.Health = 0;
        }


        if (runtimePlayerStats.Mana >= runtimePlayerStats.MaxMana)
        {
            runtimePlayerStats.Mana = runtimePlayerStats.MaxMana;
        }
        if (runtimePlayerStats.Mana < 0)
        {
            runtimePlayerStats.Mana = 0;
        }
    }


    public void DealDamage(double _damage, bool _ignoreArmor)
    {
        if (_ignoreArmor)
        {
            RuntimePlayer.Instance.RuntimePlayerStats.Health -= _damage;
            GameController.Instance.AddEventText("Вы получили урон через броню: " + _damage);
        }
        else
        {
            if (_damage > RuntimePlayer.Instance.RuntimePlayerStats.Armor)
            {
                RuntimePlayer.Instance.RuntimePlayerStats.Health -= (_damage - RuntimePlayer.Instance.RuntimePlayerStats.Armor);
                GameController.Instance.AddEventText("Вы получили урон: " + (_damage - RuntimePlayer.Instance.RuntimePlayerStats.Armor));
            }
            else
                GameController.Instance.AddEventText("Броня заблокировала урон.");
        }
    }

    public void GiveExperience(double _experience)
    {
        // Подсчёт доп. процентного опыта
        double extraExp = _experience * (RuntimePlayer.Instance.RuntimePlayerStats.ExtraExpMod / 100);
        // Опыт, который дадим
        _experience += extraExp;
        RuntimePlayer.Instance.RuntimePlayerStats.Exp += _experience;

        string info = "Получено: " + _experience + " ед. опыта!";
        if (RuntimePlayer.Instance.RuntimePlayerStats.ExtraExpMod > 0)
        {
            info = "Получено: " + _experience + " + (" + extraExp + ") ед. опыта.";
        }

        GameController.Instance.AddEventText(info);

        while (RuntimePlayer.Instance.RuntimePlayerStats.Exp >= RuntimePlayer.Instance.RuntimePlayerStats.MaxExp)
        {
            RuntimePlayer.Instance.RuntimePlayerStats.Lvl++;
            RuntimePlayer.Instance.RuntimePlayerStats.SkillPoints += 5;
            RuntimePlayer.Instance.RuntimePlayerStats.Exp -= RuntimePlayer.Instance.RuntimePlayerStats.MaxExp;
            RuntimePlayer.Instance.RuntimePlayerStats.MaxExp += RuntimePlayer.Instance.RuntimePlayerStats.ExpMulty;
            RuntimePlayer.Instance.RuntimePlayerStats.ExpMulty += 1;
            GameController.Instance.AddEventText("Новый уровень - " + RuntimePlayer.Instance.RuntimePlayerStats.Lvl + "!");
        }
    }

    public void GiveMoney(double _money)
    {
        double extraMoney = _money * RuntimePlayer.Instance.RuntimePlayerStats.ExtraMoneyMod / 100;

        _money += extraMoney;
        RuntimePlayer.Instance.RuntimePlayerStats.Money += (int)_money;

        string info = "Получено: " + _money + " ед. золота!";
        if (RuntimePlayer.Instance.RuntimePlayerStats.ExtraMoneyMod > 0)
        {
            info = "Получено: " + _money + " + (" + extraMoney + ") ед. золота.";
        }

        GameController.Instance.AddEventText(info);
    }


    public void PerformHealthRegeneration()
    {
        runtimePlayerStats.Health += runtimePlayerStats.HealthRegen;
    }

    public void PerformManaRegeneration()
    {
        runtimePlayerStats.Mana += runtimePlayerStats.ManaRegen;
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
                case CharactersLibrary.CharacterElement.Dark:
                    str = "Тьма";
                    break;
                case CharactersLibrary.CharacterElement.Earth:
                    str = "Земля";
                    break;
                case CharactersLibrary.CharacterElement.Fire:
                    str = "Огонь";
                    break;
                case CharactersLibrary.CharacterElement.Light:
                    str = "Свет";
                    break;
                case CharactersLibrary.CharacterElement.None:
                    str = "Нет стихии";
                    break;
                case CharactersLibrary.CharacterElement.Water:
                    str = "Вода";
                    break;
                case CharactersLibrary.CharacterElement.Wind:
                    str = "Воздух";
                    break;
            }
            return str;
        }
    }

    public bool isFirstEnter;

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

    public double ExtraExpMod;
    public double ExtraExpMulty;
    public double ExtraMoneyMod;
    public double ExtraMoneyMulty;
    public int SkillPoints;


    public bool AntiHole;
    public double Luck;
    public double Money;
    public double ChanceNotToDelete;
    public double ChanceToCraftTwice;
    public int openedInvCases;
    public int openedRuneCases;


    public double DodgeChance;
    public double LightStrikeChance;
    public double MediumStrikeChance;
    public double HeavyStrikeChance;
    public double CriticalStrikeChance;
    public double CriticalStrikeMulty;

    public bool isDeath;
    public bool isHadVillage;
    
    [JsonIgnore]
    public string abilityStr
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

    public bool isStun;
    public double HAChargePower;
    public double HACharge;
    public double HAMaxCharge;
    public int HALvl;
    public int HAMaxLvl;
    public HeroAbility Ability;

    public CharactersLibrary.CharacterElement Element;
    public CharactersLibrary.CharacterArmorType ArmorType;
    public CharactersLibrary.CharacterAttackType AttackType;


    public double tempHealth, tempMaxHealth, tempDamage, tempMaxMana, tempMana, tempAttackSpeed, tempArmor;
}