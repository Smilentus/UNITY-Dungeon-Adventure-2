using System.Collections.Generic;
using Dimasyechka.Code.SkillsSystem.Implementations.Base;

namespace Dimasyechka.Code.SkillsSystem.Implementations
{
    public class PlayerArmorSkillComponent : BaseSkillComponent
    {
        public override List<string> GetDeltaValues(int level)
        {
            return new List<string>() 
            {
                "������ +1 ��."
            };
        }

        public override void OnUpgraded(int level)
        {
            double delta = 1f;

            _runtimePlayer.RuntimePlayerStats.Armor += delta;
        }
    }
}
