using System.Collections.Generic;
using Dimasyechka.Code.SkillsSystem.Implementations.Base;

namespace Dimasyechka.Code.SkillsSystem.Implementations
{
    public class PlayerDamageSkillComponent : BaseSkillComponent
    {
        double delta = 2f;

        public override List<string> GetDeltaValues(int level)
        {
            return new List<string>()
            {
                $"Наносимый урон +{level * delta}"
            };
        }

        public override void OnUpgraded(int level)
        {
            _runtimePlayer.RuntimePlayerStats.Damage.Value += _skillCore.UpgradeableComponent.currentLevel * delta;
        }
    }
}