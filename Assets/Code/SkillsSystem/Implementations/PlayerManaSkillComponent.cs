using System.Collections.Generic;
using Dimasyechka.Code.SkillsSystem.Implementations.Base;

namespace Dimasyechka.Code.SkillsSystem.Implementations
{
    public class PlayerManaSkillComponent : BaseSkillComponent
    {
        public override List<string> GetDeltaValues(int level)
        {
            return new List<string>()
            {
                $"���� +{level * 5}"
            };
        }

        public override void OnUpgraded(int level)
        {
            double delta = level * 5;

            _runtimePlayer.RuntimePlayerStats.MaxMana += delta;
            _runtimePlayer.RuntimePlayerStats.Mana += delta;
        }
    }
}