// Пасхалка разрабов! Ы, Гусь Шында ХунГра-Да! <-- Под кайфом наверно был, когда писал ...

public class Player
{
    public enum HeroAbility
    {
        None,
        Warrior,
        Mage,
        Ninja,
        Archer,
        Explosioner
    }

    public static CharacterManager.CharacterElement Element;
    public static string elementStr
    {
        get
        {
            string str = "";
            switch (Element)
            {
                case CharacterManager.CharacterElement.Dark:
                    str = "Тьма";
                    break;
                case CharacterManager.CharacterElement.Earth:
                    str = "Земля";
                    break;
                case CharacterManager.CharacterElement.Fire:
                    str = "Огонь";
                    break;
                case CharacterManager.CharacterElement.Light:
                    str = "Свет";
                    break;
                case CharacterManager.CharacterElement.None:
                    str = "Нет стихии";
                    break;
                case CharacterManager.CharacterElement.Water:
                    str = "Вода";
                    break;
                case CharacterManager.CharacterElement.Wind:
                    str = "Воздух";
                    break;
            }
            return str;
        }
    }
    public static string Name;
    public static double MaxHealth, Health;
    public static double HealthRegen;
    public static CharacterManager.CharacterArmorType ArmorType;
    public static double Armor;
    public static double MaxMana, Mana;
    public static double ManaRegen;
    public static CharacterManager.CharacterAttackType AttackType;
    public static double AttackSpeed;
    public static double Damage;

    // Первый ли раз мы зашли в игру
    public static bool isFirstEnter;

    // Статистика для баффов
    public static double tempHealth, tempMaxHealth, tempDamage, tempMaxMana, tempMana, tempAttackSpeed, tempArmor;

    // Скиллы
    public static bool AntiHole;
    public static double Luck;
    public static double Money;
    public static double ChanceNotToDelete;
    public static double ChanceToCraftTwice;
    public static int openedInvCases;
    public static int openedRuneCases;

    // Индивидуальная абилка каждого класса (HA - HeroAbility)
    public static double HAChargePower;
    public static double HACharge;
    public static double HAMaxCharge;
    public static int HALvl;
    public static int HAMaxLvl;
    public static HeroAbility Ability;
    public static string abilityStr
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
    public static bool isStun;

    // Уровень
    public static double Lvl;
    public static double Exp;
    public static double MaxExp;
    public static double ExpMulty;

    public static double ExtraExpMod;
    public static double ExtraExpMulty;
    public static double ExtraMoneyMod;
    public static double ExtraMoneyMulty;

    // Очки навыков
    public static int SkillPoints;

    // Статы для битвы
    public static double DodgeChance;
    public static double LightStrikeChance;
    public static double MediumStrikeChance;
    public static double HeavyStrikeChance;
    public static double CriticalStrikeChance;
    public static double CriticalStrikeMulty;

    public static bool isDeath;
    public static bool isHadVillage;
}
