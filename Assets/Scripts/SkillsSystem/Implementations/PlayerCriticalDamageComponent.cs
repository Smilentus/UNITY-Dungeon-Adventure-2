using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCriticalDamageComponent : BaseSkillComponent
{
    public override void OnUpgraded(int level)
    {
        double delta = 0.1f;

        RuntimePlayer.Instance.RuntimePlayerStats.CriticalStrikeDamageMultiplier += delta;
    }
}