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
            RuntimePlayer.Instance.RuntimePlayerStats.Damage += skillCore.UpgradeableComponent.currentLevel * delta;
        }
    }
}