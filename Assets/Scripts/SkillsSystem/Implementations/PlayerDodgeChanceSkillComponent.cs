using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeChanceSkillComponent : BaseSkillComponent
{
    public override void OnUpgraded(int level)
    {
        double delta = 1f;

        RuntimePlayer.Instance.RuntimePlayerStats.DodgeChance += delta;
    }
}
