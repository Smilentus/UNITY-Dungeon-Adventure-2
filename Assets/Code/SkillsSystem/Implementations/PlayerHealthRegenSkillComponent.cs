using System.Collections.Generic;
using Dimasyechka.Code.SkillsSystem.Implementations.Base;

namespace Dimasyechka.Code.SkillsSystem.Implementations
{
    public class PlayerHealthRegenSkillComponent : BaseSkillComponent
    {
        public override List<string> GetDeltaValues(int level)
        {
            return new List<string>()
            {
                $"�������������� �������� +{level * 0.5f} ��."
            };
        }

        public override void OnUpgraded(int level)
        {
            double delta = skillCore.UpgradeableComponent.currentLevel * 0.5f;

            RuntimePlayer.Instance.RuntimePlayerStats.HealthRegen += delta;
        }
    }
}