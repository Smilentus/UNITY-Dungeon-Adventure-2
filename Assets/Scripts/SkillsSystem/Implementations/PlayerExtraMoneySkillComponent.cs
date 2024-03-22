using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExtraMoneySkillComponent : BaseSkillComponent
{
    public override List<string> GetDeltaValues(int level)
    {
        return new List<string>()
        {
            $"Доп. золото +10%"
        };
    }

    public override void OnUpgraded(int level)
    {
        double delta = 0.1f;

        RuntimePlayer.Instance.RuntimePlayerStats.ExtraMoneyMultiplier += delta;
    }
}