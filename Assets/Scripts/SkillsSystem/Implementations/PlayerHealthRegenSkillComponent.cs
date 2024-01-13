using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthRegenSkillComponent : BaseSkillComponent
{
    public override void OnUpgraded(int level)
    {
        double delta = skillCore.UpgradeableComponent.currentLevel * 0.5f;

        RuntimePlayer.Instance.RuntimePlayerStats.HealthRegen += delta;
    }
}