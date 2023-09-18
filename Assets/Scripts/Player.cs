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

    public static CharactersLibrary.CharacterElement Element;
    public static string elementStr
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
    public static string Name;
    public static double MaxHealth, Health;
    public static double HealthRegen;
    public static CharactersLibrary.CharacterArmorType ArmorType;
    public static double Armor;
    public static double MaxMana, Mana;
    public static double ManaRegen;
    public static CharactersLibrary.CharacterAttackType AttackType;
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
