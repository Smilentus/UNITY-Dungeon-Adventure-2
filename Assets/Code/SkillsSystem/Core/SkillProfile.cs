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
        ///     ������� ������ ������, �� ����� ����������� � �.�.
        /// </summary>
        public SkillCore skillCorePrefab { get => m_skillCorePrefab; }


        [SerializeField] 
        private List<SkillLevelData> m_skillLevelDatas;
        /// <summary>
        ///     ������ ��������� ������
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
        ///     ��� �������, ����� �������� �������� ����� ������� ��� �������� �� ���� �����
        /// </summary>
        public int PassSkillLevel => _passSkillLevel;


        [SerializeField] 
        private string _skillLevelTitle;
        /// <summary>
        ///     ������������ ��������� 
        /// </summary>
        public string skillLevelTitle => _skillLevelTitle; 


        [TextArea(5, 10)]
        [SerializeField] 
        private string _skillLevelDescription;
        /// <summary>
        ///     ��������� �������� ��������� ������
        /// </summary>
        public string skillLevelDescription => _skillLevelDescription; 


        [SerializeField] 
        private Sprite _skillLevelIcon;
        /// <summary>
        ///     ������� ������ ��������� (���� � ��� ������������ ������ ������)
        /// </summary>
        public Sprite skillLevelIcon => _skillLevelIcon;
    }
}