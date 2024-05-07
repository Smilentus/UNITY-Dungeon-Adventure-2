using System.Collections.Generic;
using Dimasyechka.Code.SkillsSystem.Implementations.Base;

namespace Dimasyechka.Code.SkillsSystem.Implementations
{
    public class PlayerHealthSkillComponent : BaseSkillComponent
    {
        public override List<string> GetDeltaValues(int level)
        {
            return new List<string>()
            {
                $"Макс. здоровье +{level * 10} ед.",
                $"Здоровье +{level * 10} ед."
            };
        }

        public override void OnUpgraded(int level)
        {
            double delta = _skillCore.UpgradeableComponent.currentLevel * 10;
            _runtimePlayer.RuntimePlayerStats.MaxHealth.Value += delta;
            _runtimePlayer.RuntimePlayerStats.Health.Value += delta;
        }
    }
}