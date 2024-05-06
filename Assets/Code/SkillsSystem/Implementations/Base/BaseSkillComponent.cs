using System.Collections.Generic;
using Dimasyechka.Code.CoreComponentSystem.Core;
using Dimasyechka.Code.CoreComponentSystem.Interfaces;
using Dimasyechka.Code.SkillsSystem.Core;

namespace Dimasyechka.Code.SkillsSystem.Implementations.Base
{
    public abstract class BaseSkillComponent : CoreComponent
    {
        protected SkillCore skillCore;


        public override void InjectComponent(ICore core)
        {
            base.InjectComponent(core);

            skillCore = (SkillCore)core;

            skillCore.UpgradeableComponent.OnUpgraded += OnUpgraded;
            skillCore.UpgradeableComponent.OnMaxUpgradesReached += OnMaxLvlReached;
        }


        public override void OnDestroyHandler()
        {
            base.OnDestroyHandler();

            skillCore.UpgradeableComponent.OnUpgraded -= OnUpgraded;
            skillCore.UpgradeableComponent.OnMaxUpgradesReached -= OnMaxLvlReached;
        }

        public virtual void OnMaxLvlReached() { }
        public abstract void OnUpgraded(int level);

        public abstract List<string> GetDeltaValues(int level);
    }
}
