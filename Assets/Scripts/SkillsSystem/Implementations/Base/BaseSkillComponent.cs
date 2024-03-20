using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseSkillComponent : CoreComponent
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


    public virtual void OnUpgraded(int level) {  }
    public virtual void OnMaxLvlReached() { }
}
