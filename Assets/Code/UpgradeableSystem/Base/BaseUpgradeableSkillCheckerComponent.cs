using Dimasyechka.Code.SkillsSystem.Core;
using UnityEngine;
using Zenject;

namespace Dimasyechka.Code.UpgradeableSystem.Base
{
    public class BaseUpgradeableSkillCheckerComponent : BaseUpgradeableCheckerComponent
    {
        [SerializeField]
        protected int _skillPointsCost;

        [SerializeField]
        protected int _skillPointsCostRaiseValue;

        [Tooltip("Увеличение цены навыка на Y каждый X уровень")]
        [SerializeField]
        protected int _everyLevelRaiseSkillPoints;


        protected RuntimePlayer _runtimePlayer;

        [Inject]
        public void Construct(RuntimePlayer runtimePlayer)
        {
            _runtimePlayer = runtimePlayer;
        }


        public override void LoadUpgradeableLevel(int level)
        {
            base.LoadUpgradeableLevel(level);

            for (int l = 0; l < level; l++)
            {
                CalculateSkillCost(l);
            }
        }

        public override string GetDescription()
        {
            return $"Необходимо {_skillPointsCost} ОН";
        }

        public override bool CanUpgrade()
        {
            return _skillPointsCost <= _runtimePlayer.RuntimePlayerStats.SkillPoints.Value;
        }

        public override void PostUpgrade()
        {
            base.PostUpgrade();

            SkillCore skillCore = attachedCore as SkillCore;
            CalculateSkillCost(skillCore.UpgradeableComponent.currentLevel);
        }

        protected override void ProcessUpgrade()
        {
            _runtimePlayer.RuntimePlayerStats.SkillPoints.Value -= _skillPointsCost;
        }

    
        protected void CalculateSkillCost(int skillLevel)
        {
            if (skillLevel % _everyLevelRaiseSkillPoints == 0)
            {
                _skillPointsCost += _skillPointsCostRaiseValue;
            }
        }
    }
}