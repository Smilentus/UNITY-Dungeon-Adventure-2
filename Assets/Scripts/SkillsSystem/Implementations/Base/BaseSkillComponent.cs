using System.Collections.Generic;


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
