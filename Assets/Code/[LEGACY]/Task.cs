using Dimasyechka.Code._LEGACY_.Inventory;
using UnityEngine;

namespace Dimasyechka.Code._LEGACY_
{
    [CreateAssetMenu(fileName = "Task", menuName = "Creatable/CreateTask")]
    public class Task : ScriptableObject
    {
        // Награда: список
        public enum taskReward
        {
            None, // Ничего
            Item, // Предмет
            Research, // Исследование (для деревни в будущем)
            Skill, // Спец. навык для игрока? 
            Recipe // Новый рецепт
        }

        // Тип задания
        public enum taskType
        {
            None,
            Active,
            Completed
        }

        [Header("Идентификатор задания")]
        public string taskID;
        [Header("Название задания")]
        public string taskHeader;
        [Header("Подробное писание задания")]
        [TextArea(5, 10)]
        public string taskDescr;

        [Space(20)]
        [Header("Предмет")]
        public ItemProfile rewardItem;
        [Header("Кол-во предметов")]
        public int rewardStack;
        // Добавить исследование
        // Добавить навык
        // Добавить рецепт
        [Header("Золото")]
        public int goldReward;
        [Header("Опыт")]
        public int expReward;

        // Тип награды
        public taskReward reward;

        // Статус задания
        public taskType status;
    }
}
