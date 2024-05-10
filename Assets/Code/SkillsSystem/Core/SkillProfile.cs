using System.Collections.Generic;
using UnityEngine;

namespace Dimasyechka.Code.SkillsSystem.Core
{
    [CreateAssetMenu(fileName = "SkillProfile_", menuName = "Creatable/SkillsSystem/Create New Skill Profile")]
    public class SkillProfile : ScriptableObject
    {
        [SerializeField] 
        private string m_skillGUID;
        public string skillGUID => m_skillGUID;


        [SerializeField] 
        private SkillCore m_skillCorePrefab;
        /// <summary>
        ///     Главный префаб навыка, со всеми улучшениями и т.п.
        /// </summary>
        public SkillCore skillCorePrefab { get => m_skillCorePrefab; }


        [SerializeField] 
        private List<SkillLevelData> m_skillLevelDatas;
        /// <summary>
        ///     Список улучшений навыка
        /// </summary>
        public List<SkillLevelData> skillLevelDatas { get => m_skillLevelDatas; }

    
        public SkillLevelData GetLevelData(int level)
        {
            SkillLevelData returnableData = null;

            foreach (SkillLevelData skillLevelData in m_skillLevelDatas)
            {
                if (skillLevelData.PassSkillLevel <= level)
                {
                    returnableData = skillLevelData;
                }
            }

            return returnableData;
        }
    }

    [System.Serializable]
    public class SkillLevelData
    {
        [SerializeField] 
        private int _passSkillLevel;
        /// <summary>
        ///     Тот уровень, после которого основной навык заменит своё описание на этот навык
        /// </summary>
        public int PassSkillLevel => _passSkillLevel;


        [SerializeField] 
        private string _skillLevelTitle;
        /// <summary>
        ///     Наименование улучшения 
        /// </summary>
        public string skillLevelTitle => _skillLevelTitle; 


        [TextArea(5, 10)]
        [SerializeField] 
        private string _skillLevelDescription;
        /// <summary>
        ///     Подробное описание улучшения навыка
        /// </summary>
        public string skillLevelDescription => _skillLevelDescription; 


        [SerializeField] 
        private Sprite _skillLevelIcon;
        /// <summary>
        ///     Главная иконка улучшения (если её нет используется иконка навыка)
        /// </summary>
        public Sprite skillLevelIcon => _skillLevelIcon;
    }
}