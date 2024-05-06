using UnityEngine;

namespace Dimasyechka.Code._LEGACY_
{
    [System.Serializable]
    public class CharacterMagic
    {
        public enum spellType
        {
            MagicShield,
            MagicSpawn,
            MagicFireball,
            MagicThunder,
            MagicStun,
            MagicHealthSteal,
            MagicManaSteal,
            MagicHealthHeal,
            MagicManaHeal,
        }

        [Header("Текущая магия")]
        public spellType currentSpell;

        [Header("Стоимость заклинания")]
        public int manaCost;

        [Header("Активное значение заклинания")]
        public double spellPower;

        [Header("Приоритет использования")]
        public int usePriority;
    }
}
