using Dimasyechka.Code.BattleSystem.EnemiesSystem;
using Dimasyechka.Code.InventorySystem.BaseItem;
using Dimasyechka.Code.MagicSystem.Profiles;
using UnityEngine;

namespace Dimasyechka.Code.BattleSystem
{
    [CreateAssetMenu(fileName = "New Character", menuName = "Creatable/Create Character")]
    public class CharacterProfile : ScriptableObject
    {
        [SerializeField]
        private RuntimeBattleCharacter _characterPrefab;
        /// <summary>
        ///     Ссылка на основной префаб персонажа, который спавнится во время боя
        /// </summary>
        public RuntimeBattleCharacter CharacterPrefab => _characterPrefab;

        [Header("Стихия персонажа")]
        public CharacterElement Element;
        public string GetElementalString
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

        [Header("Описание персонажа при встрече")]
        public string MeetingDescr;

        [Header("Встретил ли игрок этого персонажа?")]
        public bool IsMeeted;

        [Header("Имя")]
        public string Name;

        [Header("Здоровье")]
        public double MaxHealth;

        [Header("Реген. здоровья")]
        public double HealthRegen;

        [Header("Реген. маны")]
        public double ManaRegen;

        [Header("Тип защиты")]
        public CharacterArmorType ArmorType;

        [Header("Защита")]
        public double Armor;

        [Header("Мана")]
        public double MaxMana;

        [Header("Заклинания персонажа")]
        public BaseMagicProfile[] MagicProfiles;

        [Header("Тип атаки")]
        public CharacterAttackType AttackType;

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
        public BaseItemProfile[] DropItems;

        [Header("Текстура персонажа")]
        public Texture CharacterImage;

        [Header("Action Points")]
        public int DefaultActionPoints = 5;
    }

    public enum CharacterAttackType
    {
        None,
        Ranged,
        Magic,
        Melee,
        Throwable
    }
    public enum CharacterArmorType
    {
        None,
        Light,
        Medium,
        Heavy,
        Magic
    }
    public enum CharacterElement
    {
        None,
        Wind,
        Water,
        Fire,
        Earth,
        Light,
        Dark
    }
}
