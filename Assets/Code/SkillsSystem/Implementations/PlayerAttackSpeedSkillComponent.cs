using System.Collections.Generic;
using Dimasyechka.Code.SkillsSystem.Implementations.Base;

namespace Dimasyechka.Code.SkillsSystem.Implementations
{
    public class PlayerAttackSpeedSkillComponent : BaseSkillComponent
    {
        public override List<string> GetDeltaValues(int level)
        {
            return new List<string>() 
            {
                "Скорость атаки +1 ед."
            };
        }

        public override void OnUpgraded(int level)
        {
            double delta = 1;

            _runtimePlayer.RuntimePlayerStats.AttackSpeed += delta;
        }
    }
}