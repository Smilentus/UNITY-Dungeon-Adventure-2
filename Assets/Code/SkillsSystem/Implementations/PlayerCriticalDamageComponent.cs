using System.Collections.Generic;
using Dimasyechka.Code.SkillsSystem.Implementations.Base;

namespace Dimasyechka.Code.SkillsSystem.Implementations
{
    public class PlayerCriticalDamageComponent : BaseSkillComponent
    {
        private double delta = 0.1f;


        public override List<string> GetDeltaValues(int level)
        {
            return new List<string>() {
                $"Урон критического удара +{(delta * 100f).ToString("f2")}%"
            };
        }

        public override void OnUpgraded(int level)
        {
            RuntimePlayer.Instance.RuntimePlayerStats.CriticalStrikeDamageMultiplier += delta;
        }
    }
}