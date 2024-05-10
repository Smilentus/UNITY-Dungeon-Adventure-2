using System.Collections.Generic;
using System.Linq;
using Dimasyechka.Code.SkillsSystem.Implementations.Base;
using Dimasyechka.Code.UpgradeableSystem;
using UnityEngine;

namespace Dimasyechka.Code.SkillsSystem.Core
{
    public class SkillCore : CoreComponentSystem.Core.Core
    {
        [SerializeField]
        protected SkillProfile _skillProfile;
        public SkillProfile SkillProfile => _skillProfile;


        [SerializeField] 
        protected UpgradeableComponent _upgradeableComponent;
        public UpgradeableComponent UpgradeableComponent => _upgradeableComponent;


        public bool isSkillReachedMax => _upgradeableComponent.reachedMaxUpgrades;


        private List<BaseSkillComponent> _skillComponents = new List<BaseSkillComponent>();

    
        // Переделай, дружище =(
        public List<string> GetSkillDeltaValues(int level)
        {
            if (_skillComponents.Count == 0)
                _skillComponents = GetComponentsInChildren<BaseSkillComponent>().ToList();
        
            List<string> output = new List<string>();

            foreach (BaseSkillComponent baseSkillComponent in _skillComponents)
            {
                output.AddRange(baseSkillComponent.GetDeltaValues(level));
            }

            return output;
        }


        public override void BuildWithComponents()
        {
            base.BuildWithComponents();
        }


        public void LoadLevel(int level)
        {
            _upgradeableComponent.LoadLevel(level);
        }


        public bool TryUpgradeSkill()
        {
            return _upgradeableComponent.TryAddLevel();
        }
    }
}