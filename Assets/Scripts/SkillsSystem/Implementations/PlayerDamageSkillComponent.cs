using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageSkillComponent : BaseSkillComponent
{
    public override void OnUpgraded(int level)
    {
        double delta = skillCore.UpgradeableComponent.currentLevel * 2;

        RuntimePlayer.Instance.RuntimePlayerStats.Damage += delta;
    }
}