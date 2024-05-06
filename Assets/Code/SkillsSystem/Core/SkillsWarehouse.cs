using System.Collections.Generic;
using UnityEngine;

namespace Dimasyechka.Code.SkillsSystem.Core
{
    [CreateAssetMenu(fileName = "SkillsWarehouse", menuName = "Creatable/SkillsSystem/Create New SkillsWarehouse")]
    public class SkillsWarehouse : ScriptableObject
    {
        [SerializeField]
        private List<SkillProfile> m_skillProfiles = new List<SkillProfile>();
        public List<SkillProfile> SkillProfiles => m_skillProfiles;


        public SkillProfile GetSkillProfileByGUID(string skillGUID)
        {
            return m_skillProfiles.Find(x => x.skillGUID == skillGUID);
        }
    }
}