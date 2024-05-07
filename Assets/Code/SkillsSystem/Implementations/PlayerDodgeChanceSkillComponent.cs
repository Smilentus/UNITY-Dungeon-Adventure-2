using System.Collections.Generic;
using Dimasyechka.Code.SkillsSystem.Implementations.Base;

namespace Dimasyechka.Code.SkillsSystem.Implementations
{
    public class PlayerDodgeChanceSkillComponent : BaseSkillComponent
    {
        private double delta = 1f;


        public override List<string> GetDeltaValues(int level)
        {
            return new List<string>()
            {
                $"Шанс уклонения +{(delta).ToString("f2")}%"
            };
        }

        public override void OnUpgraded(int level)
        {
            _runtimePlayer.RuntimePlayerStats.DodgeChance.Value += delta;
        }
    }
}
