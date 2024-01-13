using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCriticalChanceSkillComponent : BaseSkillComponent
{
    public override void OnUpgraded(int level)
    {
        double delta = 1.5f;

        RuntimePlayer.Instance.RuntimePlayerStats.CriticalStrikeChance += delta;
    }
}