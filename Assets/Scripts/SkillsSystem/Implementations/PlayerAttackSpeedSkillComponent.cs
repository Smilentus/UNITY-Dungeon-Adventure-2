using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSpeedSkillComponent : BaseSkillComponent
{
    public override List<string> GetDeltaValues(int level)
    {
        return new List<string>() 
        {
            "Скорость атаки +1 ед."
        };
    }

    public override void OnUpgraded(int level)
    {
        double delta = 1;

        RuntimePlayer.Instance.RuntimePlayerStats.AttackSpeed += delta;
    }
}