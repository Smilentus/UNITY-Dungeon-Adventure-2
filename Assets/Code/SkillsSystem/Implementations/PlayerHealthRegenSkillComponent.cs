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
                $"�������������� �������� +{level * 1} ��."
            };
        }

        public override void OnUpgraded(int level)
        {
            double delta = _skillCore.UpgradeableComponent.currentLevel * 1;

            _runtimePlayer.RuntimePlayerStats.HealthRegen.Value += delta;
        }
    }
}