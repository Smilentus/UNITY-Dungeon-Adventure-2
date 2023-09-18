using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Creatable/Create Character")]
public class CharacterProfile : ScriptableObject
{
    [SerializeField]
    private RuntimeBattleCharacter m_characterPrefab;
    /// <summary>
    ///     Ссылка на основной префаб персонажа, который спавнится во время боя
    /// </summary>
    public RuntimeBattleCharacter CharacterPrefab => m_characterPrefab;

    [Header("Стихия персонажа")]
    public CharactersLibrary.CharacterElement Element;
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

    [Header("Описание персонажа при встрече")]
    public string meetingDescr;
    [Header("Встретил ли игрок этого персонажа?")]
    public bool isMeeted;

    [Header("Имя")]
    public string Name;
    [Header("Здоровье")]
    public double MaxHealth;
    [Header("Реген. здоровья")]
    public double HealthRegen;
    [Header("Реген. маны")]
    public double ManaRegen;
    [Header("Тип защиты")]
    public CharactersLibrary.CharacterArmorType ArmorType;
    [Header("Защита")]
    public double Armor;
    [Header("Мана")]
    public double MaxMana;
    [Header("Заклинания персонажа")]
    public CharacterMagic[] Spells;
    [Header("Тип атаки")]
    public CharactersLibrary.CharacterAttackType AttackType;
    [Header("Скорость атаки")]
    public double AttackSpeed;
    [Header("Урон")]
    public double Damage;
    [Header("Шанс увернуться | <= чем указанное число")]
    public double DodgeChance;
    [Header("Опыт")]
    public double Exp;
    [Header("Золото")]
    public double Gold;
    [Header("Дроп")]
    public ItemProfile[] DropItems;

    [Header("Текстура персонажа")]
    public Texture CharacterImage;

    [HideInInspector()]
    public int lastMagicPriority;
}
