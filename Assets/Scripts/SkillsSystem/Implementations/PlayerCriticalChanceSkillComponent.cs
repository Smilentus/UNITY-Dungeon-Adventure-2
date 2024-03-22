using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCriticalChanceSkillComponent : BaseSkillComponent
{
    public override List<string> GetDeltaValues(int level)
    {
        return new List<string>()
        {
            $"Шанс нанести критический урон +15%"
        };
    }

    public override void OnUpgraded(int level)
    {
        double delta = 1.5f;

        RuntimePlayer.Instance.RuntimePlayerStats.CriticalStrikeChance += delta;
    }
}