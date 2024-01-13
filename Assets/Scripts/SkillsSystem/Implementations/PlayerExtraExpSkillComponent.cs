using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExtraExpSkillComponent : BaseSkillComponent
{
    public override void OnUpgraded(int level)
    {
        double delta = 0.1f;

        RuntimePlayer.Instance.RuntimePlayerStats.ExtraExpMultiplier += delta;
    }
}