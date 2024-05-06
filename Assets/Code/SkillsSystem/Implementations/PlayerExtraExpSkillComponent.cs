using System.Collections.Generic;
using Dimasyechka.Code.SkillsSystem.Implementations.Base;

namespace Dimasyechka.Code.SkillsSystem.Implementations
{
    public class PlayerExtraExpSkillComponent : BaseSkillComponent
    {
        public override List<string> GetDeltaValues(int level)
        {
            return new List<string>()
            {
                $"Доп. опыт +10%"
            };
        }

        public override void OnUpgraded(int level)
        {
            double delta = 0.1f;

            _runtimePlayer.RuntimePlayerStats.ExtraExpMultiplier += delta;
        }
    }
}