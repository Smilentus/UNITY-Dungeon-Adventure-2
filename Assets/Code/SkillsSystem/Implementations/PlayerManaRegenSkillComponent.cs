using System.Collections.Generic;
using Dimasyechka.Code.SkillsSystem.Implementations.Base;

namespace Dimasyechka.Code.SkillsSystem.Implementations
{
    public class PlayerManaRegenSkillComponent : BaseSkillComponent
    {
        public override List<string> GetDeltaValues(int level)
        {
            return new List<string>()
            {
                $"Восстановление маны +{level * 0.2f}"
            };
        }

        public override void OnUpgraded(int level)
        {
            double delta = _skillCore.UpgradeableComponent.currentLevel * 0.2f;

            _runtimePlayer.RuntimePlayerStats.ManaRegen.Value += delta;
        }
    }
}