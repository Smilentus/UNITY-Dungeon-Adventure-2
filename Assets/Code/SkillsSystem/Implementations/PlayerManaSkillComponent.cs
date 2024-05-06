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
                $"Мана +{level * 5}"
            };
        }

        public override void OnUpgraded(int level)
        {
            double delta = level * 5;

            RuntimePlayer.Instance.RuntimePlayerStats.MaxMana += delta;
            RuntimePlayer.Instance.RuntimePlayerStats.Mana += delta;
        }
    }
}