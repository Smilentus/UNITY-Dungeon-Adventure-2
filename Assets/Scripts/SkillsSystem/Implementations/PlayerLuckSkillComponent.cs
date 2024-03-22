using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLuckSkillComponent : BaseSkillComponent
{
    public override List<string> GetDeltaValues(int level)
    {
        return new List<string>()
        {
            $"Удача + 0.1%"
        };
    }

    public override void OnUpgraded(int level)
    {
        double delta = 0.1f;

        RuntimePlayer.Instance.RuntimePlayerStats.Luck += delta;
    }
}