using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExtraExpSkillComponent : BaseSkillComponent
{
    public override List<string> GetDeltaValues(int level)
    {
        return new List<string>()
        {
            $"Доп. опыт +10%"
        };
    }

    public override void OnUpgraded(int level)
    {
        double delta = 0.1f;

        RuntimePlayer.Instance.RuntimePlayerStats.ExtraExpMultiplier += delta;
    }
}