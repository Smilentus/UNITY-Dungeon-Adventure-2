using System.Collections.Generic;
using Dimasyechka.Code.SkillsSystem.Implementations.Base;

namespace Dimasyechka.Code.SkillsSystem.Implementations
{
    public class PlayerLuckSkillComponent : BaseSkillComponent
    {
        public override List<string> GetDeltaValues(int level)
        {
            return new List<string>()
            {
                $"Удача + 0.1%"
            };
        }

        public override void OnUpgraded(int level)
        {
            double delta = 0.1f;

            _runtimePlayer.RuntimePlayerStats.Luck.Value += delta;
        }
    }
}