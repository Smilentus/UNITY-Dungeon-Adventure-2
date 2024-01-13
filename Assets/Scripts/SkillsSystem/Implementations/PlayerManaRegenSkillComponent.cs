using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManaRegenSkillComponent : BaseSkillComponent
{
    public override void OnUpgraded(int level)
    {
        double delta = skillCore.UpgradeableComponent.currentLevel * 0.2f;

        RuntimePlayer.Instance.RuntimePlayerStats.ManaRegen += delta;
    }
}