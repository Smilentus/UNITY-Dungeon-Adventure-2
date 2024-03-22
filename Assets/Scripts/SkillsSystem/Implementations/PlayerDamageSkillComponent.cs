using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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