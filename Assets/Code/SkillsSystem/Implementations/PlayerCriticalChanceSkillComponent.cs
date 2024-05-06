using System.Collections.Generic;
using Dimasyechka.Code.SkillsSystem.Implementations.Base;

namespace Dimasyechka.Code.SkillsSystem.Implementations
{
    public class PlayerCriticalChanceSkillComponent : BaseSkillComponent
    {
        public override List<string> GetDeltaValues(int level)
        {
            return new List<string>()
            {
                $"Шанс нанести критический урон +15%"
            };
        }

        public override void OnUpgraded(int level)
        {
            double delta = 1.5f;

            _runtimePlayer.RuntimePlayerStats.CriticalStrikeChance += delta;
        }
    }
}