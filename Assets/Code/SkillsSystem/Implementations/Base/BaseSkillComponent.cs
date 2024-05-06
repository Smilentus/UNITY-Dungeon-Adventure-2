using System.Collections.Generic;
using Dimasyechka.Code.CoreComponentSystem.Core;
using Dimasyechka.Code.CoreComponentSystem.Interfaces;
using Dimasyechka.Code.SkillsSystem.Core;
using Zenject;

namespace Dimasyechka.Code.SkillsSystem.Implementations.Base
{
    public abstract class BaseSkillComponent : CoreComponent
    {
        protected SkillCore _skillCore;


        protected RuntimePlayer _runtimePlayer;

        [Inject]
        public void Construct(RuntimePlayer runtimePlayer)
        {
            _runtimePlayer = runtimePlayer;
        }


        public override void InjectComponent(ICore core)
        {
            base.InjectComponent(core);

            _skillCore = (SkillCore)core;

            _skillCore.UpgradeableComponent.OnUpgraded += OnUpgraded;
            _skillCore.UpgradeableComponent.OnMaxUpgradesReached += OnMaxLvlReached;
        }


        public override void OnDestroyHandler()
        {
            base.OnDestroyHandler();

            _skillCore.UpgradeableComponent.OnUpgraded -= OnUpgraded;
            _skillCore.UpgradeableComponent.OnMaxUpgradesReached -= OnMaxLvlReached;
        }

        public virtual void OnMaxLvlReached() { }
        public abstract void OnUpgraded(int level);

        public abstract List<string> GetDeltaValues(int level);
    }
}
