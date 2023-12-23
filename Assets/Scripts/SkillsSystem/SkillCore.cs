using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCore : Core
{
    [SerializeField] private UpgradeableComponent m_upgradeableComponent;


    public bool isSkillReachedMax => m_upgradeableComponent.reachedMaxUpgrades;


    public override void BuildWithComponents()
    {
        base.BuildWithComponents();
    }
}
