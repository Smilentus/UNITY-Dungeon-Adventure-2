using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Creatable/Create Character")]
public class Character : ScriptableObject
{
    [Header("Класс персонажа")]
    public CharacterManager.CharacterType Type;

    [Header("Стихия персонажа")]
    public CharacterManager.CharacterElement Element;
    public string elementStr
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

    [Header("Описание персонажа при встрече")]
    public string meetingDescr;
    [Header("Встретил ли игрок этого персонажа?")]
    public bool isMeeted;

    [Header("Имя")]
    public string Name;
    [Header("Здоровье")]
    public double MaxHealth, Health;
    [Header("Реген. здоровья")]
    public double HealthRegen;
    [Header("Тип защиты")]
    public CharacterManager.CharacterArmorType ArmorType;
    [Header("Защита")]
    public double Armor;
    [Header("Мана")]
    public double MaxMana, Mana;
    [Header("Заклинания персонажа")]
    public CharacterMagic[] Spells;
    [Header("Тип атаки")]
    public CharacterManager.CharacterAttackType AttackType;
    [Header("Скорость атаки")]
    public double AttackSpeed;
    [Header("Атака")]
    public double Attack;
    [Header("Шанс увернуться | <= чем указанное число")]
    public double DodgeChance;
    [Header("Оглушен ли персонаж")]
    public bool isStun;
    [Header("Опыт")]
    public double Exp;
    [Header("Золото")]
    public double Gold;
    [Header("Дроп")]
    public Item[] DropItems;

    [Header("Текстура персонажа")]
    public Texture CharacterImage;

    [HideInInspector()]
    public int lastMagicPriority;
}
